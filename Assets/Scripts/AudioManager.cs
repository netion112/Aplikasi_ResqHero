using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

public class AudioManager : MonoBehaviour
{
    [Header("Audio Clips")]
    public AudioClip combinedClip;  // Menggabungkan audio clip untuk Prestasi, Main, HomeScreen, dan Halaman_Setting
    public AudioClip gameClip;
    public AudioClip materiClip;
    public AudioClip quizClip;
    public AudioClip[] buttonClickClips;

    [Header("Fade Settings")]
    public float fadeDuration = 1f;

    [Header("Volume Sliders")]
    public Slider masterVolumeSlider;
    public Slider backgroundMusicVolumeSlider;
    public Slider buttonClickVolumeSlider;

    [Header("Video Player")]
    public VideoPlayer videoPlayer;

    [Header("Ending Game Objects")]
    public GameObject[] endingGameObjects;

    [Header("Toggle for Sound")]
    public Toggle switchToggle;

    private AudioSource musicSource;
    private AudioSource buttonClickSource;

    private bool wasPlaying = false;
    private float backgroundMusicVolume = 1.0f;
    private float buttonClickVolume = 1.0f;

    // New variables for tracking audio state
    private static string currentClipName;
    private static float currentClipTime;

    private void Start()
    {
        SetupAudioSources();
        PlayBackgroundMusic();
        LoadSettings(); // Load settings on start
        SetupVolumeSliders();
        SetupToggle();
        LoadMusicPosition();

        if (videoPlayer != null)
        {
            videoPlayer.started += OnVideoStart;
            videoPlayer.loopPointReached += OnVideoEnd;
            videoPlayer.prepareCompleted += OnVideoEnd;
        }
    }

