using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UnitManipulator : MonoBehaviour
{
    [SerializeField]
    QueueManager qUnit = null;

    private void OnHexClick(OnClickEvent<Hex> param)
    {
        var hex = param.OnClickObject;

        if (hex.isWalkable && qUnit.SelectedUnit.ReachableHexes.Contains(hex)) 
        {
            var currentPath = Pathfinding.Instance.GeneratePathTo(qUnit.SelectedUnit.CurrentHex, hex);

            qUnit.SelectedUnit.Move(currentPath);

            qUnit.SelectedUnit.CurrentHex.isWalkable = true;
            qUnit.SelectedUnit.CurrentHex.render.color = Color.white;

            qUnit.SelectedUnit.CurrentHex = hex;

            qUnit.SelectedUnit.CurrentHex.isWalkable = false;
            qUnit.SelectedUnit.CurrentHex.render.color = Color.black;
        }
    }

    private void OnEnemyClick(OnClickEvent<Unit> param)
    {
        var enemyUnit = param.OnClickObject;

        if (enemyUnit.IsEnemy && qUnit.SelectedUnit.AttackableHexes.Contains(enemyUnit.CurrentHex))
        {
            enemyUnit.GetDamage(qUnit.SelectedUnit);
        }
    }

    private void OnEnable()
    {
        if (EventController.Instance != null)
        {
            EventController.Instance.AddListener<OnClickEvent<Hex>>("Move", OnHexClick);
            EventController.Instance.AddListener<OnClickEvent<Unit>>("Attack", OnEnemyClick);
        }
    }

    private void OnDisable()
    {
        if (EventController.Instance != null)
        {
            EventController.Instance.RemoveListener<OnClickEvent<Hex>>("Move", OnHexClick);
            EventController.Instance.RemoveListener<OnClickEvent<Unit>>("Attack", OnEnemyClick);
        }
    } 
}
