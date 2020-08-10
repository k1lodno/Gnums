using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollQueue : MonoBehaviour
{
    [SerializeField]
    ScrollRect scroll = null;

    [SerializeField]
    Image img = null;

    int imgPoolSize = 10;

    [SerializeField]
    QueueManager q = null;

    private void Awake()
    {

    }

    private void Start()
    {
        Image imgGO;

        for (int i = 0; i < imgPoolSize; i++)
        {
            for (int j = 0; j < q.QueueList.Count; j++)
            {
                imgGO = Instantiate(img, scroll.content);

                imgGO.GetComponentInChildren<Text>().text = q.QueueList[j].transform.position.ToString();
                imgGO.gameObject.SetActive(true);
            }

            imgGO = Instantiate(img, scroll.content);
            imgGO.gameObject.SetActive(true);
        }
    }

    public void OnClick()
    {
        var a = scroll.content.sizeDelta.x / img.rectTransform.rect.width;
        scroll.horizontalNormalizedPosition += 1 / a;
    }
}
