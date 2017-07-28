using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// 适用于需要接收某种状态的开始和结束事件通知时
/// </summary>
public class Activity
{
    public bool Active { get; private set; }

    Func<bool> startHandle;
    Func<bool> stopHandle;

    Action onStarted;
    Action onStopped;

    public void AddStartHandle(Func<bool> handle)
    {
        startHandle += handle;
    }

    public void RemoveStartHandle(Func<bool> handle)
    {
        startHandle -= handle;
    }

    public void AddStopHandle(Func<bool> handle)
    {
        stopHandle += handle;
    }

    public void RemoveStopHandle(Func<bool> handle)
    {
        stopHandle -= handle;
    }

    public void AddStartedListener(Action listener)
    {
        onStarted += listener;
    }

    public void RemoveStartedListener(Action listener)
    {
        onStarted -= listener;
    }

    public void AddStoppedListener(Action listener)
    {
        onStopped += listener;
    }

    public void RemoveStoppedListener(Action listener)
    {
        onStopped -= listener;
    }


    public void DirectStart()
    {
        if (Active) return;

        Active = true;

        if (onStarted != null) onStarted();
    }

    public void DirectStop()
    {
        if (!Active) return;

        Active = false;

        if (onStopped != null) onStopped();
    }

    public bool Start()
    {
        if (Active)
            return false;

        if (startHandle != null)
        {
            if (CallHandle(startHandle))
            {
                Active = true;

                if (onStarted != null)
                {
                    onStarted();
                }
                return true;
            }
        }
        return false;
    }

    public bool Stop()
    {
        if (!Active) return false;

        if (stopHandle != null)
        {
            if (CallHandle(stopHandle))
            {
                Active = false;

                if (onStopped != null)
                {
                    onStopped();
                }
                return true;
            }
        }
        return false;
    }

    private bool CallHandle(Func<bool> handles)
    {
        var invocationList = handles.GetInvocationList();
        foreach (var del in invocationList)
        {
            if (del == null) continue;
            if (!(bool)del.DynamicInvoke())
            {
                return false;
            }
        }
        return true;
    }
}
