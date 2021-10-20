using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretTargetTrigger : MonoBehaviour
{
    [SerializeField] List<Transform> enemiesInRange = new List<Transform>(0);
    // Start is called before the first frame update
    private void OnTriggerStay(Collider other) {
        switch(other.tag){
            case "Enemy":
                if(GetComponentInParent<TurretAim>().Target == null){
                    GetComponentInParent<TurretAim>().Target = other.transform;
                }
            break;
        }
    }
    private void OnTriggerEnter(Collider other) {
        if(!enemiesInRange.Contains(other.transform) && other.tag == "Enemy"){
            enemiesInRange.Add(other.transform);
        }
    }
    private void OnTriggerExit(Collider other) {
        if(GetComponentInParent<TurretAim>().Target = other.transform){
            GetComponentInParent<TurretAim>().Target = null;
        }
        if(enemiesInRange.Contains(other.transform)){
            enemiesInRange.Remove(other.transform);
        }
    }

    Transform ClosestEnemy(){
        if(enemiesInRange.Count == 0){
            Debug.Log("No enemies in range");
            return null;
        }
        else
        {
            float small = (enemiesInRange[0].position - transform.position).magnitude;
            Transform smallest = enemiesInRange[0];
            for(int loop = 1; loop < enemiesInRange.Count; loop++){
                if((enemiesInRange[loop].position - transform.position).magnitude < small){
                    smallest = enemiesInRange[loop];
                }
            }
            return smallest;
        }  
        
    }

    public void RemoveDeadEnemy(Transform enemy){
        if(enemiesInRange.Contains(enemy)){
            enemiesInRange.Remove(enemy);
        }
        else{
            return;
        }
    }
}
