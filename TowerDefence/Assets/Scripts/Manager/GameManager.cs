using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] float towerHealth = 10f;
    [SerializeField] Transform spawnPointMin;
    [SerializeField] Transform spawnPointMax;
    [SerializeField] Image healthBar;
    [SerializeField] int balance;
    [SerializeField] Enemy [] enemies;
    [Range(0, 1)][SerializeField] float sellPercentage = 0.8f;
    [SerializeField] GameObject deadEnemies;
    [SerializeField] GameObject aliveEnemies;
    [SerializeField] Text balanceTxt;
    [SerializeField] BuildPlate [] buildPlates;

    public int Balance { get => balance; set => balance = value; }
    public float SellPercentage { get => sellPercentage; set => sellPercentage = value; }

    float totalHealth;
    // Start is called before the first frame update
    void Start()
    {
        totalHealth = towerHealth;
        UpdateBalanceText();
        Debug.Log("Dead enemies " + deadEnemies.GetComponentsInChildren<Transform>().Length + deadEnemies.GetComponentsInChildren<Transform>()[0].gameObject.name);
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.M)){
            SpawnEnemy(enemies[2]);
        }
    }

    public void SpawnEnemy(Enemy enemy){
        Vector3 position;
        position.x = spawnPointMin.position.x;
        position.y = spawnPointMin.position.y;
        position.z = Random.Range(spawnPointMin.position.z, spawnPointMax.position.z);
        Transform [] dead = deadEnemies.GetComponentsInChildren<Transform>();
        if(dead.Length > 1){
            Instantiate(enemy.prefab, position, Quaternion.identity, aliveEnemies.transform);
        }
        else{
            for(int loop = 1; loop < dead.Length; loop++){
                if(dead[loop].gameObject.GetComponent<EnemyHealth>().Enemy.enemyName == enemy.enemyName){
                    dead[loop].gameObject.GetComponent<EnemyHealth>().Revive(position);
                    break;
                }
            }
        }

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
        UpdateBalanceText();
    }

    public void AddBalance(int amount){
        balance += amount;
        UpdateBalanceText();
    }

    void UpdateBalanceText(){
        balanceTxt.text = "Balance: " + Balance.ToString();
    }

    public BuildPlate NearestTower(){
        for(int loop = buildPlates.Length - 1; loop >= 0; loop--){
            if(buildPlates[loop].BuildIndex > 0){
                return buildPlates[loop];
            }
        }
        return null;
    }


}
