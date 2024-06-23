using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

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
    public Slider sfxVolumeSlider; // New slider for SFX volume

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        PlayBackgroundMusicForCurrentScene();
        SetupVolumeSliders();
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
}
