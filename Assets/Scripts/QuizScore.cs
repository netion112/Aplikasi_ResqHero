using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Score : MonoBehaviour
{
    public TextMeshProUGUI scoreText; // GameObject untuk menampilkan angka skor
    public TextMeshProUGUI messageText; // GameObject untuk menampilkan pesan berdasarkan skor

    void Update()
    {
        if (scoreText != null && messageText != null)
        {
            int score = PlayerPrefs.GetInt("score");
            scoreText.text = score.ToString(); // Update angka skor

            messageText.text = GetScoreMessage(score); // Update pesan berdasarkan skor
        }
    }

    public void ResetPlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
        Debug.Log("PlayerPrefs have been reset.");

        // Reset angka skor ke 0
        if (scoreText != null)
        {
            scoreText.text = "0";
        }

        // Reset pesan berdasarkan skor
        if (messageText != null)
        {
            messageText.text = "Belajar lagi yuk!"; // Atur pesan default sesuai kebutuhan
        }
    }

    private string GetScoreMessage(int score)
    {
        if (score < 10)
        {
            return "Belajar lagi yuk!";
        }
        else if (score < 20)
        {
            return "Lumayan, lanjutkan!";
        }
        else if (score < 30)
        {
            return "Hebat!";
        }
        else if (score < 40)
        {
            return "Luar biasa!";
        }
        else if (score < 50)
        {
            return "Luar biasa!";
        }
        else if (score < 60)
        {
            return "Luar biasa!";
        }
        else if (score < 70)
        {
            return "Luar biasa!";
        }
        else if (score < 80)
        {
            return "Luar biasa!";
        }
        else if (score < 90)
        {
            return "Luar biasa!";
        }
        else if (score < 100)
        {
            return "Luar biasa!";
        }
        else
        {
            return "Kamu luar biasa!";
        }
    }
}
