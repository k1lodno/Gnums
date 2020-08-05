using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EventController;

public class OnClickEvent<T> : BaseEvent where T : class
{

    private T onClickObject;

    public OnClickEvent(T onClickObject)
    {
        OnClickObject = onClickObject;
    }

    public T OnClickObject { get => onClickObject; set => onClickObject = value; }
}
