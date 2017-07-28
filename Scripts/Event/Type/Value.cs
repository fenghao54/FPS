using System;
using UnityEngine;

/// <summary>
/// 适用于当某一状态的值发生变化时需要接收到通知的情况
/// </summary>
/// <typeparam name="T"></typeparam>
public class Value<T>
{
    public event Action onChanged;

    private T currentValue;
    private T previousValue;

    public Value(T initialValue)
    {
        currentValue = initialValue;
        previousValue = currentValue;
    }

    public void AddChangedListener(Action listener)
    {
        onChanged += listener;
    }

    public void RemoveChangedListener(Action listener)
    {
        onChanged -= listener;
    }

    public bool Is(T value)
    {
        return currentValue != null && currentValue.Equals(value);
    }

    public T Get()
    {
        return currentValue;
    }

    public void Set(T value)
    {
        previousValue = currentValue;
        currentValue = value;

        if (onChanged != null && (previousValue == null || !previousValue.Equals(currentValue)))
            onChanged();
    }

    public T GetPreviousValue()
    {
        return previousValue;
    }

}