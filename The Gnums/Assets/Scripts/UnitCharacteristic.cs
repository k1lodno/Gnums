using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UnitCharacteristic : MonoBehaviour {

    public BaseCharacteristic unit;

    public int tileX, tileY;
    public Grid map;

    public List<Hex> currentPath;
    int currNode;

    public float speed = 2f;

    float timer = 0;
    static Vector3 currentPosition;
    Vector3 startPosition;

    private Animator anim;

    private Vector3 target;

    private bool move = false;
    private bool right = true;

    //private UnityAction<float, float> someListener;

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

    void Start()
    {
        anim = GetComponent<Animator>();
        currentPath = null;
        currNode = 0;
        startPosition = transform.position;
    }

    void Update()
    {
        if (currentPath != null) {}

        if (move)
        {
            if (right)
            {
                anim.SetBool("Go", true);
            }
            if (!right)
            {
                anim.SetBool("Go", true);
            }
        }

        if (transform.position.Equals(target))
        {
            Debug.Log(transform.position);
            Debug.Log(target);

            Debug.Log(currentPath[currNode].gameObject.transform.position);
            Debug.Log(currentPath[currentPath.Count - 1].gameObject.transform.position);
            //гекс до которого дошёл юнит
            currentPath[currNode].isWalkable = false;

            move = false;
         
            if (right)
            {
                anim.SetBool("Go", false);
            }

            isSelected = false;

            if (!right)
            {
                anim.SetBool("Go", false);
                Flip();
            }

            target.x = target.x + 1;
            target.y = target.y + 1;
            target.z = target.z + 1;

            //какого то хера всё равно вызывается хотя не должен проходить иф
            Debug.Log(transform.position);
            Debug.Log(target);
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
    }

    
    public void ReadyToMove()
    {
        if (!move & isSelected)
        {
            move = true;
            target = currentPath[currentPath.Count - 1].gameObject.transform.position;
            target = new Vector3(target.x, target.y + transform.localScale.y / 3, 0);
            StartCoroutine(MoveAlongPath());
        }
    }

    //передвижение юнита по определённому пути
    IEnumerator MoveAlongPath()
    {
        currNode = 0; 

        while (true)
        {
            //currentPosition некоректное название (?)

            //вылетала ошибка индекс аут оф рейгдж почему хз
            currentPosition = currentPath[currNode].gameObject.transform.position;
            currentPosition = new Vector3(currentPosition.x, currentPosition.y + transform.localScale.y / 3, 0);

            timer += Time.deltaTime * speed;

            if (transform.position != currentPosition)
            {
                Debug.Log(currNode);
                move = true;
                DrawPath();
                transform.position = Vector3.Lerp(startPosition, currentPosition, timer); //двигаем юнита
            }
            else
            {
                if (currNode < currentPath.Count - 1)
                {
                    Debug.Log("kek");
                    currNode++;
                    timer = 0;
                    startPosition = transform.position;
                    currentPosition = currentPath[currNode].gameObject.transform.position;
                    currentPosition = new Vector3(currentPosition.x, currentPosition.y + transform.localScale.y / 3, 0);
                }
                else
                {
                    yield break;
                }
            }
            yield return null;
        }
    }

    //отрисовка пути
    private void DrawPath()
    {
        for(int i = 0; i< currentPath.Count - 1; i++)
        {
            Vector3 start = currentPath[i].gameObject.transform.position + new Vector3(0, 0, -2);
            Vector3 end = currentPath[i + 1].gameObject.transform.position + new Vector3(0, 0, -2);

            Debug.DrawLine(start, end, Color.black);
        }
    }


    public void Flip()
    {
        right = !right;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

}
