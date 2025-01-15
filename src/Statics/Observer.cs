using System;
using System.Collections.Generic;

public class Observer
{
    private List<Action> listeners;

    public Observer()
    {
        listeners = new List<Action>();
    }

    public void Subscribe(Action callback)
    {
        listeners.Add(callback);
    }

    public void Unsubscribe(Action callback)
    {
        listeners.Remove(callback);
    }

    public void Notify()
    {
        foreach (var callback in listeners)
        {
            callback();
        }
    }
}