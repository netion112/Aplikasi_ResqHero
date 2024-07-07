using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Achievement
{
    public int id;
    public Image logo;
    public string title;
    public string description;
    public bool isUnlock;

    public Achievement(int id,Image logo, string title, string description, bool isUnlock)
    {
        this.id = id;
        this.logo = logo;
        this.title = title;
        this.description = description;
        this.isUnlock = isUnlock;
    }
}
