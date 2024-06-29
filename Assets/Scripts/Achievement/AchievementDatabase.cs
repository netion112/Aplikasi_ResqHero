using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AchievementDatabase", menuName = "Scripts/Achievement")]
public class AchievementDatabase : ScriptableObject
{
    public List<Achievement> achievements = new List<Achievement>();

    public Achievement GetAchievementByKey(string key)
    {
        return achievements.Find(a => a.title == key);
    }

    public void UnlockAchievement(string key)
    {
        Achievement achievement = GetAchievementByKey(key);
        if (achievement != null)
        {
            achievement.isUnlock = true;
        }
    }

    public List<Achievement> GetAllAchievements()
    {
        return achievements;
    }
}