using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Header("Audio Sources")]
    public AudioSource BGMSource;
    public AudioSource SFXSource;

    [Header("Audio Mixer")]
    public AudioMixer audioMixer;

    // Event untuk perubahan volume
    public static event Action<float> OnMasterVolumeChanged;
    public static event Action<float> OnBGMVolumeChanged;
    public static event Action<float> OnSFXVolumeChanged;

    private const string MasterVolumeParam = "MasterVolume";
    private const string BGMVolumeParam = "BGMVolume";
    private const string SFXVolumeParam = "SFXVolume";

    [Header("Audio Clips")]
    public AudioClip[] BGMClips;
    public AudioClip[] SFXClips;

    private AudioClip currentBGM;

    [Header("Fade Audio")]
    public float fadeDuration;

    [Header("Video Player")]
    public VideoPlayer videoPlayer;

    [Header("Volume Sliders")]
    public Slider masterVolumeSlider;
    public Slider BGMVolumeSlider;
    public Slider SFXVolumeSlider;

    private float originalBGMVolume;
    private float originalSFXVolume;

    private bool isVideoPlaying = false;
    private float lastPlaybackTime = 0f;
    private float pauseDetectionThreshold = 0.1f;

    public float masterVolume = 1f;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            originalBGMVolume = BGMSource.volume;
            originalSFXVolume = SFXSource.volume;

            InitializeAudioMixer();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        PlayBGMForCurrentScene();
        SetupVideoPlayerEvents();
        // SetupVolumeSliders();
    }

    private void InitializeAudioMixer()
    {
        SetVolume(MasterVolumeParam, PlayerPrefs.GetFloat(MasterVolumeParam, 1f));
        SetVolume(BGMVolumeParam, PlayerPrefs.GetFloat(BGMVolumeParam, originalBGMVolume));
        SetVolume(SFXVolumeParam, PlayerPrefs.GetFloat(SFXVolumeParam, originalSFXVolume));
    }

    public void SetMasterVolume(float volume)
    {
        SetVolume(MasterVolumeParam, volume);
        OnMasterVolumeChanged?.Invoke(volume);
    }

    public void SetBGMVolume(float volume)
    {
        SetVolume(BGMVolumeParam, volume);
        OnBGMVolumeChanged?.Invoke(volume);
    }

    public void SetSFXVolume(float volume)
    {
        SetVolume(SFXVolumeParam, volume);
        OnSFXVolumeChanged?.Invoke(volume);
    }

    private void SetVolume(string paramName, float volume)
    {
        float dbVolume = volume > 0 ? 20f * Mathf.Log10(volume) : -80f;
        audioMixer.SetFloat(paramName, dbVolume);
        PlayerPrefs.SetFloat(paramName, volume);
        PlayerPrefs.Save();

        if (paramName == BGMVolumeParam)
        {
            BGMSource.volume = volume * GetVolume(MasterVolumeParam);
        }
        else if (paramName == SFXVolumeParam)
        {
            SFXSource.volume = volume * GetVolume(MasterVolumeParam);
        }
        else if (paramName == MasterVolumeParam)
        {
            BGMSource.volume = GetVolume(BGMVolumeParam) * volume;
            SFXSource.volume = GetVolume(SFXVolumeParam) * volume;
        }
    }

    public float GetVolume(string paramName)
    {
        if (audioMixer.GetFloat(paramName, out float dbVolume))
        {
            return dbVolume > -79f ? Mathf.Pow(10f, dbVolume / 20f) : 0f;
        }
        return 1f;
    }

    private void SetupVideoPlayerEvents()
    {
        if (videoPlayer != null)
        {
            videoPlayer.started += OnVideoStarted;
            videoPlayer.loopPointReached += OnVideoEnded;
        }
    }

    private void Update()
    {
        if (videoPlayer != null)
        {
            if (videoPlayer.isPlaying && !isVideoPlaying)
            {
                OnVideoStarted(videoPlayer);
            }
            else if (isVideoPlaying)
            {
                if (Mathf.Approximately((float)videoPlayer.time, lastPlaybackTime))
                {
                    pauseDetectionThreshold -= Time.deltaTime;
                    if (pauseDetectionThreshold <= 0)
                    {
                        OnVideoPaused();
                    }
                }
                else
                {
                    lastPlaybackTime = (float)videoPlayer.time;
                    pauseDetectionThreshold = 0.1f;
                }
            }
        }
    }

    private void OnVideoStarted(VideoPlayer vp)
    {
        isVideoPlaying = true;
        StartCoroutine(FadeOutAllAudio());
    }

    private void OnVideoEnded(VideoPlayer vp)
    {
        isVideoPlaying = false;
        StartCoroutine(FadeInAllAudio());
    }

    private void OnVideoPaused()
    {
        isVideoPlaying = false;
        StartCoroutine(FadeInAllAudio());
    }

    private IEnumerator FadeOutAllAudio()
    {
        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / fadeDuration;

            BGMSource.volume = Mathf.Lerp(originalBGMVolume, 0f, t);
            SFXSource.volume = Mathf.Lerp(originalSFXVolume, 0f, t);

            yield return null;
        }
    }

    private IEnumerator FadeInAllAudio()
    {
        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / fadeDuration;

            BGMSource.volume = Mathf.Lerp(0f, originalBGMVolume, t);
            SFXSource.volume = Mathf.Lerp(0f, originalSFXVolume, t);

            yield return null;
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        PlayBGMForCurrentScene();
    }

    private void PlayBGMForCurrentScene()
    {
        Scene activeScene = SceneManager.GetActiveScene();
        string activeSceneName = activeScene.name;
        string[] words = activeSceneName.Split('_');

        string firstWord = words.Length > 0 ? words[0] : string.Empty;
        AudioClip selectedClip = null;

        if (firstWord == "HomeScreen" || firstWord == "Main" || firstWord == "Prestasi" || firstWord == "SelectLevel" || firstWord == "Halaman")
        {
            selectedClip = BGMClips[0]; // HomeScreen music
        }
        else if (firstWord == "Materi")
        {
            selectedClip = BGMClips[1];
        }
        else if (firstWord == "Quiz")
        {
            selectedClip = BGMClips[2];
        }
        else if (firstWord == "Game")
        {
            selectedClip = BGMClips[3];
        }

        if (selectedClip != null && selectedClip != currentBGM)
        {
            StartCoroutine(FadeOutInBGM(selectedClip));
        }
    }

    private IEnumerator FadeOutInBGM(AudioClip newClip)
    {
        float startVolume = BGMSource.volume;
        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            BGMSource.volume = Mathf.Lerp(startVolume, 0, t / fadeDuration);
            yield return null;
        }
        BGMSource.volume = 0;

        BGMSource.clip = newClip;
        BGMSource.loop = true;
        BGMSource.Play();
        currentBGM = newClip;

        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            BGMSource.volume = Mathf.Lerp(0, startVolume, t / fadeDuration);
            yield return null;
        }
        BGMSource.volume = startVolume;
    }
    
}