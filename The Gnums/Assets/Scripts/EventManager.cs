using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class ThisEvent : UnityEvent<Hex> { }

public class EventManager : MonoBehaviour
{
    //библиотека ивентов
    private Dictionary<string, ThisEvent> eventDictionary;
    private Dictionary<string, UnityEvent> secondDictionary;
    //экземпляр 
    private static EventManager eventManager;

    private EventManager() {}

    //kind of СИНХЛЕТОН
    public static EventManager Instance
    {
        get
        {
            if (!eventManager)
            {
                eventManager = FindObjectOfType(typeof(EventManager)) as EventManager;
                eventManager.Init();
            }
            return eventManager;
        }
    }

    void Init()
    {
        if (eventDictionary == null)
        {
            eventDictionary = new Dictionary<string, ThisEvent>();
        }

        if (secondDictionary == null)
        {
            secondDictionary = new Dictionary<string, UnityEvent>();
        }

    }

    //Move
    public static void StartListening(string eventName, UnityAction<Hex> listener)
    {
        ThisEvent thisEvent = null;
        //по имени ивента получаем значение и запихиваем его в thisEvent ?
        if (Instance.eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.AddListener(listener);
        }
        else
        {
            thisEvent = new ThisEvent();
            thisEvent.AddListener(listener);
            Instance.eventDictionary.Add(eventName, thisEvent);
        }
    }

    //Next
    public static void StartListening(string eventName, UnityAction listener)
    {
        UnityEvent thisEvent = null;
        //по имени ивента получаем значение и запихиваем его в thisEvent ?
        if (Instance.secondDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.AddListener(listener);
        }
        else
        {
            thisEvent = new UnityEvent();
            thisEvent.AddListener(listener);
            Instance.secondDictionary.Add(eventName, thisEvent);
        }
    }

    //Move
    public static void StopListening(string eventName, UnityAction<Hex> listener)
    {
        if (eventManager == null) { return; }

        ThisEvent thisEvent = null;

        if (Instance.eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.RemoveListener(listener);
        }
    }

    //Next
    public static void StopListening(string eventName, UnityAction listener)
    {
        if (eventManager == null) { return; }

        UnityEvent thisEvent = null;

        if (Instance.secondDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.RemoveListener(listener);
        }
    }

    //Move
    public static void TriggerEvent(string eventName, Hex hex)
    {
        ThisEvent thisEvent = null;

        if (Instance.eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.Invoke(hex);
        }
    }

    //Next
    public static void TriggerEvent(string eventName)
    {
        UnityEvent thisEvent = null;

        if (Instance.secondDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.Invoke();
        }
    }
}
