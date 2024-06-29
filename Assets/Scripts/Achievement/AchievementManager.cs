using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AchievementManager : MonoBehaviour
{
    public GameObject achievementPrefab;
    public GameObject parentAchievement;
    public List<Achievement> achievementDatabase;
    // public static SaveManager Instance { get; private set; }

    void Start()
    {
        CreateAchievement();
    }

    void Update()
    {
        
    }

    public void CreateAchievement()
    {
        foreach (Achievement achievementData in achievementDatabase)
        {
            GameObject achievement = (GameObject)Instantiate(achievementPrefab);
            SetAchievementInfo(parentAchievement, achievement, achievementData);
        }
    }

    public void SetAchievementInfo(GameObject category, GameObject achievement, Achievement achievementData)
    {
        achievement.transform.SetParent(category.transform);
        achievement.transform.localScale = new Vector3(1,1,1);

        Transform spriteAchievement = achievement.transform.Find("Image");

        if (spriteAchievement != null)
        {
            Image iconComponent = spriteAchievement.GetComponentInChildren<Image>();
            if (iconComponent != null)
            {
                iconComponent.color = achievementData.isUnlock ? new Color(0.8f, 0.4f, 0.1f)  : new Color(0.5f, 0.5f, 0.5f); 
            }
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
        Achievement achievement = achievementDatabase.Find(a => a.title == achievementKey);
        if (achievement != null)
        {
            achievement.isUnlock = true;
            CreateAchievement();
        }
    }
}
