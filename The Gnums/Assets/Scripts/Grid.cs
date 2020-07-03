﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Класс генерации поля
public class Grid : MonoBehaviour
{
    public Hex hexPrefab;
    public GameObject[] unitPrefab; //массив юнитов
    //public GameObject[] unitPrefabClone; //копия массива
    
    private Vector3 spawnPlace; //место спавна юнита
    
    public GameObject selectedUnit; //активный юнит

    private int rows = 13; //количество рядов
    private int cols = 17; //количество столбцов

    int index = 0; //индекс юнита в массиве юнитов
    Hex[,] graph; //граф гексов

    Hex hexGo;

    // Use this for initialization
    void Start()
    {
        GenerateGrid();
        FindNeighbours();
        
        foreach(Hex i in graph)
        {
            Debug.Log(i.transform.localPosition);
            i.isWalkable = false;
        }
        //selectedUnit = unitPrefabClone[index];
        selectedUnit = unitPrefab[index];
    }

    //расчёт позиции гекса
    public Vector2 Position(int i, int j)
    {
        float radius = 0.37f; //0.37 т. к. половина высоты гекса 37 пикселей
        float height = radius * 2;
        float width = Mathf.Sqrt(3) / 2 * height;

        float offsetX = width;
        float offsetY = height * 0.75f;

        if (j % 2 == 0)
        {
            return new Vector2(offsetX * i, offsetY * j);
        }
        else
        {
            return new Vector2(offsetX * (i + 0.5f), offsetY * j);
        }
    }

    public void GenerateGrid()
    {
        int k = 0;
        graph = new Hex[cols, rows];

        for (int i = 0; i < cols; i++)
        {
            for (int j = 0; j < rows; j++)
            {
                hexGo = Instantiate(hexPrefab, Position(i, j), Quaternion.identity, transform);
                hexGo.onHexClick += OnHexClick;
                hexGo.SetCoord(i, j);

                graph[i, j] = hexGo;

                hexGo.GetComponentInChildren<TextMesh>().text = string.Format("{0},{1}", i, j);

                //SpawnUnits(i, j, k);
                if (i == 0 && j % 2 == 0)
                {
                    if (k < unitPrefab.Length)
                    {
                        if (unitPrefab[k] != null)
                        {
                            unitPrefab[k].GetComponent<UnitCharacteristic>().unit.spawnLocation = Position(i, j);

                            spawnPlace = new Vector3(unitPrefab[k].GetComponent<UnitCharacteristic>().unit.spawnLocation.x,
                                unitPrefab[k].GetComponent<UnitCharacteristic>().unit.spawnLocation.y + unitPrefab[k].transform.localScale.y / 3, 0);

                            //unitPrefabClone[k] = Instantiate(unitPrefab[k], spawnTarget, Quaternion.identity) as GameObject;
                            unitPrefab[k] = Instantiate(unitPrefab[k], spawnPlace, Quaternion.identity) as GameObject;

                            //unitPrefabClone[k].GetComponent<UnitCharacteristic>().tileX = i;
                            //unitPrefabClone[k].GetComponent<UnitCharacteristic>().tileY = j;
                            unitPrefab[k].GetComponent<UnitCharacteristic>().tileX = i;
                            unitPrefab[k].GetComponent<UnitCharacteristic>().tileY = j;

                            
                        }
                        k++;
                    }
                }
                //else if (j == rows - 1 && j % 2 != 0) {}
            }
        }
        BubbleSort();
    }

