using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitStatsView : MonoBehaviour
{

    public RectTransform unitStatsPanel;
    public Text unitName;
    public Text attack;
    public Text defence;
    public Text damage;
    public Text ammunition;
    public Text health;
    public Text curHealth;
    public Text speed;
    public Text initiative;

    public void SetStatsView(Unit clickedUnit)
    {
        unitStatsPanel.gameObject.SetActive(true);

        unitName.text = clickedUnit.baseUnit.unitName;
        attack.text = "Attack " + clickedUnit.baseUnit.attack.ToString();
        defence.text = "Defence " + clickedUnit.baseUnit.defence.ToString();
        damage.text = "Damage " + clickedUnit.baseUnit.minDamage.ToString() + "-" + clickedUnit.baseUnit.maxDamage.ToString();
        ammunition.text = "Ammunition " + clickedUnit.baseUnit.ammunition.ToString();
        health.text = "Health " + clickedUnit.baseUnit.health.ToString();
        curHealth.text = "Cur. Health " + clickedUnit.baseUnit.currentHealth.ToString();
        speed.text = "Speed " + clickedUnit.baseUnit.speed.ToString();
        initiative.text = "Initiative " + clickedUnit.baseUnit.initiative.ToString();
    }
}
