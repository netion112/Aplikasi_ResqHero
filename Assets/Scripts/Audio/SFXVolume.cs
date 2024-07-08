using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SFXVolume : MonoBehaviour
{
    private void Start()
    {
        Slider slider = GetComponent<Slider>();
        if (AudioManager.instance != null)
        {
            slider.value = PlayerPrefs.GetFloat("SFXVolume", 1f);
            slider.onValueChanged.AddListener(AudioManager.instance.SetSFXVolume);
        }
    }
}
