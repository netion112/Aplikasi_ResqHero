using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Achievement
{
    public int id;
    public string title;
    public string description;

    public Achievement(int id, string title, string description)
    {
        this.id = id;
        this.title = title;
        this.description = description;
    }
}
