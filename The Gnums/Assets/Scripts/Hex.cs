using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.EventSystems;


public class Hex : MonoBehaviour
{
    private int q; //column
    private int r; //row
    //public int s;

    private List<Hex> neighbours = new List<Hex>(); //соседи гекса

    private bool mouse;
    private SpriteRenderer render;

    public bool isWalkable;

    public int Q { get => q; set => q = value; }
    public int R { get => r; set => r = value; }
    public List<Hex> Neighbours { get => neighbours; set => neighbours = value; }

    //public Action<Hex> onHexClick;

    private void Awake()
    {
        isWalkable = true;
    }

    void Start()
    {
        render = gameObject.GetComponentInChildren<SpriteRenderer>();
    }

    public void SetCoord(int q, int r)
    {
        this.Q = q;
        this.R = r;
        //this.s = -(q + r);
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
        //FindObjectOfType<Grid>().selectedUnit.GetComponent<UnitCharacteristic>().isSelected = true;
        //onHexClick?.Invoke(this);
        EventManager.Instance.TriggerEvent("Move", this);
    }
}


