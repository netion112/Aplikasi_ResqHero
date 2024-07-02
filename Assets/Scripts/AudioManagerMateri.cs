using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class AudioManagerMateri : MonoBehaviour
{
    [Header("Audio Sources")]
    public AudioSource backgroundMusicSource;

    [Header("Video Player")]
    public VideoPlayer videoPlayer;

    private float originalBackgroundMusicVolume;
    private bool wasPlaying = false;

    private void Start()
    {
        // Store the original volume of the background music
        originalBackgroundMusicVolume = backgroundMusicSource.volume;

        // Subscribe to video player events
        if (videoPlayer != null)
        {
            videoPlayer.started += OnVideoStart;
            videoPlayer.loopPointReached += OnVideoEnd;
            videoPlayer.prepareCompleted += OnVideoEnd; // This event will fire when the video is stopped and prepared again
        }
    }

    private void OnDestroy()
    {
        // Unsubscribe from video player events
        if (videoPlayer != null)
        {
            videoPlayer.started -= OnVideoStart;
            videoPlayer.loopPointReached -= OnVideoEnd;
            videoPlayer.prepareCompleted -= OnVideoEnd;
        }
    }

    private void Update()
    {
        if (videoPlayer != null)
        {
            // Check if the video was playing and now is paused
            if (wasPlaying && !videoPlayer.isPlaying)
            {
                OnVideoPause();
            }

            // Check if the video was paused and now is playing
            if (!wasPlaying && videoPlayer.isPlaying)
            {
                OnVideoStart(videoPlayer);
            }

            wasPlaying = videoPlayer.isPlaying;
        }
    }

    private void OnVideoStart(VideoPlayer vp)
    {
        // Mute background music when video starts
        backgroundMusicSource.volume = 0;
    }

    private void OnVideoPause()
    {
        // Restore background music volume when video is paused
        backgroundMusicSource.volume = originalBackgroundMusicVolume;
    }

    private void OnVideoEnd(VideoPlayer vp)
    {
        // Restore background music volume when video ends or is stopped
        backgroundMusicSource.volume = originalBackgroundMusicVolume;
    }

    public void PlayBackgroundMusic(AudioClip clip)
    {
        if (backgroundMusicSource.clip != clip)
        {
            backgroundMusicSource.clip = clip;
            backgroundMusicSource.Play();
        }
    }

    public void SetBackgroundMusicVolume(float volume)
    {
        originalBackgroundMusicVolume = volume;
        backgroundMusicSource.volume = volume;
    }
}