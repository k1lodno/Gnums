using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Pathfinding : Singleton<Pathfinding>
{
    [SerializeField]
    private Grid gridExample;

    //дийкстра
    public List<Hex> GeneratePathTo(Hex source, Hex target)
    {

        
        if (!gridExample.Graph[target.Q, target.R].isWalkable)
        {
            Debug.Log("стопэ");
            return null;
        }

        Dictionary<Hex, float> dist = new Dictionary<Hex, float>(); //дистанция от сёрса
        Dictionary<Hex, Hex> prev = new Dictionary<Hex, Hex>();     //предыдущий в оптимальном пути

        // непосещённые ноды
        List<Hex> unvisited = new List<Hex>();

        dist[source] = 0;
        prev[source] = null;

        foreach (Hex v in gridExample.Graph)
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
            Hex u = null; // u - непосещённый нод с минимальной дистанцей

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

            foreach (Hex v in u.Neighbours)
            {
                float alt = dist[u] + CostToEnterTile(u, v);

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
            return null;
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

        return currentPath;
    }

    public float CostToEnterTile(Hex u, Hex v)
    {
        float cost = 1;

        if (u.Q != v.Q && u.R != v.R)
        {
            cost += 0.001f; // фикс излишней диагональности (?)
        }

        //повышаем стоимость занятой клетки чтобы обходить её
        if (!v.isWalkable)
        {
            cost = cost + 10;
            return cost;
        }

        return cost;
    }

}