    private void Update()
    {
        if (endingGameObjects != null && endingGameObjects.Length > 0)
        {
            CheckEndingCondition();
        }

        if (videoPlayer != null)
        {
            if (wasPlaying && !videoPlayer.isPlaying)
            {
                OnVideoPause();
            }

            if (!wasPlaying && videoPlayer.isPlaying)
            {
                OnVideoStart(videoPlayer);
            }

            wasPlaying = videoPlayer.isPlaying;
        }

        if (musicSource != null && musicSource.isPlaying)
        {
            currentClipName = musicSource.clip.name;
            currentClipTime = musicSource.time;
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;

        if (videoPlayer != null)
        {
            videoPlayer.started -= OnVideoStart;
            videoPlayer.loopPointReached -= OnVideoEnd;
            videoPlayer.prepareCompleted -= OnVideoEnd;
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        PlayBackgroundMusic();
        LoadSettings(); // Load settings on scene load
    }

    private void SetupAudioSources()
    {
        if (musicSource == null)
        {
            musicSource = gameObject.AddComponent<AudioSource>();
            musicSource.loop = true;
        }

        if (buttonClickSource == null)
        {
            buttonClickSource = gameObject.AddComponent<AudioSource>();
        }
    }

    private void PlayBackgroundMusic()
    {
        string sceneName = SceneManager.GetActiveScene().name;
        string[] activeSceneName = sceneName.Split('_');
        string firstSceneWord = activeSceneName.Length > 0 ? activeSceneName[0] : string.Empty;

        AudioClip newClip = GetClipForScene(firstSceneWord);

        if (newClip != null && musicSource != null)
        {
            // Jika newClip adalah combinedClip, ganti tanpa fade
            if (newClip == combinedClip)
            {
                musicSource.clip = newClip;
                musicSource.volume = backgroundMusicVolume;
                if (currentClipName == newClip.name && currentClipTime > 0)
                {
                    musicSource.time = currentClipTime;
                }
                else
                {
                    currentClipTime = 0;
                }
                musicSource.Play();
            }
            // Jika clip berbeda dari yang sekarang diputar, atau jika musicSource tidak sedang memainkan apapun
            else if (musicSource.clip != newClip || !musicSource.isPlaying)
            {
                StartCoroutine(FadeOutInBackgroundMusic(newClip));
            }
            else
            {
                // Jika clip sama dan sudah bermain, sesuaikan volume saja
                musicSource.volume = backgroundMusicVolume;
            }
        }
        else if (newClip == null)
        {
            Debug.LogError($"No background music clip found for scene: {sceneName}");
        }
    }

    private IEnumerator FadeOutInBackgroundMusic(AudioClip newClip)
    {
        float startVolume = musicSource.volume;

        // Fade out
        if (musicSource.isPlaying)
        {
            for (float t = 0; t < fadeDuration; t += Time.deltaTime)
            {
                musicSource.volume = Mathf.Lerp(startVolume, 0, t / fadeDuration);
                yield return null;
            }
        }

        musicSource.Stop();

        // Ganti clip
        musicSource.clip = newClip;
        currentClipName = newClip.name;

        // Jika ada waktu tersimpan dan clipnya sama, gunakan waktu tersimpan
        if (currentClipName == newClip.name && currentClipTime > 0)
        {
            musicSource.time = currentClipTime;
        }
        else
        {
            currentClipTime = 0;
        }

        musicSource.Play();

        // Fade in
        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            musicSource.volume = Mathf.Lerp(0, backgroundMusicVolume, t / fadeDuration);
            yield return null;
        }

        musicSource.volume = backgroundMusicVolume;
    }

    private AudioClip GetClipForScene(string sceneName)
    {
        if (sceneName == "Prestasi" || sceneName == "Main" || sceneName == "HomeScreen" || sceneName == "Halaman" || sceneName == "SelectLevel")
        {
            return combinedClip;
        }
        else if (sceneName.StartsWith("Game"))
        {
            return gameClip;
        }
        else if (sceneName.StartsWith("Materi"))
        {
            return materiClip;
        }
        else if (sceneName.StartsWith("Quiz"))
        {
            return quizClip;
        }
        else
        {
            return null;
        }
    }

    private void CheckEndingCondition()
    {
        bool anyActive = false;

        foreach (var obj in endingGameObjects)
        {
            if (obj != null && obj.activeSelf)
            {
                anyActive = true;
                break;
            }
        }

        if (anyActive && musicSource != null)
        {
            musicSource.volume = 0; // Mute background music
        }
        else if (musicSource != null)
        {
            musicSource.volume = backgroundMusicVolumeSlider != null ? backgroundMusicVolumeSlider.value : backgroundMusicVolume; // Set background music volume to slider value
        }
    }

    public void PlayButtonClick(int clipIndex)
    {
        if (buttonClickClips != null && buttonClickClips.Length > 0 && clipIndex >= 0 && clipIndex < buttonClickClips.Length)
        {
            buttonClickSource.PlayOneShot(buttonClickClips[clipIndex], buttonClickVolume);
        }
        else
        {
            Debug.LogError("Button click clip not assigned or index out of range!");
        }
    }

    public void SetMasterVolume(float volume)
    {
        AudioListener.volume = volume;
        PlayerPrefs.SetFloat("MasterVolume", volume); // Save master volume
    }

    public void SetBackgroundMusicVolume(float volume)
    {
        backgroundMusicVolume = volume;
        if (musicSource != null)
        {
            musicSource.volume = volume;
        }
        PlayerPrefs.SetFloat("BackgroundMusicVolume", volume); // Save background music volume
    }

    public void SetButtonClickVolume(float volume)
    {
        buttonClickVolume = volume;
        PlayerPrefs.SetFloat("ButtonClickVolume", volume); // Save button click volume
    }

    private void SetupVolumeSliders()
    {
        if (masterVolumeSlider != null)
        {
            masterVolumeSlider.onValueChanged.AddListener(SetMasterVolume);
            masterVolumeSlider.value = PlayerPrefs.GetFloat("MasterVolume", 0.75f);
        }

        if (backgroundMusicVolumeSlider != null)
        {
            backgroundMusicVolumeSlider.onValueChanged.AddListener(SetBackgroundMusicVolume);
            backgroundMusicVolumeSlider.value = PlayerPrefs.GetFloat("BackgroundMusicVolume", 0.75f);
        }

        if (buttonClickVolumeSlider != null)
        {
            buttonClickVolumeSlider.onValueChanged.AddListener(SetButtonClickVolume);
            buttonClickVolumeSlider.value = PlayerPrefs.GetFloat("ButtonClickVolume", 0.75f);
        }
    }

    private void SetupToggle()
    {
        if (switchToggle != null)
        {
            switchToggle.onValueChanged.AddListener(delegate { ToggleSound(switchToggle.isOn); });
            switchToggle.isOn = PlayerPrefs.GetInt("ToggleSound", 1) == 1; // Load initial state
            ToggleSound(switchToggle.isOn); // Set initial state
        }
    }

    private void ToggleSound(bool isOn)
    {
        if (isOn)
        {
            SetMasterVolume(masterVolumeSlider != null ? masterVolumeSlider.value : 1.0f);
            if (musicSource != null)
            {
                StartCoroutine(FadeInMusic());
            }
        }
        else
        {
            StartCoroutine(FadeOutMusic());
        }
        PlayerPrefs.SetInt("ToggleSound", isOn ? 1 : 0);
    }

    private IEnumerator FadeOutMusic()
    {
        float startVolume = musicSource.volume;
        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            musicSource.volume = Mathf.Lerp(startVolume, 0, t / fadeDuration);
            yield return null;
        }
        musicSource.volume = 0;
        SetMasterVolume(0);
    }

