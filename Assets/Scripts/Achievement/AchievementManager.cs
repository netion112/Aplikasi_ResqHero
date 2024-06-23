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
        
        Transform container = achievement.transform.Find("Container Text Title Achivement");
        if (container != null)
        {
            TMP_Text[] textComponents = container.GetComponentsInChildren<TMP_Text>();
            foreach (TMP_Text textComponent in textComponents)
            {
                if (textComponent.name == "Title")
                {
                    textComponent.text = achievementData.title;
                }
                else if (textComponent.name == "Desc")
                {
                    textComponent.text = achievementData.description;
                }
            }
        }
        else
        {
            Debug.LogError("ContainerTextTitleAchievement not found in the achievement prefab.");
        }
    }
}
