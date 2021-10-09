using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyPathing : MonoBehaviour
{
    [SerializeField] Vector3 towerPosition;
    [SerializeField] bool attacksTowers = false;
    [SerializeField] float floatHeight = 9;

    NavMeshAgent navMeshAgent;
    Vector3 nearestTower;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    private void OnEnable() {
        Physics.IgnoreLayerCollision(9, 10, true);
        navMeshAgent = GetComponent<NavMeshAgent>();
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
        if(nearestTower != NearestTowerPosition() && attacksTowers){
            nearestTower = NearestTowerPosition();
            navMeshAgent.destination = nearestTower;
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


}
