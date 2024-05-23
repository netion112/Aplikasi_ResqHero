using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagement : MonoBehaviour
{
    void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    void Home()
    {
        SceneManager.LoadScene("Home");
    }

    void Game()
    {
        SceneManager.LoadScene(0);
    }
}
