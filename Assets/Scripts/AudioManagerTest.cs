using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AudioManagerTest : MonoBehaviour
{
    public static AudioManagerTest instance;

    [Header("Audio Sources")]
    public AudioSource backgroundMusicSource;
    public AudioSource buttonClickSource;
    public AudioSource sfxSource;

    [Header("Audio Clips")]
    public AudioClip[] backgroundMusicClips;
    public AudioClip[] buttonClickClips;
    public AudioClip[] sfxClips;
    
    private AudioClip currentBackgroundMusic;
    
    public float fadeDuration;

    // [Header("Volume Sliders")]
    // public Slider masterVolumeSlider;
    // public Slider backgroundMusicVolumeSlider;
    // public Slider buttonClickVolumeSlider;
    // public Slider sfxVolumeSlider; // New slider for SFX volume
    
    private float originalBackgroundMusicVolume;
    // private float originalButtonClickVolume;
    // private float originalSfxVolume;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            
            originalBackgroundMusicVolume = backgroundMusicSource.volume;
            // originalButtonClickVolume = buttonClickSource.volume;
            // originalSfxVolume = sfxSource.volume;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        PlayBackgroundMusicForCurrentScene();
        // SetupVolumeSliders();
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
        PlayBackgroundMusicForCurrentScene();
        // SetupVolumeSlidersFromPauseMenu();
    }

    private void PlayBackgroundMusicForCurrentScene()
    {
        Scene activeScene = SceneManager.GetActiveScene();
        string activeSceneName = activeScene.name;
        string[] words = activeSceneName.Split('_');

        string firstWord = words.Length > 0 ? words[0] : string.Empty;
        AudioClip selectedClip = null;
        
        if (firstWord == "Game" || firstWord == "Materi")
        {
            backgroundMusicSource.volume = 0;
            buttonClickSource.volume = 0;
            sfxSource.volume = 0;
        }
        else
        {
            backgroundMusicSource.volume = originalBackgroundMusicVolume;
            // buttonClickSource.volume = originalButtonClickVolume;
            // sfxSource.volume = originalSfxVolume;
        }

        if (firstWord == "HomeScreen" || firstWord == "Main" || firstWord == "Prestasi" || firstWord == "SelectLevel")
        {
            selectedClip = backgroundMusicClips[0]; // HomeScreen music
        }
        else if(firstWord == "Quiz")
        {
            selectedClip = backgroundMusicClips[3];
        }
        // else if (firstWord == "Materi")
        // {
        //     selectedClip = backgroundMusicClips[1]; // Materi music
        // }
        // else if (firstWord == "Game")
        // {
        //     selectedClip = backgroundMusicClips[3];
        // }

        if (selectedClip != null && selectedClip != currentBackgroundMusic)
        {
            StartCoroutine(FadeOutInBackgroundMusic(selectedClip));
        }
    }
    
    private IEnumerator FadeOutInBackgroundMusic(AudioClip newClip)
    {
        // Fade out
        float startVolume = backgroundMusicSource.volume;
        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            backgroundMusicSource.volume = Mathf.Lerp(startVolume, 0, t / fadeDuration);
            yield return null;
        }
        backgroundMusicSource.volume = 0;

        // Change the clip
        backgroundMusicSource.clip = newClip;
        backgroundMusicSource.loop = true;
        backgroundMusicSource.Play();
        currentBackgroundMusic = newClip;

        // Fade in
        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            backgroundMusicSource.volume = Mathf.Lerp(0, startVolume, t / fadeDuration);
            yield return null;
        }
        backgroundMusicSource.volume = startVolume;
    }

    // public void PlayButtonClick()
    // {
    //     int randomIndex = Random.Range(0, buttonClickClips.Length);
    //     buttonClickSource.clip = buttonClickClips[randomIndex];
    //     buttonClickSource.Play();
    // }
    //
    // public void PlaySFX(int index)
    // {
    //     if (index < sfxClips.Length)
    //     {
    //         sfxSource.clip = sfxClips[index];
    //         sfxSource.Play();
    //     }
    // }
    //
    // public void SetMasterVolume(float volume)
    // {
    //     AudioListener.volume = volume;
    // }
    //
    // public void SetBackgroundMusicVolume(float volume)
    // {
    //     backgroundMusicSource.volume = volume;
    // }
    //
    // public void SetButtonClickVolume(float volume)
    // {
    //     buttonClickSource.volume = volume;
    // }
    //
    // public void SetSFXVolume(float volume)
    // {
    //     sfxSource.volume = volume;
    // }
    
    // private void SetupVolumeSlidersFromPauseMenu()
    // {
    //     GameObject pauseMenu = GameObject.Find("PauseMenu");
    //     Debug.Log(pauseMenu);
    //     if (pauseMenu != null)
    //     {
    //         masterVolumeSlider = pauseMenu.transform.Find("Master Volume Slider").GetComponent<Slider>();
    //         backgroundMusicVolumeSlider = pauseMenu.transform.Find("Background Music Volume Slider").GetComponent<Slider>();
    //         buttonClickVolumeSlider = pauseMenu.transform.Find("Button Click Volume Slider").GetComponent<Slider>();
    //         sfxVolumeSlider = pauseMenu.transform.Find("SFX Volume Slider").GetComponent<Slider>();
    //
    //         SetupVolumeSliders();
    //     }
    // }
    //
    // private void SetupVolumeSliders()
    // {
    //     if (masterVolumeSlider != null)
    //     {
    //         masterVolumeSlider.onValueChanged.AddListener(SetMasterVolume);
    //         masterVolumeSlider.value = AudioListener.volume;
    //     }
    //
    //     if (backgroundMusicVolumeSlider != null)
    //     {
    //         backgroundMusicVolumeSlider.onValueChanged.AddListener(SetBackgroundMusicVolume);
    //         backgroundMusicVolumeSlider.value = backgroundMusicSource.volume;
    //     }
    //
    //     if (buttonClickVolumeSlider != null)
    //     {
    //         buttonClickVolumeSlider.onValueChanged.AddListener(SetButtonClickVolume);
    //         buttonClickVolumeSlider.value = buttonClickSource.volume;
    //     }
    //
    //     if (sfxVolumeSlider != null)
    //     {
    //         sfxVolumeSlider.onValueChanged.AddListener(SetSFXVolume);
    //         sfxVolumeSlider.value = sfxSource.volume;
    //     }
    // }

}