    //определяем место спавна и спавним юнитов
    public void SpawnUnits(int i, int j, int k)
    {
        

        if (i == 0 && j % 2 == 0)
        {
            if (k < unitPrefab.Length)
            {
                if (unitPrefab[k] != null)
                {
                    unitPrefab[k].GetComponent<UnitCharacteristic>().unit.spawnLocation = Position(i, j);


                    spawnPlace = new Vector3(unitPrefab[k].GetComponent<UnitCharacteristic>().unit.spawnLocation.x,
                        unitPrefab[k].GetComponent<UnitCharacteristic>().unit.spawnLocation.y + unitPrefab[k].transform.localScale.y / 3, 0);
                    //unitPrefabClone[k] = Instantiate(unitPrefab[k], spawnTarget, Quaternion.identity) as GameObject;
                    unitPrefab[k] = Instantiate(unitPrefab[k], spawnPlace, Quaternion.identity) as GameObject;

                    //unitPrefabClone[k].GetComponent<UnitCharacteristic>().tileX = i;
                    //unitPrefabClone[k].GetComponent<UnitCharacteristic>().tileY = j;
                    unitPrefab[k].GetComponent<UnitCharacteristic>().tileX = i;
                    unitPrefab[k].GetComponent<UnitCharacteristic>().tileY = j;
                }
                k++;
            }
        }
        //else if (j == rows - 1 && j % 2 != 0) {}
    }

    public float CostToEnterTile(int sourceX, int sourceY, int targetX, int targetY)
    {
        float cost = 1;

        if (sourceX != targetX && sourceY != targetY)
        {
            // фикс излишней диагональности (?)
            cost += 0.001f;
        }

        return cost;
    }

    //вычисляем соседей гекса
    public void FindNeighbours()
    {
        for (int x = 0; x < cols; x++)
        {
            for (int y = 0; y < rows; y++)
            {
                if (x - 1 >= 0)
                {
                    graph[x, y].neighbours.Add(graph[x - 1, y]);

                    if (y + 1 < rows && (y % 2 == 0)) //если чётная строка
                    {
                        graph[x, y].neighbours.Add(graph[x - 1, y + 1]);
                    }

                    if (y - 1 >= 0 && (y % 2 == 0)) //если чётная строка
                    {
                        graph[x, y].neighbours.Add(graph[x - 1, y - 1]);
                    }
                }

                if (x + 1 < cols)
                {
                    graph[x, y].neighbours.Add(graph[x + 1, y]);

                    if (y + 1 < rows && (y % 2 != 0)) // и если нечётная строка 
                    {
                        graph[x, y].neighbours.Add(graph[x + 1, y + 1]);
                    }
                }

                if (y - 1 >= 0)
                {
                    graph[x, y].neighbours.Add(graph[x, y - 1]);

                    if (x + 1 < cols && (y % 2 != 0)) //если нечётная строка 
                    {
                        graph[x, y].neighbours.Add(graph[x + 1, y - 1]);
                    }
                }

                if (y + 1 < rows)
                {
                    graph[x, y].neighbours.Add(graph[x, y + 1]);
                }
            }
        }
    }

    private void OnHexClick(Hex hex)
    {
        GeneratePathTo(hex.q, hex.r);

        selectedUnit.GetComponent<UnitCharacteristic>().tileX = hex.q;
        selectedUnit.GetComponent<UnitCharacteristic>().tileY = hex.r;
    }

