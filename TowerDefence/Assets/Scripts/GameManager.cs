using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] Lane [] lanes;
    [SerializeField] GameObject basicEnemy;
    [SerializeField] float towerHealth = 10f;
    [SerializeField] Image healthBar;
    [SerializeField] int balance;

    List<GameObject> basicDeadEnemies = new List<GameObject>();

    public List<GameObject> BasicDeadEnemies { get => basicDeadEnemies; set => basicDeadEnemies = value; }
    public int Balance { get => balance; set => balance = value; }

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
            SpawnEnemy(basicEnemy);
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



}
