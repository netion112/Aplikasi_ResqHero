using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuizManager : MonoBehaviour
{
    public List<GameObject> questions; // List of all question GameObjects
    public List<GameObject> questionButtons; // List of buttons corresponding to each question

    private void Start()
    {
        // Ensure only the first question is active at start
        ActivateQuestion(0);
    }

    public void ActivateQuestion(int questionIndex)
    {
        // Deactivate all questions
        foreach (GameObject question in questions)
        {
            question.SetActive(false);
        }

        // Activate the selected question
        if (questionIndex >= 0 && questionIndex < questions.Count)
        {
            questions[questionIndex].SetActive(true);
        }
        else
        {
            Debug.LogError("Question index out of range");
        }
    }

    // This method can be called when a button is clicked
    public void OnQuestionButtonClicked(int questionIndex)
    {
        ActivateQuestion(questionIndex);
    }
}
