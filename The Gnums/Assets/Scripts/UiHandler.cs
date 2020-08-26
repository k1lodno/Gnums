using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static EventController;

public class UiHandler : MonoBehaviour
{
    [SerializeField]
    ScrollRect scroll = null;

    [SerializeField]
    Image img = null;


    public void OnNextClick()
    {
        var a = scroll.content.sizeDelta.x / img.rectTransform.rect.width;
        scroll.horizontalNormalizedPosition += 1 / a;

        scroll.GetComponent<QueueManager>().AddToQueue();
    }

    public void OnWaitClick()
    {
        EventController.Instance.TriggerEvent("Next", new BaseEvent());
    }

    public void OnDefenceClick()
    {
        EventController.Instance.TriggerEvent("Next", new BaseEvent());
    }
}
