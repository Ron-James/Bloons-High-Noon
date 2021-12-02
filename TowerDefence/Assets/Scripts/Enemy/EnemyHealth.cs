using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    
    [SerializeField] Enemy enemy;
    [SerializeField] GameObject body;


    float maxHealth;
    [SerializeField] float health;
    public bool tutorialEnemy = false;
    public bool secondTutorialEnemy = false;
    GameObject deadEnemies;
    GameObject aliveEnemies;
    [SerializeField] bool invisible = false;
    [SerializeField] EnemySound enemySound;
    public bool isAlive = true;

    public float Health { get => health; set => health = value; }
    public Enemy Enemy { get => enemy; set => enemy = value; }
    public bool Invisible1 { get => invisible; set => invisible = value; }
    public float MaxHealth { get => maxHealth; set => maxHealth = value; }

    // Start is called before the first frame update
    void Start()
    {
        enemySound = GetComponentInChildren<EnemySound>();
        health = enemy.health;
        MaxHealth = health;
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
        Debug.Log(damage + " Damage Taken");
        if(health <= 0){
            GameManager.instance.AddBalance(enemy.value);
            if(tutorialEnemy && secondTutorialEnemy){
                GameManager.instance.CompleteLevel();
            }
            else if(tutorialEnemy && !secondTutorialEnemy){
                GameManager.instance.SpawnRangedEnemy();
                Debug.Log("Tut enemy dead");
            }
            Debug.Log("Killed enemy ");
            Kill();
            
        }
        if(GetComponent<EnemyFollower>().IsStopped){
            enemySound.engineNoise.StopSource();
        }
        else if(!GetComponent<EnemyFollower>().IsStopped && isAlive && !enemySound.engineNoise.src.isPlaying){
            enemySound.engineNoise.PlayLoop();
        }
    }
    private void OnEnable() {
        if(isAlive){
            enemySound.engineNoise.PlayLoop();
        }
    }
    public void Kill(){
        health = 0;
        GetComponent<EnemyFollower>().StopAllParticles();
        isAlive = false;
        this.transform.SetParent(deadEnemies.transform);
        this.transform.position = deadEnemies.transform.position;
        Deactivate();
        
    }
    
    public void Deactivate(){
        body.SetActive(false);
        GetComponent<EnemyFollower>().StopAllParticles();
        enemySound.engineNoise.StopSource();
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
        GetComponent<EnemyFollower>().StopAllParticles();
        body.SetActive(true);
        //engineNoise.PlayLoop();
        enemySound.engineNoise.PlayLoop();
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
            health = MaxHealth;
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
