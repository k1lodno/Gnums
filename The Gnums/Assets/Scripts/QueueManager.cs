using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EventController;

public class QueueManager : MonoBehaviour
{
    [SerializeField]
    SpawnManager spawn;

    private Unit selectedUnit; //активный юнит

    int index = 0; //индекс юнита в массиве юнитов

    public Unit SelectedUnit { get => selectedUnit; set => selectedUnit = value; }

    void Start()
    {
        SelectedUnit = spawn.UnitPrefabClone[index];
        SelectedUnit.GetMovableRange(SelectedUnit.CurrentHex, 0);
        SelectedUnit.GetAttackRange(SelectedUnit.CurrentHex, 0);
    }

    private void OnEnable()
    {
        EventController.Instance.AddListener<BaseEvent>("Next", Queue);
    }

    private void OnDisable()
    {
        EventController.Instance.RemoveListener<BaseEvent>("Next", Queue);
    }

    public void Queue(BaseEvent baseEvent)
    {
        index++;

        if (index == spawn.UnitPrefabClone.Count)
        {
            index = 0;
        }

        if (index < spawn.UnitPrefabClone.Count)
        {
            SelectedUnit = spawn.UnitPrefabClone[index];
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
}
