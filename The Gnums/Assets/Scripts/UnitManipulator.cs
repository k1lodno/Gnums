using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UnitManipulator : MonoBehaviour
{
    [SerializeField]
    SpawnManager spawn;

    private UnityAction<Hex> hexListener;

    private void Awake()
    {
        hexListener = new UnityAction<Hex>(OnHexClick);
    }

 
    private void OnHexClick(Hex hex)
    {
        var currentPath = Pathfinding.Instance.GeneratePathTo(spawn.SelectedUnit.CurrentHex, hex);

        spawn.SelectedUnit.Move(currentPath);

        //поидее здесь не правильно
        spawn.SelectedUnit.CurrentHex.isWalkable = true;
        spawn.SelectedUnit.CurrentHex.GetComponent<SpriteRenderer>().color = Color.white;
        
        spawn.SelectedUnit.CurrentHex = hex;

        spawn.SelectedUnit.CurrentHex.isWalkable = false;
        spawn.SelectedUnit.CurrentHex.GetComponent<SpriteRenderer>().color = Color.black;
    }

    private void OnEnable()
    {
        EventManager.Instance.StartListening("Move", hexListener);

    }

    private void OnDisable()
    {
        EventManager.Instance.StartListening("Move", hexListener);

    }

  
}
