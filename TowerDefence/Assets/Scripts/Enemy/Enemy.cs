using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy", menuName = "Enemy")]
public class Enemy : ScriptableObject
{
    public string enemyName;
    public GameObject prefab;
    public int difficultyRating;
    public float health;

    public int value = 10;

}