    private IEnumerator FadeInMusic()
    {
        float targetVolume = backgroundMusicVolumeSlider != null ? backgroundMusicVolumeSlider.value : backgroundMusicVolume;
        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            musicSource.volume = Mathf.Lerp(0, targetVolume, t / fadeDuration);
            yield return null;
        }
        musicSource.volume = targetVolume;
    }

    private void OnVideoStart(VideoPlayer vp)
    {
        if (musicSource != null)
        {
            musicSource.volume = 0;
        }
    }

    private void OnVideoPause()
    {
        if (musicSource != null)
        {
            musicSource.volume = backgroundMusicVolumeSlider != null ? backgroundMusicVolumeSlider.value : backgroundMusicVolume;
        }
    }

    private void OnVideoEnd(VideoPlayer vp)
    {
        if (musicSource != null)
        {
            musicSource.volume = backgroundMusicVolumeSlider != null ? backgroundMusicVolumeSlider.value : backgroundMusicVolume;
        }
    }

    public void Refresh()
    {
        SetMasterVolume(PlayerPrefs.GetFloat("MasterVolume", 0.75f));
        SetBackgroundMusicVolume(PlayerPrefs.GetFloat("BackgroundMusicVolume", 0.75f));
        SetButtonClickVolume(PlayerPrefs.GetFloat("ButtonClickVolume", 0.75f));

        if (masterVolumeSlider != null)
        {
            masterVolumeSlider.value = PlayerPrefs.GetFloat("MasterVolume", 0.75f);
        }

        if (backgroundMusicVolumeSlider != null)
        {
            backgroundMusicVolumeSlider.value = PlayerPrefs.GetFloat("BackgroundMusicVolume", 0.75f);
        }

        if (buttonClickVolumeSlider != null)
        {
            buttonClickVolumeSlider.value = PlayerPrefs.GetFloat("ButtonClickVolume", 0.75f);
        }
    }

    private void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            SaveMusicPosition();
        }
    }

    private void OnApplicationQuit()
    {
        SaveMusicPosition();
    }

    private void SaveMusicPosition()
    {
        if (musicSource != null && musicSource.clip != null)
        {
            currentClipName = musicSource.clip.name;
            currentClipTime = musicSource.time;
            PlayerPrefs.SetString("CurrentClipName", currentClipName);
            PlayerPrefs.SetFloat("CurrentClipTime", currentClipTime);
            PlayerPrefs.Save();
        }
    }

    private void LoadMusicPosition()
    {
        currentClipName = PlayerPrefs.GetString("CurrentClipName", "");
        currentClipTime = PlayerPrefs.GetFloat("CurrentClipTime", 0f);
    }

    private void LoadSettings()
    {
        // Load and apply saved settings
        float masterVolume = PlayerPrefs.GetFloat("MasterVolume", 0.75f);
        float backgroundMusicVolume = PlayerPrefs.GetFloat("BackgroundMusicVolume", 0.75f);
        float buttonClickVolume = PlayerPrefs.GetFloat("ButtonClickVolume", 0.75f);
        bool toggleSound = PlayerPrefs.GetInt("ToggleSound", 1) == 1;

        SetMasterVolume(masterVolume);
        SetBackgroundMusicVolume(backgroundMusicVolume);
        SetButtonClickVolume(buttonClickVolume);
        ToggleSound(toggleSound);
    }
}
