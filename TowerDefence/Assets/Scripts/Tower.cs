using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Tower", menuName = "Tower")]
public class Tower : ScriptableObject
{
    public string towerName;
    public int cost;
    public GameObject tower;
    public int health = 3;

    
    public int CalculateSellValue(){
        return (int) GameManager.instance.SellPercentage * cost;
    }
}
