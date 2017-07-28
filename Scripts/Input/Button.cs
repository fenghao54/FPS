using UnityEngine;
using System;

[Serializable]
public class Button
{
    public string Name { get { return buttonName; } set { buttonName = value; } }

    public KeyCode Key { get { return buttonKey; } }

    [SerializeField]
    private string buttonName;

    [SerializeField]
    private KeyCode buttonKey;

    public Button(string name)
    {
        buttonName = name;
    }

    public Button(string name, KeyCode key)
    {
        buttonName = name;
        buttonKey = key;
    }
}