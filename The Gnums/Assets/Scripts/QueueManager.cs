using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EventController;

public class QueueManager : MonoBehaviour
{
    private Unit selectedUnit; //активный юнит
    List<Unit> queueList; //лист всех юнитов 

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
    }


    private void OnEnable()
    {
        if (EventController.Instance != null)
        {
            EventController.Instance.AddListener<BaseEvent>("Next", Queue);
        }
    }

    private void OnDisable()
    {
        if (EventController.Instance != null)
        {
            EventController.Instance.RemoveListener<BaseEvent>("Next", Queue);
        }
    }
}
