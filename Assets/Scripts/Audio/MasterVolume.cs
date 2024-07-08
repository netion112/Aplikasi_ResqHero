using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MasterVolume : MonoBehaviour
{
    private Slider slider;

    private void Start()
    {
        slider = GetComponent<Slider>();
        if (slider != null)
        {
            slider.value = PlayerPrefs.GetFloat("MasterVolume", 1f);
            slider.onValueChanged.AddListener(delegate { SetMasterVolume(slider.value); });
        }
    }

    private void SetMasterVolume(float volume)
    {
        if (AudioManager.instance != null)
        {
            AudioManager.instance.SetMasterVolume(volume);
        }
    }
}
