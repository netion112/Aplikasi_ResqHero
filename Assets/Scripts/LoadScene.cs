using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{

    public string sceneName;
    public PlayableDirector playableDirector;
    
    public void SceneManagemen(string loadScene)
    {
        SceneManager.LoadScene(loadScene);
    }
    
    // Start is called before the first frame update
    void Start()
    {
        if (playableDirector != null)
        {
            playableDirector.stopped += OnPlayableDirectorStopped;
        }
        else
        {
            Debug.LogWarning("PlayableDirector not assigned.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    void OnPlayableDirectorStopped(PlayableDirector director)
    {
        if (director == playableDirector)
        {
            SceneManager.LoadScene(sceneName);
        }
    }

    void OnDestroy()
    {
        // Menghapus listener ketika objek dihancurkan untuk menghindari memory leak
        if (playableDirector != null)
        {
            playableDirector.stopped -= OnPlayableDirectorStopped;
        }
    }
}
