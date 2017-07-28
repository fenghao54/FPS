using System.Linq;
using UnityEngine;


public enum InputMode
{
    Buttons,
    Axes
}

public enum AxisType
{
    Unity,
    Custom
}

public class InputManager : MonoBehaviour
{
	public InputData InputData
	{ 
		get { return inputData; }
		set { inputData = value; }
	}

    [SerializeField]
    private InputData inputData;

    public void SetupDefaults()
    {
        AddAxis(new Axis("Horizontal", AxisType.Unity, "Horizontal"));
        AddAxis(new Axis("Vertical", AxisType.Unity, "Vertical"));

        AddAxis(new Axis("Mouse X", AxisType.Unity, "Mouse X"));
        AddAxis(new Axis("Mouse Y", AxisType.Unity, "Mouse Y"));

        AddButton(new Button("Sprint", KeyCode.LeftShift));
        AddButton(new Button("Attack", KeyCode.Mouse0));
        AddButton(new Button("Jump", KeyCode.Space));
        AddButton(new Button("Crouch", KeyCode.C));
        AddButton(new Button("Reload", KeyCode.R));

    }

    public void Clear(InputMode inputMode)
    {
        if (inputMode == InputMode.Axes)
            inputData.Axes.Clear();
        else if (inputMode == InputMode.Buttons)
            inputData.Buttons.Clear();
    }

    public void ClearAll()
    {
        inputData.Axes.Clear();

        inputData.Buttons.Clear();
    }

    public float GetAxis(string name)
    {
       
        Axis axis = FindAxis(name);
        float value = 0f;

        if (axis != null)
        {
            if (axis.AxisType == AxisType.Unity)
                value += UnityEngine.Input.GetAxis(axis.UnityAxisName);
            if (axis.AxisType == AxisType.Custom)
                value += -GetKeyPress(axis.NegativeKey) + GetKeyPress(axis.PositiveKey);
        }

        return Mathf.Clamp(value, -1f, 1f);
    }

	public float GetAxisRaw(string name)
	{
		Axis axis = FindAxis(name);
		float value = 0f;

		if (axis != null)
		{
			if (axis.AxisType == AxisType.Unity)
				value += UnityEngine.Input.GetAxisRaw(axis.UnityAxisName);
			if (axis.AxisType == AxisType.Custom)
				value += -GetKeyPress(axis.NegativeKey) + GetKeyPress(axis.PositiveKey);
		}

		return Mathf.Clamp(value, -1f, 1f);
	}

    public bool GetButton(string name)
    {
        Button button = FindButton(name);
        bool value = false;

        if (button != null)
            value = Input.GetKey(button.Key);

        return value;
    }

    public bool GetButtonDown(string name)
    {
        Button button = FindButton(name);
        bool value = false;

        if (button != null)
            value = Input.GetKeyDown(button.Key);

        return value;
    }

	public bool GetButtonUp(string name)
	{
		Button button = FindButton(name);
		bool value = false;

		if (button != null)
			value = Input.GetKeyUp(button.Key);

		return value;
	}

    private void AddButton(Button toAdd)
    {
        inputData.Buttons.Add(toAdd);
    }

    private void AddAxis(Axis toAdd)
    {
        inputData.Axes.Add(toAdd);
    }

    private Button FindButton(string name)
    {
		for(int i = 0;i < inputData.Buttons.Count;i ++)
		{
			if(name == inputData.Buttons[i].Name)
				return inputData.Buttons[i];
		}

		return null;
    }

    private Axis FindAxis(string name)
    {
		for(int i = 0;i < inputData.Axes.Count;i ++)
		{
			if(name == inputData.Axes[i].AxisName)
				return inputData.Axes[i];
		}

		return null;
    }

    private int GetKeyPress(KeyCode key)
    {
        if (Input.GetKey(key))
            return 1;

        return 0;
    }
}
