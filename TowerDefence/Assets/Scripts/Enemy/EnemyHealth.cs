using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    
    [SerializeField] Enemy enemy;
    [SerializeField] GameObject body;


    float maxHealth;
    [SerializeField] float health;
    [SerializeField] bool tutorialEnemy = false;
    [SerializeField] bool secondTutorialEnemy = false;
    GameObject deadEnemies;
    GameObject aliveEnemies;
    [SerializeField] bool invisible = false;
    public bool isAlive = true;

    public float Health { get => health; set => health = value; }
    public Enemy Enemy { get => enemy; set => enemy = value; }
    public bool Invisible1 { get => invisible; set => invisible = value; }

    // Start is called before the first frame update
    void Start()
    {
        
        health = enemy.health;
        maxHealth = health;
        deadEnemies = GameManager.instance.DeadEnemies;
        aliveEnemies = GameManager.instance.AliveEnemies;

        if(!isAlive){
            Kill();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void TakeDamage(float damage){
        health -= damage;
        if(health <= 0){
            Kill();
            if(tutorialEnemy && secondTutorialEnemy){
                GameManager.instance.CompleteLevel();
            }
            else if(tutorialEnemy && !secondTutorialEnemy){
                GameManager.instance.SpawnRangedEnemy();
            }
        }
    }

    public void Kill(){
        health = 0;
        isAlive = false;
        this.transform.SetParent(deadEnemies.transform);
        this.transform.position = deadEnemies.transform.position;
        Deactivate();
        
    }
    
    public void Deactivate(){
        body.SetActive(false);
        GetComponent<EnemyFollower>().enabled = false;
        //GetComponent<MeshRenderer>().enabled = false;
        if(GetComponent<EnemyRanged>() != null){
            GetComponent<EnemyRanged>().enabled = false;
        }
        if(GetComponent<EnemyDamage>() != null){
            GetComponent<EnemyDamage>().enabled = false;
        }
        GetComponent<Collider>().enabled = false;
        GetComponent<Animator>().enabled = false;
        if(GetComponentInChildren<EnemyTargetTrigger>() != null){
            GetComponentInChildren<EnemyTargetTrigger>().enabled = false;
        }
        
        
    }
    public void Activate(){
        body.SetActive(true);
        GetComponent<EnemyFollower>().enabled = true;
        if(GetComponent<EnemyRanged>() != null){
            GetComponent<EnemyRanged>().enabled = true;
        }
        if(GetComponent<EnemyDamage>() != null){
            GetComponent<EnemyDamage>().enabled = true;
        }
        if(GetComponentInChildren<EnemyTargetTrigger>() != null){
            GetComponentInChildren<EnemyTargetTrigger>().enabled = true;
        }
        //GetComponent<MeshRenderer>().enabled = true;
        GetComponent<Collider>().enabled = true;
        GetComponent<Animator>().enabled = true;
    }

    public void Revive(Vector3 position){
        if(transform.parent != deadEnemies.transform){
            return;
        }
        else{
            health = maxHealth;
            isAlive = true;
            this.transform.SetParent(aliveEnemies.transform);
            this.transform.position = position;
            Activate();
        }
    }
    public void Invisible(){
        invisible = true;
        body.SetActive(false);
    }
    public void Visible(){
        invisible = false;
        body.SetActive(true);
    }
}
