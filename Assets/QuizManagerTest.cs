using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuizManagerTest : MonoBehaviour
{
    private SaveManager _saveManager;
    private AchievementManager _achievementManager;

    void Start()
    {
        _saveManager = SaveManager.Instance;
        if (_saveManager != null)
        {
            if (_saveManager.CheckFirstTimeQuiz())
            {
                Debug.Log("achivement unlock");
                StartCoroutine(UnlockAchievementDelayed());
            }
        }
        else
        {
            Debug.LogError("SaveManager tidak ditemukan.");
        }
    }
    
    private IEnumerator UnlockAchievementDelayed()
    {
        // Load the scene where AchievementManager is located
        yield return new WaitForSeconds(0.1f); // Optional delay to ensure scene is loaded
        _achievementManager = FindObjectOfType<AchievementManager>();
        if (_achievementManager != null)
        {
            _achievementManager.UnlockAchievement("Bencana Pertamaku");
        }
        else
        {
            Debug.LogError("AchievementManager tidak ditemukan.");
        }
    }
}
