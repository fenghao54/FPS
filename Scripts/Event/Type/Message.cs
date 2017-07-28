using System;

/// <summary>
/// 适用于当某一事件发生时需要接收到通知的情况
/// </summary>
public class Message
{
    private Action listeners;

    public void AddListener(Action listener)
    {
        listeners += listener;
    }

    public void RemoveListener(Action listener)
    {
        listeners -= listener;
    }

    public void Send()
    {
        if (listeners != null)
            listeners();
    }
}

public class Message<T>
{
    private Action<T> listeners;

    public void AddListener(Action<T> listener)
    {
        listeners += listener;
    }

    public void RemoveListener(Action<T> callback)
    {
        listeners -= callback;
    }

    public void Send(T message)
    {
        if (listeners != null)
            listeners(message);
    }
}