using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyPathing : MonoBehaviour
{
    [SerializeField] Vector3 towerPosition;
    [SerializeField] bool attacksTowers = false;
    [SerializeField] float floatHeight = 9;
    [SerializeField] float regularSpeed = 8;
    [SerializeField] float slowedSpeed = 4;
    GameObject Tower;
    NavMeshAgent navMeshAgent;
    Vector3 nearestTower;
    bool slowed;
    Coroutine slowCo;

    public bool Slowed { get => slowed; set => slowed = value; }
    public Vector3 TowerPosition { get => towerPosition; set => towerPosition = value; }

    // Start is called before the first frame update
    void Start()
    {
        Tower = GameObject.FindGameObjectWithTag("Tower");
        navMeshAgent = GetComponent<NavMeshAgent>();
    }
    private void OnEnable() {
        navMeshAgent = GetComponent<NavMeshAgent>();
        Physics.IgnoreLayerCollision(9, 10, true);
        
        if(!attacksTowers){
            navMeshAgent.destination = towerPosition;
        }
        else{
            navMeshAgent.destination = nearestTower;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
        
    }
    void ControlSpeed(){
        if(slowed){
            navMeshAgent.speed = slowedSpeed;
        }
        else{
            navMeshAgent.speed = regularSpeed;
        }
    }
    

    public void SlowDownEnemy(float time){
        if(slowCo != null){
            StopCoroutine(slowCo);
            slowCo = null;
            slowCo = StartCoroutine(SlowEnemy(time));
        }
        else{
            slowCo = StartCoroutine(SlowEnemy(time));
        }
        
    }
    
    IEnumerator SlowEnemy(float duration){
        Rigidbody rigidbody = GetComponent<Rigidbody>();
        float time = 0;
        slowed = true;
        navMeshAgent.speed = slowedSpeed;
        rigidbody.velocity = slowedSpeed * rigidbody.velocity.normalized; 
        while(true){
            time += Time.deltaTime;

            if(time >= duration){
                slowed = false;
                navMeshAgent.speed = regularSpeed;
                slowCo = null;
                break;
            }
            else{
                yield return null;
            }
        }
    } 


}
