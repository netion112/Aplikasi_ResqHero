using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
    public AudioClip clickSound; // Drag and drop your audio clip here in the Inspector
    private AudioSource audioSource;

    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = clickSound;
    }
    
    public void SceneManagemen(string loadScene)
    {
        audioSource.Play();
        StartCoroutine(WaitAndChangeScene(loadScene));
    }
    
    private IEnumerator WaitAndChangeScene(string sceneName)
    {
        // Wait until the sound finishes playing
        yield return new WaitForSeconds(clickSound.length);
        
        // Change the scene
        SceneManager.LoadScene(sceneName);
    }
}
