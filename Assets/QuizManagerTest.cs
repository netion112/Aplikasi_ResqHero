using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuizManagerTest : MonoBehaviour
{
    private SaveManager _saveManager;

    void Start()
    {
        _saveManager = SaveManager.Instance;
        if (_saveManager != null)
        {
            if (_saveManager.CheckFirstTimeQuiz())
            {
                Debug.Log("achivement unlock");
                _saveManager.UnlockAchievement("Bencana Pertamaku");
            }
        }
        else
        {
            Debug.LogError("SaveManager tidak ditemukan atau achievement manager tidak ditemukan.");
        }
    }
}
