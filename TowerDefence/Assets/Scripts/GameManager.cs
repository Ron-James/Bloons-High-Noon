using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] Lane [] lanes;
    [SerializeField] float towerHealth = 10f;
    [SerializeField] Image healthBar;
    [SerializeField] int balance;
    [SerializeField] Enemy [] enemies;
    [Range(0, 1)][SerializeField] float sellPercentage = 0.8f;
    public int Balance { get => balance; set => balance = value; }
    public float SellPercentage { get => sellPercentage; set => sellPercentage = value; }

    float totalHealth;
    // Start is called before the first frame update
    void Start()
    {
        totalHealth = towerHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.M)){
            SpawnEnemy(enemies[0].prefab);
        }
    }

    void SpawnEnemy(GameObject enemy){
        int random = Random.Range(0, lanes.Length);
        lanes[random].SpawnEnemy(enemy);
    }

    public void DamageTower(float amount){
        towerHealth -= amount;
        healthBar.fillAmount = towerHealth/totalHealth;
        if(towerHealth <= 0){
            //GameOver
        }
    }

    public void Purchase(int cost){
        balance -= cost;
    }

    public void AddBalance(int amount){
        balance += amount;
    }



}
