using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    Grid grid;

    [SerializeField]
    Transform unitSpawnPlace;

    public UnitCharacteristic[] unitPrefab; //массив юнитов
    private List<UnitCharacteristic> unitPrefabClone;
    private UnitCharacteristic selectedUnit; //активный юнит

    int index = 0; //индекс юнита в массиве юнитов

    public UnitCharacteristic SelectedUnit { get => selectedUnit; set => selectedUnit = value; }

    void Start()
    {
        unitPrefabClone = new List<UnitCharacteristic>();
        SelectSpawnPlaces();
        SelectedUnit = unitPrefabClone[index];
    }


    public void SelectSpawnPlaces()
    {
        int k = 0;

        for (int i = 0; i < grid.Cols; i++)
        {
            for (int j = 0; j < grid.Rows; j++)
            {
                if (i == 0 && j % 2 == 0)
                {
                    if (k < unitPrefab.Length)
                    {
                        if (unitPrefab[k] != null)
                        {
                            var unit = Instantiate(unitPrefab[k], unitSpawnPlace);
                            unitPrefabClone.Add(unit);

                            var hex = grid.GetHex(i, j);

                            unit.Spawn(hex);

                            grid.Graph[i, j].isWalkable = false;
                            grid.Graph[i, j].GetComponent<SpriteRenderer>().color = Color.black;
                        }

                        k++;
                    }
                }
            }
        }

        //else if (j == rows - 1 && j % 2 != 0) {}

        //не учитывается то что если у юнитов одинаковая инициатива то должен ходить верхний
        unitPrefabClone.Sort(delegate (UnitCharacteristic x, UnitCharacteristic y)
        {
            return y.unit.initiative.CompareTo(x.unit.initiative);
        });
    }


    private void OnEnable()
    {
        EventManager.Instance.StartListening("Next", QueueManager);
    }

    private void OnDisable()
    {
        EventManager.Instance.StopListening("Next", QueueManager);
    }

    public void QueueManager()
    {
        index++;

        if (index == 7)
        {
            index = 0;
        }

        if (index < unitPrefabClone.Count)
        {
            SelectedUnit = unitPrefabClone[index];
        }
    }
}
