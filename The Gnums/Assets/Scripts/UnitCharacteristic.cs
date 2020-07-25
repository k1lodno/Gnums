using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UnitCharacteristic : MonoBehaviour {

    //вылетают ошибки при попытке состряпать свойство
    public BaseCharacteristic unit;

    int currNode;

    float speed = 2f;
    float timer = 0;

    static Vector3 currentPosition;
    Vector3 startPosition;

    [SerializeField]
    private Animator anim;

    private Vector3 target;

    private bool move = false;
    private bool right = true;

    private Hex currentHex;

    public Hex CurrentHex { get => currentHex; set => currentHex = value; }

    void Start()
    {
        currNode = 0;
        startPosition = transform.position;
    }

    void Update()
    {

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

        if (move)
        {
            if (transform.position.Equals(target))
            {
                /*
                Debug.Log(transform.position);
                Debug.Log(target);

                Debug.Log(currentPath[currNode].gameObject.transform.position);
                Debug.Log(currentPath[currentPath.Count - 1].gameObject.transform.position);
                //гекс до которого дошёл юнит
                //currentPath[currNode].isWalkable = false;*/

                move = false;

                if (right)
                {
                    anim.SetBool("Go", false);
                }

                //isSelected = false;

                if (!right)
                {
                    anim.SetBool("Go", false);
                    Flip();
                }

                target.x = target.x + 1;
                target.y = target.y + 1;
                target.z = target.z + 1;

                //какого то хера всё равно вызывается хотя не должен проходить иф
                //Debug.Log(transform.position);
                //Debug.Log(target);
                EventManager.Instance.TriggerEvent("Next");
            }
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

    public void Spawn(Hex hex)
    {
        transform.localPosition = hex.transform.localPosition;
        CurrentHex = hex;
    }

    public void Move(List<Hex> currentPath)
    {
        if (!move)
        {
            move = true;
            target = currentPath[currentPath.Count - 1].gameObject.transform.position;
            target = new Vector3(target.x, target.y, 0);
            StartCoroutine(MoveAlongPath(currentPath));
        }
    }

    //передвижение юнита по определённому пути
    IEnumerator MoveAlongPath(List<Hex> currentPath)
    {
        currNode = 0; 

        while (true)
        {
            //currentPosition некоректное название (?)

            //вылетала ошибка индекс аут оф рейгдж почему хз
            currentPosition = currentPath[currNode].gameObject.transform.position;
            currentPosition = new Vector3(currentPosition.x, currentPosition.y, 0);

            timer += Time.deltaTime * speed;

            if (transform.position != currentPosition)
            {
                Debug.Log(currNode);
                move = true;
                DrawPath(currentPath);
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
                    currentPosition = new Vector3(currentPosition.x, currentPosition.y, 0);
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
    private void DrawPath(List<Hex> currentPath)
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
