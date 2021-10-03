using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] Lane [] lanes;
    [SerializeField] GameObject basicEnemy;
    [SerializeField] float towerHealth = 10f;

    List<GameObject> basicDeadEnemies = new List<GameObject>();

    public List<GameObject> BasicDeadEnemies { get => basicDeadEnemies; set => basicDeadEnemies = value; }

    // Start is called before the first frame update
    void Start()
    {
        
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
    }



}
