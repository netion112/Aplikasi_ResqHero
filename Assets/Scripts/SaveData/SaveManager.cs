using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance { get; private set; }
    public AchievementManager achievementManager;

    void Start()
    {
        LoadData();
    }
    
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Membuat SaveManager tetap ada di seluruh scene
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public bool CheckFirstTimeQuiz()
    {
        return !PlayerPrefs.HasKey("Bencana Pertamaku");
    }

    private void LoadData()
    {
        // Load other necessary data
    }
}