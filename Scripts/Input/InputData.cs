using System.Collections.Generic;
using UnityEngine;

public class InputData : ScriptableObject
{
    public List<Button> Buttons { get { return buttons; } }

    public List<Axis> Axes { get { return axes; } }

    [SerializeField]
    private List<Button> buttons = new List<Button>();

    [SerializeField]
    private List<Axis> axes = new List<Axis>();
}