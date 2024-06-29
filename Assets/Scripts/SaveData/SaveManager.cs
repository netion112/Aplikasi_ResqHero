using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance { get; private set; }
    public AchievementDatabase achievementDatabase;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        LoadData();
        // PlayerPrefs.DeleteAll();
    }

    public bool CheckFirstTimeQuiz()
    {
        return !PlayerPrefs.HasKey("Bencana Pertamaku");
    }

    public void SaveData()
    {
        foreach (Achievement achievement in achievementDatabase.achievements)
        {
            PlayerPrefs.SetInt(achievement.title, achievement.isUnlock ? 1 : 0);
        }
        PlayerPrefs.Save();
    }

    public void LoadData()
    {
        foreach (Achievement achievement in achievementDatabase.achievements)
        {
            if (PlayerPrefs.HasKey(achievement.title))
            {
                achievement.isUnlock = PlayerPrefs.GetInt(achievement.title) == 1;
            }
        }
    }

    public void UnlockAchievement(string achievementKey)
    {
        achievementDatabase.UnlockAchievement(achievementKey);
        SaveData();
    }
}