using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;
using Random = UnityEngine.Random;

public class AudioManager : MonoBehaviour
{
    [Header("Audio Sources")]
    public AudioSource backgroundMusicSource;
    public AudioSource buttonClickSource;
    public AudioSource sfxSource;

    [Header("Audio Clips")]
    public AudioClip[] backgroundMusicClips;
    public AudioClip[] buttonClickClips;
    public AudioClip[] sfxClips;

    [Header("Volume Sliders")]
    public Slider masterVolumeSlider;
    public Slider backgroundMusicVolumeSlider;
    public Slider buttonClickVolumeSlider;
    public Slider sfxVolumeSlider;

    [Header("Video Player")]
    public VideoPlayer videoPlayer;

    [Header("Ending Game Objects")]
    public GameObject[] endingGameObjects; // Daftar game object yang mempengaruhi volume musik

    private float originalBackgroundMusicVolume;
    private float originalButtonClickVolume;
    private float originalSFXVolume;

    private void Awake()
    {
        // Uncomment this section if you want the AudioManager to persist between scenes
        // if (instance == null)
        // {
        //     instance = this;
        //     DontDestroyOnLoad(gameObject);
        // }
        // else
        // {
        //     Destroy(gameObject);
        // }
    }

    private void Start()
    {
        PlayBackgroundMusicForCurrentScene();
        SetupVolumeSliders();

        if (videoPlayer != null)
        {
            videoPlayer.started += OnVideoStart;
            videoPlayer.loopPointReached += OnVideoEnd;
        }

        // Store original volumes
        originalBackgroundMusicVolume = backgroundMusicSource.volume;
        originalButtonClickVolume = buttonClickSource.volume;
        originalSFXVolume = sfxSource.volume;
    }

    private void Update()
    {
        CheckEndingCondition();
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
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        PlayBackgroundMusicForCurrentScene();
    }

    private void PlayBackgroundMusicForCurrentScene()
    {
        int sceneIndex = SceneManager.GetActiveScene().buildIndex;
        if (sceneIndex < backgroundMusicClips.Length)
        {
            backgroundMusicSource.clip = backgroundMusicClips[sceneIndex];
            backgroundMusicSource.Play();
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

        if (anyActive)
        {
            backgroundMusicSource.volume = 0;
        }
        else
        {
            backgroundMusicSource.volume = originalBackgroundMusicVolume;
        }
    }

    public void PlayButtonClick()
    {
        int randomIndex = Random.Range(0, buttonClickClips.Length);
        buttonClickSource.clip = buttonClickClips[randomIndex];
        buttonClickSource.Play();
    }

    public void PlaySFX(int index)
    {
        if (index < sfxClips.Length)
        {
            sfxSource.clip = sfxClips[index];
            sfxSource.Play();
        }
    }

    public void SetMasterVolume(float volume)
    {
        AudioListener.volume = volume;
    }

    public void SetBackgroundMusicVolume(float volume)
    {
        backgroundMusicSource.volume = volume;
    }

    public void SetButtonClickVolume(float volume)
    {
        buttonClickSource.volume = volume;
    }

    public void SetSFXVolume(float volume)
    {
        sfxSource.volume = volume;
    }

    private void SetupVolumeSliders()
    {
        if (masterVolumeSlider != null)
        {
            masterVolumeSlider.onValueChanged.AddListener(SetMasterVolume);
            masterVolumeSlider.value = AudioListener.volume;
        }

        if (backgroundMusicVolumeSlider != null)
        {
            backgroundMusicVolumeSlider.onValueChanged.AddListener(SetBackgroundMusicVolume);
            backgroundMusicVolumeSlider.value = backgroundMusicSource.volume;
        }

        if (buttonClickVolumeSlider != null)
        {
            buttonClickVolumeSlider.onValueChanged.AddListener(SetButtonClickVolume);
            buttonClickVolumeSlider.value = buttonClickSource.volume;
        }

        if (sfxVolumeSlider != null)
        {
            sfxVolumeSlider.onValueChanged.AddListener(SetSFXVolume);
            sfxVolumeSlider.value = sfxSource.volume;
        }
    }

    private void OnVideoStart(VideoPlayer vp)
    {
        // Set all audio sources' volumes to 0 when the video starts
        backgroundMusicSource.volume = 0;
        buttonClickSource.volume = 0;
        sfxSource.volume = 0;
    }

    private void OnVideoEnd(VideoPlayer vp)
    {
        // Restore all audio sources' volumes when the video ends
        backgroundMusicSource.volume = originalBackgroundMusicVolume;
        buttonClickSource.volume = originalButtonClickVolume;
        sfxSource.volume = originalSFXVolume;
    }

    public void Refresh()
    {
        // Refresh the audio settings to original volumes
        backgroundMusicSource.volume = originalBackgroundMusicVolume;
        buttonClickSource.volume = originalButtonClickVolume;
        sfxSource.volume = originalSFXVolume;

        if (masterVolumeSlider != null)
        {
            masterVolumeSlider.value = AudioListener.volume;
        }

        if (backgroundMusicVolumeSlider != null)
        {
            backgroundMusicVolumeSlider.value = originalBackgroundMusicVolume;
        }

        if (buttonClickVolumeSlider != null)
        {
            buttonClickVolumeSlider.value = originalButtonClickVolume;
        }

        if (sfxVolumeSlider != null)
        {
            sfxVolumeSlider.value = originalSFXVolume;
        }
    }
}
