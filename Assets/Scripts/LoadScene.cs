using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
    public void SceneManagemen(string loadScene)
    {
        SceneManager.LoadScene(loadScene);
    }
}
