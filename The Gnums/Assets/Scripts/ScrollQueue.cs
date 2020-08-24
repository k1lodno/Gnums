using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static EventController;

public class ScrollQueue : MonoBehaviour
{
    [SerializeField]
    ScrollRect scroll = null;

    [SerializeField]
    Image img = null;

    public void OnClick()
    {
        var a = scroll.content.sizeDelta.x / img.rectTransform.rect.width;
        scroll.horizontalNormalizedPosition += 1 / a;
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
