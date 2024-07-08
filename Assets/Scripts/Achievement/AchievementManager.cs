using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AchievementManager : MonoBehaviour
{
     // public static AchievementManager Instance { get; private set; }

    public GameObject achievementPrefab;
    public GameObject parentAchievement;
    public AchievementDatabase achievementDatabase;
    
    public TMP_Text achievementStatus;
    public TMP_Text achievementPercent;
    public TMP_Text achievementPoint;
    public Slider slider;

    void Awake()
    {
        // if (Instance == null)
        // {
        //     Instance = this;
        //     DontDestroyOnLoad(gameObject);
        // }
        // else
        // {
        //     Destroy(gameObject);
        // }
    }

    void Start()
    {
        Initialize();
    }

    public void Initialize()
    {
        // Load achievement data from SaveManager
        achievementDatabase = SaveManager.Instance.achievementDatabase;
        CreateAchievement();
        GetTotalUnlockedAchievements();
    }

    public void CreateAchievement()
    {
        // Clear UI before recreating
        foreach (Transform child in parentAchievement.transform)
        {
            Destroy(child.gameObject);
        }

        foreach (Achievement achievementData in achievementDatabase.achievements)
        {
            GameObject achievement = Instantiate(achievementPrefab);
            SetAchievementInfo(parentAchievement, achievement, achievementData);
        }
    }

    public void SetAchievementInfo(GameObject category, GameObject achievement, Achievement achievementData)
    {
        achievement.transform.SetParent(category.transform);
        achievement.transform.localScale = new Vector3(1, 1, 1);

        Transform imageTransform = achievement.transform.Find("Image");
        if (imageTransform != null)
        {
            Image imageComponent = imageTransform.GetComponentInChildren<Image>();
            imageComponent.color = achievementData.isUnlock ? new Color(0.8f, 0.4f, 0.1f) : new Color(0.5f, 0.5f, 0.5f);
            
            Transform iconTransform = imageTransform.Find("Icon");
            if (iconTransform != null)
            {
                Image iconComponent = iconTransform.GetComponentInChildren<Image>();
                if (iconComponent != null)
                {
                    iconComponent.sprite = achievementData.image;
                }
                else
                {
                    Debug.LogError("Image component not found in the Icon GameObject.");
                }
            }
            else
            {
                Debug.LogError("Icon GameObject not found in the Image GameObject.");
            }
        }
        else
        {
            Debug.LogError("Image GameObject not found in the achievement prefab.");
        }

        Transform container = achievement.transform.Find("Container Text Title Achivement");
        if (container != null)
        {
            TMP_Text[] textComponents = container.GetComponentsInChildren<TMP_Text>();
            foreach (TMP_Text textComponent in textComponents)
            {
                if (textComponent.name == "Title")
                {
                    textComponent.text = achievementData.title;
                    textComponent.color = achievementData.isUnlock ? new Color(0.2f, 0.3f, 0.2f) : new Color(0.5f, 0.5f, 0.5f);
                }
                else if (textComponent.name == "Desc")
                {
                    textComponent.text = achievementData.description;
                    textComponent.color = achievementData.isUnlock ? new Color(0.8f, 0.4f, 0.1f) : new Color(0.5f, 0.5f, 0.5f);
                }
            }
        }
        else
        {
            Debug.LogError("ContainerTextTitleAchievement not found in the achievement prefab.");
        }
    }

    public void UnlockAchievement(string achievementKey)
    {
        Achievement achievement = achievementDatabase.achievements.Find(a => a.title == achievementKey);
        if (achievement != null)
        {
            achievement.isUnlock = true;
            SaveManager.Instance.SaveData(); // Save achievement data after unlocking
            CreateAchievement();
            GetTotalUnlockedAchievements();
        }
    }

    public void GetTotalUnlockedAchievements()
    {
        int unlockedCount = 0;
        
        foreach (Achievement achievement in achievementDatabase.achievements)
        {
            if (achievement.isUnlock)
            {
                unlockedCount++;
            }
        }

        achievementStatus.text = $"{unlockedCount} OF {achievementDatabase.achievements.Count} ACHIEVEMENTS EARNED";
        int achievementPercentage = Mathf.RoundToInt((float)unlockedCount / achievementDatabase.achievements.Count * 100f);
        achievementPercent.text = $"{achievementPercentage}%";
        achievementPoint.text = $"{unlockedCount}";
        slider.maxValue = achievementDatabase.achievements.Count;
        slider.value = unlockedCount;
    }
}