using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Achievement
{
    public int id;
    public Sprite image;
    public string title;
    public string description;
    public bool isUnlock;

    public Achievement(int id, string title, string description, bool isUnlock, Sprite image)
    {
        this.id = id;
        this.image = image;
        this.title = title;
        this.description = description;
        this.isUnlock = isUnlock;
    }
}
