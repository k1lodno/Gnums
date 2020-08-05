using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using static EventController;

public class Unit : MonoBehaviour, IPointerDownHandler, IPointerUpHandler//, IPointerEnterHandler
{
    public BaseUnit baseUnit;

    //название юнита
    public string unitName;
    //параметр атаки
    public int attack;
    //параметр защиты
    public int defence;
    //боезапас
    public int ammunition;
    //минимальный урон 
    public int minDamage;
    //максимальынй урон
    public int maxDamage;
    //здоровье 
    public int health;
    //текущее здоровье
    public int currentHealth;
    //дальность пермещения
    public int speed;
    //инициатива юнита
    public int initiative;
    //прирост юнитов за ед. времени
    public int growth;
    //стоимость одного юнита
    public int cost;

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
    private List<Hex> reachableHexes = new List<Hex>();
    private List<Hex> attackableHexes = new List<Hex>();

    public Hex CurrentHex { get => currentHex; set => currentHex = value; }
    public bool IsMoving { get => isMoving; set => isMoving = value; }
    public List<Hex> ReachableHexes { get => reachableHexes; set => reachableHexes = value; }
    public List<Hex> AttackableHexes { get => attackableHexes; set => attackableHexes = value; }

    void Start()
    {
        unitName = baseUnit.unitName;
        attack = baseUnit.attack;
        defence = baseUnit.defence;
        ammunition = baseUnit.ammunition;
        minDamage = baseUnit.minDamage;
        maxDamage = baseUnit.maxDamage;
        health = baseUnit.health;
        currentHealth = baseUnit.currentHealth;
        speed = baseUnit.speed;
        initiative = baseUnit.initiative;

        view = GameObject.Find("UnitStatsView");
        startPosition = transform.position;
    }

    void Update()
    {

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
            v.render.color = Color.black;
            ReachableHexes.Add(v);
            GetMovableRange(v, s + 1);
        }
    }

    public void GetAttackRange(Hex curHex, int i)
    {
        if (baseUnit.unitType == BaseUnit.TypeOfUnit.MELEE)
        {
            foreach (var v in curHex.Neighbours)
            {
                AttackableHexes.Add(v);
            }
        }
        else
        {
            //рейнж атаки для рейнжевиков, потом добавить сломанную стрелу
            if (i >= 17)
                return;

            foreach (var v in curHex.Neighbours)
            {
                AttackableHexes.Add(v);
                GetAttackRange(v, i + 1);
            }
        }
    }

    public void GetDamage(Unit attacker)
    {
        System.Random rnd = new System.Random();
        int dmg = rnd.Next(attacker.baseUnit.minDamage, attacker.baseUnit.maxDamage);
        baseUnit.currentHealth -= dmg;
        Debug.Log(baseUnit.unitName + " получает " + dmg + " урона");
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

                    //EventManager.Instance.TriggerEvent("Next");
                    EventController.Instance.TriggerEvent("Next", new BaseEvent());

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
        if (eventData.button == PointerEventData.InputButton.Right)
            view.GetComponent<UnitStatsView>().SetStatsView(this);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
            view.GetComponent<UnitStatsView>().unitStatsPanel.gameObject.SetActive(false);
    }

    /*
    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("detect");
    }

    void OnMouseEnter()
    {
        Debug.Log("detect");
    }*/

    void OnMouseDown()
    {
        //EventManager.Instance.TriggerEvent("Attack", this);
        EventController.Instance.TriggerEvent("Attack", new OnClickEvent<Unit>(this));
    }
}



