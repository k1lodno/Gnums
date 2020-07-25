using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class ThisEvent : UnityEvent<Hex> { }

public class EventManager : Singleton<EventManager>
{
    //библиотека ивентов
    private Dictionary<string, ThisEvent> eventDictionary = new Dictionary<string, ThisEvent>();
    private Dictionary<string, UnityEvent> secondDictionary = new Dictionary<string, UnityEvent>();

    

    //Move
    public void StartListening(string eventName, UnityAction<Hex> listener)
    {
        ThisEvent thisEvent = null;
        //по имени ивента получаем значение и запихиваем его в thisEvent ?
        if (eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.AddListener(listener);
        }
        else
        {
            thisEvent = new ThisEvent();
            thisEvent.AddListener(listener);
            eventDictionary.Add(eventName, thisEvent);
        }
    }

    //Next
    public void StartListening(string eventName, UnityAction listener)
    {
        UnityEvent thisEvent = null;
        //по имени ивента получаем значение и запихиваем его в thisEvent ?
        if (secondDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.AddListener(listener);
        }
        else
        {
            thisEvent = new UnityEvent();
            thisEvent.AddListener(listener);
            secondDictionary.Add(eventName, thisEvent);
        }
    }

    //Move
    public void StopListening(string eventName, UnityAction<Hex> listener)
    {

        ThisEvent thisEvent = null;

        if (eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.RemoveListener(listener);
        }
    }

    //Next
    public void StopListening(string eventName, UnityAction listener)
    {


        UnityEvent thisEvent = null;

        if (secondDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.RemoveListener(listener);
        }
    }

    //Move
    public void TriggerEvent(string eventName, Hex hex)
    {
        ThisEvent thisEvent = null;

        if (eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.Invoke(hex);
        }
    }

    //Next
    public void TriggerEvent(string eventName)
    {
        UnityEvent thisEvent = null;

        if (secondDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.Invoke();
        }
    }
}
