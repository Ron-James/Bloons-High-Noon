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

    NavMeshAgent navMeshAgent;
    Vector3 nearestTower;
    bool slowed;
    Coroutine slowCo;

    public bool Slowed { get => slowed; set => slowed = value; }

    // Start is called before the first frame update
    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
    }
    private void OnEnable() {
        navMeshAgent = GetComponent<NavMeshAgent>();
        Physics.IgnoreLayerCollision(9, 10, true);
        nearestTower = NearestTowerPosition();
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
        ControlSpeed();
        if(nearestTower != NearestTowerPosition() && attacksTowers){
            nearestTower = NearestTowerPosition();
            navMeshAgent.destination = nearestTower;
        }
    }
    void ControlSpeed(){
        if(slowed){
            navMeshAgent.speed = slowedSpeed;
        }
        else{
            navMeshAgent.speed = regularSpeed;
        }
    }
    Vector3 NearestTowerPosition(){
        if(GameManager.instance.NearestTower() != null){
            BuildPlate buildPlate = GameManager.instance.NearestTower();
            Vector3 towerPos = buildPlate.gameObject.transform.position;
            towerPos.y = floatHeight;
            return towerPos;
        }
        else{
            return towerPosition;
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
        float time = 0;
        slowed = true;

        while(true){
            time += Time.deltaTime;

            if(time >= duration){
                slowed = false;
                slowCo = null;
                break;
            }
            else{
                yield return null;
            }
        }
    } 


}
