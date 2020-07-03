using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BaseCharacteristic
{
    //название юнита
    public string name;
    //параметр атаки
    public int attack;
    //параметр защиты
    public int defense;
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
    public Vector2 spawnLocation;

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

    public Race RaceType;
    public TypeOfArmour ArmourType;
}
