using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New Unit", menuName = "Unit")]
public class BaseUnit: ScriptableObject
{
    //раса юнита
    public enum Race
    {
        DWARF,
        HUMAN,
        ORC
    }

    //тип брони юнита
    public enum TypeOfArmour
    {
        LIGHT,
        MEDIUM,
        HEAVY
    }

    public enum TypeOfUnit
    {
        MELEE,
        RANGE
    }

    public Race raceType;
    public TypeOfArmour armourType;
    public TypeOfUnit unitType;

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

    //координаты спавна юнита
    //public Vector2 spawnLocation;
}
