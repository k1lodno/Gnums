using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class Unit : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler
{
    public BaseUnit baseUnit;

    private GameObject view;

    int currNode = 0;
    float movespeed = 2f;
    float timer = 0;

    static Vector3 currentPosition;
    Vector3 startPosition;
    private Vector3 target;

    [SerializeField]
    private Animator anim;

    private bool isMoving = false;
    private bool right = true;

    private Hex currentHex;

    public Hex CurrentHex { get => currentHex; set => currentHex = value; }
    public bool IsMoving { get => isMoving; set => isMoving = value; }

    void Start()
    {
        view = GameObject.Find("UnitStatsView");
        startPosition = transform.position;
    }

    void Update()
    {

        /*
        if (Input.GetMouseButton(1))
        {
            
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            if (GetComponent<Collider2D>().OverlapPoint(mousePosition))
            {
                Debug.Log("зажато");
                Debug.Log(view);
                view.GetComponent<UnitStatsView>().SetStatsView(this);
                //view.SetStatsView(this);
            }

            view.GetComponent<UnitStatsView>().SetStatsView(this);
        }
        else
        {
            view.GetComponent<UnitStatsView>().unitStatsPanel.gameObject.SetActive(false);
        }
    */


        if (target.x < transform.position.x && right)
        {
            Flip();
        }

        else if (target.x > transform.position.x && !right)
        {
            Flip();
        }
    }

    public void GetMovableRange(Hex curHex, int s)
    {
        if (s >= baseUnit.speed)
            return;

        foreach (var v in curHex.Neighbours)
        {
            v.GetComponent<SpriteRenderer>().color = Color.black;

            GetMovableRange(v, s + 1);
        }
    }

    public void Spawn(Hex hex)
    {
        transform.localPosition = new Vector3(hex.transform.localPosition.x, hex.transform.localPosition.y, transform.localPosition.z);
        CurrentHex = hex;
    }

    public void Move(List<Hex> currentPath)
    {       
        if (!IsMoving && currentPath != null)
        {
            IsMoving = true;
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
            //вылетала ошибка индекс аут оф рейгдж 
            currentPosition = currentPath[currNode].gameObject.transform.position;
            currentPosition = new Vector3(currentPosition.x, currentPosition.y, transform.position.z);

            timer += Time.deltaTime * movespeed;

            if (transform.position != currentPosition)
            {
                IsMoving = true;
                anim.SetBool("Go", true);
                DrawPath(currentPath);
                transform.position = Vector3.Lerp(startPosition, currentPosition, timer); //двигаем юнита
            }
            else
            {
                if (currNode < currentPath.Count - 1)
                {
                    currNode++;
                    timer = 0;
                    startPosition = transform.position;
                    currentPosition = currentPath[currNode].gameObject.transform.position;
                    currentPosition = new Vector3(currentPosition.x, currentPosition.y, transform.position.z);
                }
                else
                {
                    IsMoving = false;

                    if (right)
                    {
                        anim.SetBool("Go", false);
                    }
                    else
                    {
                        anim.SetBool("Go", false);
                        Flip();
                    }

                    EventManager.Instance.TriggerEvent("Next");

                    yield break;
                }
            }
            yield return null;
        }
    }

    //отрисовка пути
    private void DrawPath(List<Hex> currentPath)
    {
        for (int i = 0; i < currentPath.Count - 1; i++)
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

    public void OnPointerDown(PointerEventData eventData)
    {
        //if (eventData.button == PointerEventData.InputButton.Right)
        Debug.Log("Right click");
        view.GetComponent<UnitStatsView>().SetStatsView(this);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        //if (eventData.button == PointerEventData.InputButton.Right)
        Debug.Log("Right click");
        view.GetComponent<UnitStatsView>().unitStatsPanel.gameObject.SetActive(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
            Debug.Log("Right click");
    }
}



