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

        unitName.text = clickedUnit.unitName;
        attack.text = "Attack " + clickedUnit.attack.ToString();
        defence.text = "Defence " + clickedUnit.defence.ToString();
        damage.text = "Damage " + clickedUnit.minDamage.ToString() + "-" + clickedUnit.maxDamage.ToString();
        ammunition.text = "Ammunition " + clickedUnit.ammunition.ToString();
        health.text = "Health " + clickedUnit.health.ToString();
        curHealth.text = "Cur. Health " + clickedUnit.currentHealth.ToString();
        speed.text = "Speed " + clickedUnit.speed.ToString();
        initiative.text = "Initiative " + clickedUnit.initiative.ToString();
    }
}
