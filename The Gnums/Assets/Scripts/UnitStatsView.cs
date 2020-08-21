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

        unitName.text = clickedUnit.BaseUnit.unitName;
        attack.text = "Attack " + clickedUnit.Attack.ToString();
        defence.text = "Defence " + clickedUnit.Defence.ToString();
        damage.text = "Damage " + clickedUnit.MinDamage.ToString() + "-" + clickedUnit.MaxDamage.ToString();
        ammunition.text = "Ammunition " + clickedUnit.Ammunition.ToString();
        health.text = "Health " + clickedUnit.Health.ToString();
        curHealth.text = "Cur. Health " + clickedUnit.CurrentHealth.ToString();
        speed.text = "Speed " + clickedUnit.Speed.ToString();
        initiative.text = "Initiative " + clickedUnit.Initiative.ToString();
    }
}
