using UnityEngine;


[System.Serializable]
public class Axis
{
    public string AxisName
    {
        get
        {
            return axisName;
        }

        set
        {
            axisName = value;
        }
    }

    public AxisType AxisType
    {
        get
        {
            return axisType;
        }
    }

    public string UnityAxisName
    {
        get
        {
            return unityAxisName;
        }
    }

    public KeyCode NegativeKey
    {
        get
        {
            return negativeKey;
        }
    }

    public KeyCode PositiveKey
    {
        get
        {
            return positiveKey;
        }
    }


    [SerializeField]
    private string axisName;


    [SerializeField]
    private AxisType axisType;

    [SerializeField]
    private string unityAxisName;

    [SerializeField]
    private KeyCode positiveKey;

    [SerializeField]
    private KeyCode negativeKey;

    public Axis(string name, AxisType type)
    {
        axisName = name;
        axisType = type;
    }

    public Axis(string name, AxisType type, string unityAxis)
    {
        axisName = name;
        axisType = type;

        unityAxisName = unityAxis;
    }

    public Axis(string name, AxisType type, KeyCode positive, KeyCode negative, string unityAxis)
    {
        axisName = name;
        axisType = type;

        positiveKey = positive;
        negativeKey = negative;

        unityAxisName = unityAxis;
    }
}
