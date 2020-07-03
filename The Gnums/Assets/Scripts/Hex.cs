using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.EventSystems;

//класс гекса
public class Hex : MonoBehaviour
{
    public  int q; //column
    public  int r; //row
    public  int s;

    public List<Hex> neighbours = new List<Hex>(); //соседи гекса

    bool mouse;
    SpriteRenderer render;

    public bool isWalkable; 

    public Action<Hex> onHexClick; 

    void Start()
    {
        render = gameObject.GetComponentInChildren<SpriteRenderer>();
        isWalkable = true;
    }

    public void SetCoord(int q, int r)
    {
        this.q = q;
        this.r = r;
        this.s = -(q + r);
    }

    private void OnMouseEnter()
    {
        mouse = true;
        render.color = Color.black;
    }

    private void OnMouseExit()
    {
        mouse = false;
        render.color = Color.white;
    }

    private void OnMouseDown()
    {

        FindObjectOfType<Grid>().selectedUnit.GetComponent<UnitCharacteristic>().isSelected = true;

        onHexClick?.Invoke(this);

        /*
        if (EventSystem.current.IsPointerOverGameObject())
        {
            FindObjectOfType<Grid>().selectedUnit.GetComponent<Movement>().isSelected = true;
            EventManager.TriggerEvent("Move", q, r);
        }*/
    }

    public float DistanceTo(Hex n)
    {
        if (n == null)
        {
            Debug.LogError("WTF?");
        }

        return Vector2.Distance(
            new Vector2(q, r),
            new Vector2(n.q, n.r)
            );
    }
}


