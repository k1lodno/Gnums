using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.EventSystems;


public class Hex : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private int q; //column
    private int r; //row
    //public int s;

    private List<Hex> neighbours = new List<Hex>(); //соседи гекса

    private bool mouse;

    public SpriteRenderer render;

    public bool isWalkable;

    public int Q { get => q; set => q = value; }
    public int R { get => r; set => r = value; }
    public List<Hex> Neighbours { get => neighbours; set => neighbours = value; }

    private void Awake()
    {
        isWalkable = true;
    }

    public void SetCoord(int q, int r)
    {
        this.Q = q;
        this.R = r;
        //this.s = -(q + r);
    }

    private void OnMouseDown()
    {
        EventController.Instance.TriggerEvent("Move", new OnClickEvent<Hex>(this));
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        mouse = true;
        render.color = Color.black;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        mouse = false;
        render.color = Color.white;
    }
}


