using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class LoadScene : MonoBehaviour
{
    public AudioClip clickSound;
    private AudioSource audioSource;
    public AudioMixer audioMixer;

    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = clickSound;
    }
    
    public void SceneManagemen(string loadScene)
    {
        StartCoroutine(PlayClickSoundAndChangeScene(loadScene));
    }
    
    private IEnumerator PlayClickSoundAndChangeScene(string sceneName)
    {
        // Play the click sound
        float bgmVolume = GetMixerVolume("SFXVolume");
        audioSource.volume = bgmVolume;
        audioSource.Play();
        
        // Wait until the sound finishes playing
        yield return new WaitForSecondsRealtime(clickSound.length);
        
        // Change the scene
        SceneManager.LoadScene(sceneName);
    }

    private float GetMixerVolume(string paramName)
    {
        float volume = 1f; // Default volume if parameter not found

        if (audioMixer != null)
        {
            audioMixer.GetFloat(paramName, out float dbVolume); // Get volume in decibels from Audio Mixer
            volume = Mathf.Pow(10f, dbVolume / 20f); // Convert decibels to linear scale
        }

        return volume;
    }
}
