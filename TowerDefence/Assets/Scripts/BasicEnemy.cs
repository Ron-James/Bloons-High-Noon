using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;



public enum Type{
    basic = 0
}
public class BasicEnemy : MonoBehaviour
{
    GameObject deadEnemies;
    NavMeshAgent navMeshAgent;
    bool isAlive;
    [SerializeField] float health = 1;
    [SerializeField] Enemy enemy;
    [SerializeField] float speed;
    [SerializeField] Vector3 mainTowerPos;
    [SerializeField] LayerMask laneMask;
    [SerializeField] float towerDamage = 1;
    [SerializeField] Lane currentLane;
    [SerializeField] Type type = (Type) 0;


    RaycastHit laneHit;
    bool pulled;



    public Lane CurrentLane { get => currentLane; set => currentLane = value; }
    public Enemy Enemy { get => enemy; set => enemy = value; }

    // Start is called before the first frame update
    void Start()
    {
        pulled = false;
        isAlive = true;
        Physics.Raycast(transform.position, -Vector3.up, out laneHit, 15, laneMask);
        deadEnemies = GameObject.Find("Dead Enemies");

        Physics.IgnoreLayerCollision(9, 10, true);
    }

    // Update is called once per frame
    void Update()
    {
        
        if(Physics.Raycast(transform.position, -Vector3.up, out laneHit, 15, laneMask) && laneHit.collider.gameObject.GetComponentInParent<Lane>() != currentLane){
            currentLane = laneHit.collider.gameObject.GetComponentInParent<Lane>();
            //navMeshAgent.destination = currentLane.EndPoint.position;
        }
    }

     private void OnEnable() {
        isAlive = true;
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.destination = mainTowerPos;
    }
    private void OnDisable() {
        isAlive = false;
    }

    private void Awake() {
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.destination = mainTowerPos;
    }
    
    private void OnTriggerEnter(Collider other) {
        switch(other.tag){
            case "Tower":
                Kill();
                GameManager.instance.DamageTower(towerDamage);
            break;
            case "Island":
                Island island = other.GetComponentInParent<Island>();
                this.transform.SetParent(island.Enemies.transform);
                //Debug.Log("On a new Island");
            break;
            case "Projectile":
                Projectile proj = other.GetComponent<Projectile>();
                DealDamage(proj.Damage);
                proj.Turret.Target = null;
                Destroy(other.gameObject);
            break;
        }
    }

    void Kill(){
        //this.transform.SetParent(deadEnemies.transform);
        //this.gameObject.SetActive(false);
        //transform.position = deadEnemies.transform.position;
        isAlive = false;
        GameManager.instance.AddBalance(enemy.value);
        Destroy(this.gameObject);
    }

    void DealDamage(float amount){
        health -= amount;
        Debug.Log(health + " health");
        if(health <= 0){
            Kill();
        }
    }

    
}