    public void GeneratePathTo(int x, int y)
    {
        // чистим старый путь
        selectedUnit.GetComponent<UnitCharacteristic>().currentPath = null;

        Dictionary<Hex, float> dist = new Dictionary<Hex, float>();
        Dictionary<Hex, Hex> prev = new Dictionary<Hex, Hex>();

        // непосещённые ноды
        List<Hex> unvisited = new List<Hex>();

        Hex source = graph[selectedUnit.GetComponent<UnitCharacteristic>().tileX, selectedUnit.GetComponent<UnitCharacteristic>().tileY];

        Hex target = graph[x, y];

        dist[source] = 0;
        prev[source] = null;

        foreach (Hex v in graph)
        {
            if (v != source)
            {
                dist[v] = Mathf.Infinity;
                prev[v] = null;
            }

            unvisited.Add(v);
        }

        while (unvisited.Count > 0)
        {
            // u - непосещённый нод с минимальной дистанцей 
            Hex u = null;

            foreach (Hex possibleU in unvisited)
            {
                if (u == null || dist[possibleU] < dist[u])
                {
                    u = possibleU;
                }
            }

            if (u == target)
            {
                break;
            }

            unvisited.Remove(u);

            foreach (Hex v in u.neighbours)
            {
                //float alt = dist[u] + u.DistanceTo(v);
                float alt = dist[u] + CostToEnterTile(u.q, u.r, v.q, v.r);
                //float alt = dist[u] + 1;

                if (alt < dist[v])
                {
                    dist[v] = alt;
                    prev[v] = u;
                }
            }
        }

        if (prev[target] == null)
        {
            // нет пути от сёрса к таргету
            return;
        }

        List<Hex> currentPath = new List<Hex>();

        Hex curr = target;

        // добавляем нод в путь
        while (curr != null)
        {
            currentPath.Add(curr);
            curr = prev[curr];
        }

        // ревёрсим потому что ща путь от таргета к сурсу 
        currentPath.Reverse();

        /*
        foreach (Hex i in currentPath)
        {
            Debug.Log("path" + " " + i.q + " " + i.r);
        }*/

        selectedUnit.GetComponent<UnitCharacteristic>().currentPath = currentPath;

        selectedUnit.GetComponent<UnitCharacteristic>().ReadyToMove();
       
    }

    /*
    public GameObject[] BubbleSort()
    {
        GameObject temp;
        for (int i = 0; i < unitPrefabClone.Length; i++)
        {
            for (int j = i + 1; j < unitPrefabClone.Length; j++)
            {
                if (unitPrefabClone[i].GetComponent<UnitCharacteristic>().unit.initiative < unitPrefabClone[j].GetComponent<UnitCharacteristic>().unit.initiative ||
                   (unitPrefabClone[i].GetComponent<UnitCharacteristic>().unit.initiative == unitPrefabClone[j].GetComponent<UnitCharacteristic>().unit.initiative &&
                    unitPrefabClone[i].GetComponent<UnitCharacteristic>().unit.spawnLocation.y < unitPrefabClone[j].GetComponent<UnitCharacteristic>().unit.spawnLocation.y))
                {
                    temp = unitPrefabClone[i];
                    unitPrefabClone[i] = unitPrefabClone[j];
                    unitPrefabClone[j] = temp;
                }
            }
        }
        return unitPrefabClone;
    }*/

    //сортируем юнитов для очерёдности ходов изходя из статов юнита и его места спавна
    public GameObject[] BubbleSort()
    {
        GameObject temp;
        for (int i = 0; i < unitPrefab.Length; i++)
        {
            for (int j = i + 1; j < unitPrefab.Length; j++)
            {
                if (unitPrefab[i].GetComponent<UnitCharacteristic>().unit.initiative < unitPrefab[j].GetComponent<UnitCharacteristic>().unit.initiative ||
                   (unitPrefab[i].GetComponent<UnitCharacteristic>().unit.initiative == unitPrefab[j].GetComponent<UnitCharacteristic>().unit.initiative &&
                    unitPrefab[i].GetComponent<UnitCharacteristic>().unit.spawnLocation.y < unitPrefab[j].GetComponent<UnitCharacteristic>().unit.spawnLocation.y))
                {
                    temp = unitPrefab[i];
                    unitPrefab[i] = unitPrefab[j];
                    unitPrefab[j] = temp;
                }
            }
        }
        return unitPrefab;
    }
    
   

    private void OnEnable()
    {
        EventManager.StartListening("Next", QueueManager);
    }

    private void OnDisable()
    {
        EventManager.StopListening("Next", QueueManager);
    }

    public void QueueManager()
    {
        index++;

        if (index == 7)
        {
            index = 0;
        }

        if (index < unitPrefab.Length)
        {
            //selectedUnit = unitPrefabClone[index];
            selectedUnit = unitPrefab[index];
        }

        Debug.Log(selectedUnit.GetComponent<UnitCharacteristic>().unit.spawnLocation);
        Debug.Log(index);
    }
}
