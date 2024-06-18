using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonMenu : MonoBehaviour
{
    public GameObject Inactive_Button_Beranda;
    public GameObject Active_Button_Beranda;
    public GameObject Inactive_Button_Materi;
    public GameObject Active_Button_Materi;
    public GameObject Inactive_Button_Prestasi;
    public GameObject Active_Button_Prestasi;
    public GameObject Inactive_Button_Bermain;
    public GameObject Active_Button_Bermain;
    
    void Start()
    {
        DetectActiveScene();
    }

    void DetectActiveScene()
    {
        Scene activeScene  = SceneManager.GetActiveScene();
        string activeSceneName = activeScene.name;
        
        UpdateButtonState(Inactive_Button_Beranda, Active_Button_Beranda, activeSceneName == "HomeScreen");
        UpdateButtonState(Inactive_Button_Materi, Active_Button_Materi, activeSceneName == "Materi");
        UpdateButtonState(Inactive_Button_Prestasi, Active_Button_Prestasi, activeSceneName == "Prestasi");
        UpdateButtonState(Inactive_Button_Bermain, Active_Button_Bermain, activeSceneName == "Bermain");
    }
    
    void UpdateButtonState(GameObject inactiveButton, GameObject activeButton, bool isActive)
    {
        inactiveButton.SetActive(!isActive);
        activeButton.SetActive(isActive);
    }
}
