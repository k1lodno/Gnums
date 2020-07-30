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

    public Unit[] unitPrefab; //массив юнитов
    private List<Unit> unitPrefabClone;
    private Unit selectedUnit; //активный юнит

    public Unit[] enemyList;
    private List<Unit> enemyListClone;

    int index = 0; //индекс юнита в массиве юнитов

    public Unit SelectedUnit { get => selectedUnit; set => selectedUnit = value; }

    void Start()
    {
        unitPrefabClone = new List<Unit>();
        enemyListClone = new List<Unit>();
        SelectSpawnPlaces();
        SelectedUnit = unitPrefabClone[index];
        selectedUnit.GetMovableRange(selectedUnit.CurrentHex, 0);
    }


    public void SelectSpawnPlaces()
    {
        int k = 0;
        int l = 0;

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
                else if (i == (grid.Cols - 1) && j % 2 != 0)
                {
                    if (l < enemyList.Length)
                    {
                        if (enemyList[l] != null)
                        {
                            var enemy = Instantiate(enemyList[l], unitSpawnPlace);
                            enemyListClone.Add(enemy);

                            var hex = grid.GetHex(i, j);

                            enemy.Spawn(hex);

                            grid.Graph[i, j].isWalkable = false;
                            grid.Graph[i, j].GetComponent<SpriteRenderer>().color = Color.black;
                        }

                        l++;
                    }

                }
            }
        }

        //не учитывается то что если у юнитов одинаковая инициатива то должен ходить верхний
        unitPrefabClone.Sort(delegate (Unit x, Unit y)
        {
            return y.baseUnit.initiative.CompareTo(x.baseUnit.initiative);
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
            selectedUnit.GetMovableRange(selectedUnit.CurrentHex, 0);
        }
    }
}
