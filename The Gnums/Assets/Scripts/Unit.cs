using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using static EventController;

public class Unit : MonoBehaviour, IPointerDownHandler, IPointerUpHandler//, IPointerEnterHandler
{
    public BaseUnit baseUnit;

    //количество юнитов в стеке
    private int numberOfUnits;

    //параметр атаки
    private int attack;
    //параметр защиты
    private int defence;
    //боезапас
    private int ammunition;
    //минимальный урон 
    private int minDamage;
    //максимальынй урон
    private int maxDamage;
    //здоровье 
    private int health;
    //текущее здоровье
    private int currentHealth;
    //дальность пермещения
    private int speed;
    //инициатива юнита
    private int initiative;

    private GameObject view;

    int currNode = 0;
    float movespeed = 2f;
    float timer = 0;

    static Vector3 currentPosition;
    Vector3 startPosition;
    private Vector3 target;

    System.Random rnd = new System.Random();

    [SerializeField]
    private Animator anim = null;

    [SerializeField]
    TextMesh numOfUnits = null;

    [SerializeField]
    GameObject numPanelPrefab = null;

    private bool isMoving = false;
    private bool right = true;
    bool isEnemy = false;

    private Hex currentHex;
    private List<Hex> reachableHexes = new List<Hex>();
    private List<Hex> attackableHexes = new List<Hex>();

    public Hex CurrentHex { get => currentHex; set => currentHex = value; }
    public bool IsMoving { get => isMoving; set => isMoving = value; }
    public List<Hex> ReachableHexes { get => reachableHexes; set => reachableHexes = value; }
    public List<Hex> AttackableHexes { get => attackableHexes; set => attackableHexes = value; }
    public int Attack { get => attack; set => attack = value; }
    public int Defence { get => defence; set => defence = value; }
    public int Ammunition { get => ammunition; set => ammunition = value; }
    public int MinDamage { get => minDamage; set => minDamage = value; }
    public int MaxDamage { get => maxDamage; set => maxDamage = value; }
    public int Health { get => health; set => health = value; }
    public int CurrentHealth { get => currentHealth; set => currentHealth = value; }
    public int Speed { get => speed; set => speed = value; }
    public int Initiative { get => initiative; set => initiative = value; }
    public int NumberOfUnits { get => numberOfUnits; set => numberOfUnits = value; }
    public bool IsEnemy { get => isEnemy; set => isEnemy = value; }

    void Awake()
    {
        Attack = baseUnit.attack;
        Defence = baseUnit.defence;
        Ammunition = baseUnit.ammunition;
        Speed = baseUnit.speed;
        Initiative = baseUnit.initiative;
        MinDamage = baseUnit.minDamage;
        MaxDamage = baseUnit.maxDamage;
        Health = baseUnit.health;
        CurrentHealth = baseUnit.health;
    }

    void Start()
    {
        var go = Instantiate(numPanelPrefab, transform.position, Quaternion.identity, transform);
        go.GetComponentInChildren<TextMesh>().text = NumberOfUnits.ToString();

        numOfUnits.text = NumberOfUnits.ToString();
        view = GameObject.Find("UnitStatsView");
        startPosition = transform.position;
    }

    void Update()
    {

        if (target.x < transform.position.x && right)
        {
            //кроме юнита флипается и табличка
            Flip();
        }

        else if (target.x > transform.position.x && !right)
        {
            Flip();
        }
    }

    public void GetMovableRange(Hex curHex, int s)
    {
        if (s >= Speed)
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

    /*
     * Атака и защита героя прибавляется к атаке и защите монстра. 
     * расчёт повреждений рассчитывается так: 
     * если уровень атаки атакующего монстра превышает уровень защиты защищающегося монстра, 
     * то за каждый уровень превышения повреждения увеличиваются на 5%. Если же защита больше, то повреждения уменьшаются на 3%. 
     * Например уровень атаки монстра с учётом всех бонусов - 20, а уровень защиты монстра противника - 15. 
     * Повреждения увеличатся на (20 - 15)х5%= 25%, если же атака 20, а защита 25, то повреждения уменьшатся на (25 - 20) х 3% = 15%.
     * 
     * наносимые повреждения не могут составить свыше 400% или меньше 30% от значения, наносимого при равенстве атаки атакующего и защиты атакуемого.
     * 
     * высчитывается урон одного монстра другому в случае 1*1, затем умножается на кол-во нападающих монстров. 
      */
    public void GetDamage(Unit attacker)
    {
        int dmg = rnd.Next(attacker.MinDamage, attacker.MaxDamage);
        dmg *= attacker.NumberOfUnits;
        int statDiff = 0;

        if (attacker.Attack > Defence)
        {
            statDiff = (attacker.Attack - Defence) * 5;
        }
        else if (attacker.Attack < Defence)
        {
            statDiff = (Defence - attacker.Attack) * 5;
        }

        int totalDmg = dmg * (1 + statDiff / 100);

        CurrentHealth -= totalDmg;

        Debug.Log(baseUnit.unitName + " получает " + totalDmg + " урона");
        Debug.Log(baseUnit.unitName + " Текущее здоровье: " + CurrentHealth);

        int deathRate = totalDmg / baseUnit.health;
        NumberOfUnits -= deathRate;

        Debug.Log("Количество юнитов: " + NumberOfUnits);
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
        EventController.Instance.TriggerEvent("Attack", new OnClickEvent<Unit>(this));
    }
}



