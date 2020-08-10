using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    Grid grid = null;

    [SerializeField]
    Transform unitSpawnPlace = null;

    [SerializeField]
    Transform warlordSpawnPlace = null;

    [SerializeField]
    QueueManager queue = null;

    public Warlord warlordPrefab;

    public Unit[] unitPrefab; //массив юнитов игрока
    private List<Unit> unitPrefabClone;

    public Unit[] enemyList; //массив юнитов ии
    private List<Unit> enemyListClone;

    //public List<Unit> UnitPrefabClone { get => unitPrefabClone; set => unitPrefabClone = value; }
    //public List<Unit> EnemyListClone { get => enemyListClone; set => enemyListClone = value; }
    
    System.Random rnd = new System.Random();

    void Start()
    {
        SpawnWarlord();
        SelectSpawnPlaces();
    }

    public void SpawnWarlord()
    {
        var warlord = Instantiate(warlordPrefab, warlordSpawnPlace);
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
                            queue.QueueList.Add(unit);

                            var hex = grid.GetHex(i, j);

                            //пока так рандомим количество юнитов в стеке
                            unit.NumberOfUnits = rnd.Next(1, 10);
                            unit.Spawn(hex);

                            grid.Graph[i, j].isWalkable = false;
                            grid.Graph[i, j].render.color = Color.black;
                        }

                        k++;
                    }
                }
                //противоположная сторона
                else if (i == (grid.Cols - 1) && j % 2 != 0)
                {
                    if (l < enemyList.Length)
                    {
                        if (enemyList[l] != null)
                        {
                            var enemy = Instantiate(enemyList[l], unitSpawnPlace);
                            enemy.IsEnemy = true;
                            queue.QueueList.Add(enemy);

                            var hex = grid.GetHex(i, j);

                            enemy.NumberOfUnits = rnd.Next(1, 10);
                            enemy.Spawn(hex);

                            grid.Graph[i, j].isWalkable = false;
                            grid.Graph[i, j].render.color = Color.black;
                        }

                        l++;
                    }

                }
            }
        }

        //не учитывается то что если у юнитов одинаковая инициатива то должен ходить верхний

        queue.QueueList.Sort(delegate (Unit x, Unit y)
        {
            return y.Initiative.CompareTo(x.Initiative);
        });
    }
}
