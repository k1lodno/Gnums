using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static EventController;

public class QueueManager : MonoBehaviour
{

    [SerializeField]
    Image queueMember = null;

    [SerializeField]
    ScrollQueue scroll = null;

    private Unit selectedUnit; //активный юнит
    List<Unit> queueList; //лист всех юнитов 

    int numberOfTurns = 10;
    int index = 0; //индекс юнита в массиве юнитов

    public Unit SelectedUnit { get => selectedUnit; set => selectedUnit = value; }
    public List<Unit> QueueList { get => queueList; set => queueList = value; }

    private void Awake()
    {
        QueueList = new List<Unit>();
    }
    void Start()
    {        
        SelectedUnit = QueueList[index];
        SelectedUnit.GetMovableRange(SelectedUnit.CurrentHex, 0);
        SelectedUnit.GetAttackRange(SelectedUnit.CurrentHex, 0);

        InitQueue();
    }


    public void InitQueue()
    {
        Image imgGO;

        for (int i = 0; i < numberOfTurns; i++)
        {
            for (int j = 0; j < QueueList.Count; j++)
            {
                imgGO = Instantiate(queueMember, GetComponent<ScrollRect>().content);

                imgGO.gameObject.GetComponent<Image>().sprite = QueueList[j].UnitAvatar;
                imgGO.gameObject.SetActive(true);
            }

            imgGO = Instantiate(queueMember, GetComponent<ScrollRect>().content);
            imgGO.GetComponentInChildren<Text>().text = (i + 1).ToString();
            imgGO.gameObject.SetActive(true);
        }
    }

    public void UpdateQueue(OnClickEvent<Unit> p)
    {
        var deadUnit = p.OnClickObject;

        if (QueueList.Contains(deadUnit))
        {
            QueueList.Remove(deadUnit);
            InitQueue();
        }
    }

    public void Queue(BaseEvent baseEvent)
    {
        index++;

        if (index == QueueList.Count)
        {
            index = 0;
        }

        if (index < QueueList.Count)
        {
            SelectedUnit = QueueList[index];

            //чистим предыдующие доступные гексы 
            if (SelectedUnit.ReachableHexes != null)
            {
                SelectedUnit.ReachableHexes.Clear();
            }

            SelectedUnit.GetMovableRange(SelectedUnit.CurrentHex, 0);
            
            if (SelectedUnit.AttackableHexes != null)
            {
                SelectedUnit.AttackableHexes.Clear();           
            }

            SelectedUnit.GetAttackRange(SelectedUnit.CurrentHex, 0);
        }

        scroll.OnClick();
    }


    private void OnEnable()
    {
        if (EventController.Instance != null)
        {
            EventController.Instance.AddListener<BaseEvent>("Next", Queue);
            EventController.Instance.AddListener<OnClickEvent<Unit>>("Death", UpdateQueue);
        }
    }

    private void OnDisable()
    {
        if (EventController.Instance != null)
        {
            EventController.Instance.RemoveListener<BaseEvent>("Next", Queue);
            EventController.Instance.RemoveListener<OnClickEvent<Unit>>("Death", UpdateQueue);
        }
    }
}
