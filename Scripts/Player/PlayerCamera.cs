using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : PlayerBehaviour
{
    [Header("General")]

    [SerializeField]
    private Transform lookRoot;

    [SerializeField]
    private Transform playerRoot;

    [Header("Motion")]

    [SerializeField]
    private float sensitivity = 5f;

    [SerializeField]
    [Range(0, 20)]
    private int smoothSteps = 10;

    [SerializeField]
    [Range(0f, 1f)]
    private float smoothWeight = 0.4f;

    [SerializeField]
    private float rollAngle = 10f;

    [SerializeField]
    private float rollSpeed = 3f;

    [Header("Rotation Limits")]

    [SerializeField]
    private Vector2 defaultLookLimits = new Vector2(-60f, 90f);

    private float rollAngleCurrent;

    private Vector2 lookAngles;

    private int lastLookFrame;
    private Vector2 currentMouseLook;
    private Vector2 smoothMove;
    private List<Vector2> smoothBuffer = new List<Vector2>();


    private void Start()
    {
        if (!lookRoot)
        {
            enabled = false;
        }

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void OnGUI()
    {
        Vector2 buttonSize = new Vector2(256f, 24f);

        if (Event.current.type == EventType.KeyDown)
        {
            if (Event.current.keyCode == KeyCode.Escape)
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }
        }

        if (Cursor.lockState == CursorLockMode.None)
        {
            if (GUI.Button(new Rect(Screen.width * 0.5f - buttonSize.x / 2f, 16f, buttonSize.x, buttonSize.y), "Hide Cursor"))
            {
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            }
        }
    }

    private void Update()
    {
        if (Player.viewLocked.Is(false) && Cursor.lockState == CursorLockMode.Locked && Player.health.Get() > 0f)
            LookAround();

        Player.viewLocked.Set(Cursor.lockState != CursorLockMode.Locked);
    }


    private void LookAround()
    {
        CalculateMouseInput(Time.deltaTime);

        lookAngles.x += currentMouseLook.x * sensitivity * -1f;
        lookAngles.y += currentMouseLook.y * sensitivity;

        lookAngles.x = ClampAngle(lookAngles.x, defaultLookLimits.x, defaultLookLimits.y);

        rollAngleCurrent = Mathf.Lerp(rollAngleCurrent, Player.lookInput.Get().x * rollAngle, Time.deltaTime * rollSpeed);

        lookRoot.localRotation = Quaternion.Euler(lookAngles.x, 0f, rollAngleCurrent);

        playerRoot.localRotation = Quaternion.Euler(0f, lookAngles.y, 0f);

        Player.lookDirection.Set(lookRoot.forward);
    }

    private float ClampAngle(float angle, float min, float max)
    {
        if (angle > 360f)
            angle -= 360f;
        else if (angle < -360f)
            angle += 360f;

        return Mathf.Clamp(angle, min, max);
    }

    private void CalculateMouseInput(float deltaTime)
    {
        if (lastLookFrame == Time.frameCount)
            return;

        lastLookFrame = Time.frameCount;

        smoothMove = new Vector2(Input.GetAxisRaw("Mouse Y"), Input.GetAxisRaw("Mouse X"));

        smoothSteps = Mathf.Clamp(smoothSteps, 1, 20);
        smoothWeight = Mathf.Clamp01(smoothWeight);

        while (smoothBuffer.Count > smoothSteps)
            smoothBuffer.RemoveAt(0);

        smoothBuffer.Add(smoothMove);

        float weight = 1f;
        Vector2 average = Vector2.zero;
        float averageTotal = 0f;

        for (int i = smoothBuffer.Count - 1; i > 0; i--)
        {
            average += smoothBuffer[i] * weight;
            averageTotal += weight;
            weight *= smoothWeight / (deltaTime * 60f);
        }

        averageTotal = Mathf.Max(1f, averageTotal);
        currentMouseLook = average / averageTotal;
    }
}