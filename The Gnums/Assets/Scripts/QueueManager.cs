using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using static EventController;

public class QueueManager : MonoBehaviour
{
    [SerializeField]
    GameObject queueMemberPrefab = null;

    [SerializeField]
    GameObject turnPrefab = null;

    [SerializeField]
    UiHandler scroll = null;

    private Unit selectedUnit; //активный юнит
    List<Unit> queueList; //лист всех юнитов 

    //Dictionary<Unit, GameObject> oneTurnDict = new Dictionary<Unit, GameObject>();
    List<GameObject> oneTurnList = new List<GameObject>();
    GameObject queueGO;

    int index = 0; //индекс юнита в массиве юнитов
    //беспонтовые счётчики
    int k = 0;
    int l;
    int numberOfTurnsInScroll;

    public Unit SelectedUnit { get => selectedUnit; set => selectedUnit = value; }
    public List<Unit> QueueList { get => queueList; set => queueList = value; }

    private void Awake()
    {
        QueueList = new List<Unit>();
    }
    void Start()
    {        
        SelectedUnit = QueueList[index];
        SelectedUnit.GetMovableRange(SelectedUnit.CurrentHex, 0);
        SelectedUnit.GetAttackRange(SelectedUnit.CurrentHex, 0);

        InitQueue();
    }

    /*
GameObject val;

for (int j = 0; j < QueueList.Count; j++)
{
    oneTurnDict.Add(QueueList[j], queueMemberPrefab);
}

for (int i = 0; i < numberOfTurns; i++)
{
    for (int j = 0; j < QueueList.Count; j++)
    {

        oneTurnDict.TryGetValue(QueueList[j], out val);
        val.gameObject.GetComponent<Image>().sprite = QueueList[j].UnitAvatar;

        Instantiate(val, GetComponent<ScrollRect>().content);

    }

    queueGO = Instantiate(turnPrefab, GetComponent<ScrollRect>().content);
    queueGO.GetComponentInChildren<Text>().text = (i + 1).ToString();
}*/

    public void InitQueue()
    {       
        int scrollCapacity = (int) (GetComponent<RectTransform>().rect.width / queueMemberPrefab.GetComponent<RectTransform>().rect.width);
        int turnLength = QueueList.Count + 1;

        numberOfTurnsInScroll = scrollCapacity / turnLength;
        
        if(scrollCapacity % turnLength != 0)
        {
            numberOfTurnsInScroll += 1;
        }

        for (int i = 1; i <= numberOfTurnsInScroll; i++)
        {
            for (int j = 0; j < QueueList.Count; j++)
            {
                oneTurnList.Add(queueMemberPrefab);
                oneTurnList[j].gameObject.GetComponent<Image>().sprite = QueueList[j].UnitAvatar;
                Instantiate(oneTurnList[j], GetComponent<ScrollRect>().content);
            }

            queueGO = Instantiate(turnPrefab, GetComponent<ScrollRect>().content);
            queueGO.GetComponentInChildren<Text>().text = (i).ToString();
        }
    }

    /*
           GameObject go;
           oneTurnDict.TryGetValue(deadUnit, out go);
           Destroy(go);
           oneTurnDict.Remove(deadUnit);*/

    public void UpdateQueue(OnClickEvent<Unit> p)
    {
        var deadUnit = p.OnClickObject;

        if (QueueList.Contains(deadUnit))
        {            
            QueueList.Remove(deadUnit);
            oneTurnList.Clear();

            foreach(Transform child in GetComponent<ScrollRect>().content)
            {
                Destroy(child.gameObject);
            }

            InitQueue();
        }
    }

    public void AddToQueue()
    {
        Debug.Log("here");

        if (k >= QueueList.Count)
        {
            queueGO = Instantiate(turnPrefab, GetComponent<ScrollRect>().content);
            queueGO.GetComponentInChildren<Text>().text = (l).ToString();
            l++;
            k = 0;
        }

        Instantiate(oneTurnList[k], GetComponent<ScrollRect>().content);
        k++;
        l = numberOfTurnsInScroll;
    }

    public void Queue(BaseEvent baseEvent)
    {
        index++;

        if (index == QueueList.Count)
        {
            index = 0;
        }

        if (index < QueueList.Count)
        {
            SelectedUnit = QueueList[index];

            //чистим предыдующие доступные гексы 
            if (SelectedUnit.ReachableHexes != null)
            {
                SelectedUnit.ReachableHexes.Clear();
            }

            SelectedUnit.GetMovableRange(SelectedUnit.CurrentHex, 0);
            
            if (SelectedUnit.AttackableHexes != null)
            {
                SelectedUnit.AttackableHexes.Clear();           
            }

            SelectedUnit.GetAttackRange(SelectedUnit.CurrentHex, 0);
        }

        scroll.OnNextClick();
    }


    private void OnEnable()
    {
        if (EventController.Instance != null)
        {
            EventController.Instance.AddListener<BaseEvent>("Next", Queue);
            EventController.Instance.AddListener<OnClickEvent<Unit>>("Death", UpdateQueue);
        }
    }

    private void OnDisable()
    {
        if (EventController.Instance != null)
        {
            EventController.Instance.RemoveListener<BaseEvent>("Next", Queue);
            EventController.Instance.RemoveListener<OnClickEvent<Unit>>("Death", UpdateQueue);
        }
    }
}
