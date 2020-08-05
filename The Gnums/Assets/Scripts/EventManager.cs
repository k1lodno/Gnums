using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class MoveEvent : UnityEvent<Hex> { }

[System.Serializable]
public class AttackEvent : UnityEvent<Unit> { }

public class EventManager : Singleton<EventManager>
{
    //библиотека ивентов
    private Dictionary<string, MoveEvent> moveDictionary = new Dictionary<string, MoveEvent>();
    private Dictionary<string, UnityEvent> queueDictionary = new Dictionary<string, UnityEvent>();
    private Dictionary<string, AttackEvent> attackDictionary = new Dictionary<string, AttackEvent>();


    //Move
    public void StartListening(string eventName, UnityAction<Hex> listener)
    {
        MoveEvent thisEvent = null;
        //по имени ивента получаем значение и запихиваем его в thisEvent ?
        if (moveDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.AddListener(listener);
        }
        else
        {
            thisEvent = new MoveEvent();
            thisEvent.AddListener(listener);
            moveDictionary.Add(eventName, thisEvent);
        }
    }

    //Next
    public void StartListening(string eventName, UnityAction listener)
    {
        UnityEvent thisEvent = null;
        //по имени ивента получаем значение и запихиваем его в thisEvent ?
        if (queueDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.AddListener(listener);
        }
        else
        {
            thisEvent = new UnityEvent();
            thisEvent.AddListener(listener);
            queueDictionary.Add(eventName, thisEvent);
        }
    }

    //Attack
    public void StartListening(string eventName, UnityAction<Unit> listener)
    {
        AttackEvent thisEvent = null;
        //по имени ивента получаем значение и запихиваем его в thisEvent ?
        if (attackDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.AddListener(listener);
        }
        else
        {
            thisEvent = new AttackEvent();
            thisEvent.AddListener(listener);
            attackDictionary.Add(eventName, thisEvent);
        }
    }

    //Move
    public void StopListening(string eventName, UnityAction<Hex> listener)
    {

        MoveEvent thisEvent = null;

        if (moveDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.RemoveListener(listener);
        }
    }

    //Next
    public void StopListening(string eventName, UnityAction listener)
    {


        UnityEvent thisEvent = null;

        if (queueDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.RemoveListener(listener);
        }
    }

    //Attack
    public void StopListening(string eventName, UnityAction<Unit> listener)
    {

        AttackEvent thisEvent = null;

        if (attackDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.RemoveListener(listener);
        }
    }

    //Move
    public void TriggerEvent(string eventName, Hex hex)
    {
        MoveEvent thisEvent = null;

        if (moveDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.Invoke(hex);
        }
    }

    //Next
    public void TriggerEvent(string eventName)
    {
        UnityEvent thisEvent = null;

        if (queueDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.Invoke();
        }
    }

    //Attack
    public void TriggerEvent(string eventName, Unit unit)
    {
        AttackEvent thisEvent = null;

        if (attackDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.Invoke(unit);
        }
    }
}
