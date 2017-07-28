using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEditorInternal;

public class InputManagerWindow : EditorWindow
{
    SerializedObject inputDataSerialized;

    ReorderableList buttonReorderables;
    ReorderableList axesReorderables;

    InputManager inputManager;

    Vector2 scrollPosition;

    void OnEnable()
    {
        Init();
        EnableReorderableList();
    }

    void OnGUI()
    {
        if(inputManager == null || inputManager.InputData == null)
        {
            EditorGUILayout.LabelField("No InputData or InputManager found", GUIStyle.none);
            return;
        }
        RefreshGUI();
    }

    [MenuItem("Tools/Input Manager", false)]
    public static void OpenInputManager()
    {
        EditorWindow window = GetWindow<InputManagerWindow>(true, "Input Manager");

        window.maxSize = new Vector2(1000, 500);
        window.minSize = window.maxSize;
    }

    void Init()
    {
        if (!inputManager)
            inputManager = FindObjectOfType<InputManager>();

        if (inputDataSerialized == null)
            inputDataSerialized = new SerializedObject(inputManager.InputData);
    }

    void RefreshGUI()
    {
        inputDataSerialized.Update();
        EditorGUIUtility.labelWidth = 100;
        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);

        RefreshReorderableList();

        EditorGUILayout.BeginVertical();
        GUILayout.Space(20);

        EditorGUILayout.BeginHorizontal();
        GUILayout.Space(40);
        if (GUILayout.Button("Delete All"))
        {
            inputManager.ClearAll();
        }
        GUILayout.Space(20);
        if (GUILayout.Button("Reset All Defaults"))
        {
            inputManager.ClearAll();
            inputManager.SetupDefaults();
        }
        GUILayout.Space(40);
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.EndVertical();

        EditorGUILayout.EndScrollView();

        inputDataSerialized.ApplyModifiedProperties();
    }

    void RefreshReorderableList()
    {
        EditorGUILayout.BeginHorizontal();
        GUILayout.Space(40);
        EditorGUILayout.BeginVertical();
        GUILayout.Space(20);
        if (inputManager.InputData.Buttons.Count > 0 || buttonReorderables != null)
        {
            buttonReorderables.DoLayoutList();
        }
        GUILayout.Space(20);
        if (inputManager.InputData.Axes.Count > 0 || axesReorderables != null)
        {
            axesReorderables.DoLayoutList();
        }

        EditorGUILayout.EndVertical();
        GUILayout.Space(40);
        EditorGUILayout.EndHorizontal();
    }

    void EnableReorderableList()
    {
        SerializedProperty buttonsProperty = inputDataSerialized.FindProperty("buttons");

        buttonReorderables = new ReorderableList(inputDataSerialized, buttonsProperty);

        buttonReorderables.drawHeaderCallback = (Rect rect) =>
        {
            GUI.Label(rect, "Buttons");
        };

        buttonReorderables.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
        {
            EditorGUI.PropertyField(new Rect(rect.x, rect.y, 200, EditorGUIUtility.singleLineHeight), buttonsProperty.GetArrayElementAtIndex(index).FindPropertyRelative("buttonName"));

            EditorGUI.PropertyField(new Rect(rect.x + 250, rect.y, 200, EditorGUIUtility.singleLineHeight), buttonsProperty.GetArrayElementAtIndex(index).FindPropertyRelative("buttonKey"));
        };

        SerializedProperty axesProperty = inputDataSerialized.FindProperty("axes");

        axesReorderables = new ReorderableList(inputDataSerialized, axesProperty);
        axesReorderables.drawHeaderCallback = (Rect rect) =>
        {
            GUI.Label(rect, "Axes");
        };

        axesReorderables.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
        {
            EditorGUI.PropertyField(new Rect(rect.x, rect.y, 200, EditorGUIUtility.singleLineHeight), axesProperty.GetArrayElementAtIndex(index).FindPropertyRelative("axisName"));

            AxisType axisType = (AxisType)axesProperty.GetArrayElementAtIndex(index).FindPropertyRelative("axisType").enumValueIndex;

            EditorGUI.PropertyField(new Rect(rect.x + 250, rect.y, 200, EditorGUIUtility.singleLineHeight), axesProperty.GetArrayElementAtIndex(index).FindPropertyRelative("axisType"));

            if (axisType == AxisType.Custom)
            {
                EditorGUI.PropertyField(new Rect(rect.x, rect.y + (EditorGUIUtility.singleLineHeight + 5), 200, EditorGUIUtility.singleLineHeight), axesProperty.GetArrayElementAtIndex(index).FindPropertyRelative("positiveKey"));
                EditorGUI.PropertyField(new Rect(rect.x + 250, rect.y + (EditorGUIUtility.singleLineHeight + 5), 200, EditorGUIUtility.singleLineHeight), axesProperty.GetArrayElementAtIndex(index).FindPropertyRelative("negativeKey"));
            }
            else if (axisType == AxisType.Unity)
            {
                EditorGUI.PropertyField(new Rect(rect.x + 500, rect.y, 200, EditorGUIUtility.singleLineHeight), axesProperty.GetArrayElementAtIndex(index).FindPropertyRelative("unityAxisName"));
            }
        };
        axesReorderables.elementHeight = 40f;
    }
}