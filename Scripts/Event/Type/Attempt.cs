using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// 适用于当激活某一状态时，需要先进行处理，根据处理结果决定是否派发通知时
/// </summary>
public class Attempt
{
    private Func<bool> handle;
    private Action listeners;

    public void SetHandle(Func<bool> rHandle)
    {
        handle = rHandle;
    }

    public void AddListener(Action listener)
    {
        listeners += listener;
    }

    public void RemoveListener(Action listener)
    {
        listeners -= listener;
    }

    public bool Do()
    {
        bool wasSuccessful = (handle == null || handle());
        if (wasSuccessful)
        {
            if (listeners != null)
                listeners();
            return true;
        }

        return false;
    }
}