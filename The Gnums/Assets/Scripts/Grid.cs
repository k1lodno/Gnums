using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//Класс генерации поля
public class Grid : MonoBehaviour
{
    public Hex hexPrefab;

    private int rows = 13; //количество рядов
    private int cols = 17; //количество столбцов

    private Hex[,] graph; //массив гексов

    private Hex hexGo;

    public int Rows { get => rows; set => rows = value; }
    public int Cols { get => cols; set => cols = value; }
    public Hex[,] Graph { get => graph; set => graph = value; }

    private void Awake()
    {
        GenerateGrid();
        FindNeighbours();
    }

    // Use this for initialization
    void Start()
    {
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
        Graph = new Hex[Cols, Rows];

        for (int i = 0; i < Cols; i++)
        {
            for (int j = 0; j < Rows; j++)
            {
                hexGo = Instantiate(hexPrefab, Position(i, j), Quaternion.identity, transform);
                //hexGo.onHexClick += OnHexClick;
                hexGo.SetCoord(i, j);

                Graph[i, j] = hexGo;

                hexGo.GetComponentInChildren<TextMesh>().text = string.Format("{0},{1}", i, j);
            }
        }
    }

    public Hex GetHex(int q, int r)
    {
        if (q >= 0 && q < Cols)
        {
            if (r >= 0 && r < Rows)
            {
                return Graph[q, r];
            }
        }

        return null;
    }

    //вычисляем соседей гекса
    public void FindNeighbours()
    {
        for (int x = 0; x < Cols; x++)
        {
            for (int y = 0; y < Rows; y++)
            {
                if (x - 1 >= 0)
                {
                    Graph[x, y].Neighbours.Add(Graph[x - 1, y]);

                    if (y + 1 < Rows && (y % 2 == 0)) //если чётная строка
                    {
                        Graph[x, y].Neighbours.Add(Graph[x - 1, y + 1]);
                    }

                    if (y - 1 >= 0 && (y % 2 == 0)) //если чётная строка
                    {
                        Graph[x, y].Neighbours.Add(Graph[x - 1, y - 1]);
                    }
                }

                if (x + 1 < Cols)
                {
                    Graph[x, y].Neighbours.Add(Graph[x + 1, y]);

                    if (y + 1 < Rows && (y % 2 != 0)) // и если нечётная строка 
                    {
                        Graph[x, y].Neighbours.Add(Graph[x + 1, y + 1]);
                    }
                }

                if (y - 1 >= 0)
                {
                    Graph[x, y].Neighbours.Add(Graph[x, y - 1]);

                    if (x + 1 < Cols && (y % 2 != 0)) //если нечётная строка 
                    {
                        Graph[x, y].Neighbours.Add(Graph[x + 1, y - 1]);
                    }
                }

                if (y + 1 < Rows)
                {
                    Graph[x, y].Neighbours.Add(Graph[x, y + 1]);
                }
            }
        }
    }
}
