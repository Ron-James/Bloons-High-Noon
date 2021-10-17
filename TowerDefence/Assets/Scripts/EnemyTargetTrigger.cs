using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyTargetTrigger : MonoBehaviour
{
    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other) {
        switch(other.tag){
            case "Build Plate":
            if(other.gameObject.GetComponentInParent<BuildPlate>().BuildIndex > 0 && GetComponentInParent<EnemyRanged>().Target == null){
                GetComponentInParent<EnemyRanged>().Target = other.transform;
                GetComponentInParent<NavMeshAgent>().isStopped = true;
            }
            break;
        }
    }
}
