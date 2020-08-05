using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;


public class EventController : Singleton<EventController>
{
    public class BaseEvent { }

    public delegate void EventDelegate<T>(T e) where T : BaseEvent;

    private Dictionary<string, Delegate> delegates = new Dictionary<string, System.Delegate>();

    public void AddListener<T>(string eventName, EventDelegate<T> del) where T : BaseEvent
    {
        if (delegates.ContainsKey(eventName))
        {
            System.Delegate tempDel = delegates[eventName];

            delegates[eventName] = Delegate.Combine(tempDel, del);
        }
        else
        {
            delegates[eventName] = del;
        }
    }

    public void RemoveListener<T>(string eventName, EventDelegate<T> del) where T : BaseEvent
    {
        if (delegates.ContainsKey(eventName))
        {
            var currentDel = Delegate.Remove(delegates[eventName], del);

            if (currentDel == null)
            {
                delegates.Remove(eventName);
            }
            else
            {
                delegates[eventName] = currentDel;
            }
        }
    }

    public void TriggerEvent(string eventName, BaseEvent e)
    {
        if (e == null)
        {
            Debug.Log("Invalid event argument: " + e.GetType().ToString());
            return;
        }

        if (delegates.ContainsKey(eventName))
        {
            delegates[eventName].DynamicInvoke(e);
        }
    }
}


