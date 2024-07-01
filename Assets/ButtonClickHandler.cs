using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonClickHandler : MonoBehaviour
{
    public AudioClip clickSound; // Drag and drop your audio clip here in the Inspector
    private AudioSource audioSource;

    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = clickSound;
    }

    public void OnButtonClick(string sceneName)
    {
        // Play the click sound
        audioSource.Play();
        
        // Start coroutine to wait for the sound to finish before changing the scene
        StartCoroutine(WaitAndChangeScene(sceneName));
    }

    private IEnumerator WaitAndChangeScene(string sceneName)
    {
        // Wait until the sound finishes playing
        yield return new WaitForSeconds(clickSound.length);
        
        // Change the scene
        SceneManager.LoadScene(sceneName);
    }
}
