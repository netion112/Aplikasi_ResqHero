using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Score : MonoBehaviour
{
    private TextMeshProUGUI m_TextMeshPro;

    private void Start()
    {
        m_TextMeshPro = GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        if (m_TextMeshPro != null)
        {
            m_TextMeshPro.text = PlayerPrefs.GetInt("score").ToString();
        }
    }

    public void ResetPlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
        Debug.Log("PlayerPrefs have been reset.");

        // Optionally reset the score display
        if (m_TextMeshPro != null)
        {
            m_TextMeshPro.text = "0";
        }
    }
}
