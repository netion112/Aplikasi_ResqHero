using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonMenu : MonoBehaviour
{
    public GameObject Button_Beranda;
    public GameObject Button_Materi;
    public GameObject Button_Prestasi;
    public GameObject Button_Bermain;

    void Start()
    {
        DetectActiveScene();
    }

    void DetectActiveScene()
    {
        Scene activeScene = SceneManager.GetActiveScene();
        string activeSceneName = activeScene.name;
        string[] words = activeSceneName.Split('_');

        string firstWord = words.Length > 0 ? words[0] : string.Empty;
        
        SetButtonState(Button_Beranda, false);
        SetButtonState(Button_Materi, false);
        SetButtonState(Button_Prestasi, false);
        SetButtonState(Button_Bermain, true);

        if (firstWord == "HomeScreen")
        {
            SetButtonState(Button_Beranda, true, true);
            SetButtonState(Button_Prestasi, true, false);
        }
        else if (firstWord == "Materi")
        {
            SetButtonState(Button_Beranda, true, false);
            SetButtonState(Button_Materi, true, true);
            SetButtonState(Button_Bermain, true, false);
        }
        else if (firstWord == "Prestasi")
        {
            SetButtonState(Button_Prestasi, true, true);
            SetButtonState(Button_Beranda, true, false);
        }
        else if (firstWord == "Bermain")
        {
            SetButtonState(Button_Beranda, true, false);
            SetButtonState(Button_Materi, true, false);
            SetButtonState(Button_Bermain, true, true);
        }
    }

    void SetButtonState(GameObject button, bool isActive, bool isMainButton = false)
    {
        button.SetActive(isActive);

        if (isActive)
        {
            Transform inactiveButton = button.transform.Find("Inactive_" + button.name);
            Transform activeButton = button.transform.Find("Active_" + button.name);

            if (inactiveButton != null && activeButton != null)
            {
                inactiveButton.gameObject.SetActive(!isMainButton);
                activeButton.gameObject.SetActive(isMainButton);
            }
            else
            {
                Debug.LogWarning("Inactive or Active child not found for " + button.name);
            }
        }
    }
}
