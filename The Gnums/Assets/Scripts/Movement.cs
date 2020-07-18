using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class Movement : MonoBehaviour
{
    /*
    private Animator anim;

    private Vector3 target;

    public float speed = 2f;
    private bool move = false;
    private bool right = true;

    private UnityAction<float, float> someListener;

    private bool _isSelected;

    public bool isSelected
    {
        get
        {
            return _isSelected;
        }

        set
        {
            _isSelected = value;
        }
    }

    private void Awake()
    {
        someListener = new UnityAction<float, float>(MoveSelectedUnitTo);
    }

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    ?
    private void OnEnable()
    {
        EventManager.StartListening("Move", someListener);
    }

    private void OnDisable()
    {
        EventManager.StopListening("Move", someListener);
    }

    //заменено на ReadyToMove в классе UnitCharacteristic
    public void MoveSelectedUnitTo(float x, float y)
    {
        if (!move & isSelected)
        {
            move = true;
            target = new Vector3(x, y + transform.localScale.y / 3, 0);
        }

    }


    public void Flip()
    {
        right = !right;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    // Update is called once per frame
    void Update()
    {

        //Debug.Log(target.x + "target.x");
        if (transform.position.Equals(target))
        {
            move = false;
            if (right)
            {
                anim.SetBool("Go", false);
            }


            isSelected = false;

            if (!right)
            {
                anim.SetBool("Go", false);
                //anim.SetBool("Go2", false);
                Flip();
            }

            target.x = target.x + 1;
            target.y = target.y + 1;
            target.z = target.z + 1;

            EventManager.TriggerEvent("Next");


        }

        if (target.x < transform.position.x && right)
        {
            Flip();
        }
        else if (target.x > transform.position.x && !right)
        {
            Flip();
        }

        if (move)
        {
            if (right)
            {
                anim.SetBool("Go", true);

            }
            if (!right)
            {
                anim.SetBool("Go", true);
                // anim.SetBool("Go2", true);

            }
            transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);

        }

    }*/
}
