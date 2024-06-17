using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class quiz : MonoBehaviour
{

    public void question(bool answer)
    {
        int score;
        if (answer)
        {
            score = PlayerPrefs.GetInt("score") + 10;
        }
        else
        {
            score = PlayerPrefs.GetInt("score") - 0;
            if (score < 0)
            {
                score = 0;
            }
        }
        PlayerPrefs.SetInt("score", score);

        gameObject.SetActive(false);
        transform.parent.GetChild(gameObject.transform.GetSiblingIndex()+1).gameObject.SetActive(true);
    }
}
