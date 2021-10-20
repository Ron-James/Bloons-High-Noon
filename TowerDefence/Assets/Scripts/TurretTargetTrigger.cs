using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretTargetTrigger : MonoBehaviour
{
    [SerializeField] List<Transform> enemiesInRange = new List<Transform>(0);
    GameObject deadEnemies;
    // Start is called before the first frame update
    private void OnTriggerStay(Collider other) {
        
    }
    private void Start() {
        deadEnemies = GameObject.Find("Dead Enemies");
    }
    private void Update() {
        if(GetComponentInParent<TurretAim>().Target == null || GetComponentInParent<TurretAim>().Target.gameObject.GetComponent<EnemyHealth>().Health <= 0 || GetComponentInParent<TurretAim>().Target.parent == deadEnemies.transform){
            RemoveDeadEnemies();
            GetComponentInParent<TurretAim>().Target = FurthestEnemy();
        }
    }
    private void OnTriggerEnter(Collider other) {
        if(!enemiesInRange.Contains(other.transform) && other.tag == "Enemy"){
            AddEnemy(other.transform);
        }
    }
    private void OnTriggerExit(Collider other) {
        if(GetComponentInParent<TurretAim>().Target = other.transform){
            GetComponentInParent<TurretAim>().Target = null;
        }
        if(other.tag == "Enemy"){
            RemoveEnemy(other.transform);
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
                    small = (enemiesInRange[loop].position - transform.position).magnitude;
                }
            }
            return smallest;
        }  
        
    }
    Transform FurthestEnemy(){
        if(FirstEnemy() == null){
            Debug.Log("No enemies in range");
            return null;
        }
        else
        {
            float large = (FirstEnemy().position - transform.position).magnitude;
            Transform largest = FirstEnemy();
            for(int loop = 1; loop < enemiesInRange.Count; loop++){
                if(enemiesInRange[loop] != null){
                    if((enemiesInRange[loop].position - transform.position).magnitude > large){
                        largest = enemiesInRange[loop];
                        large = (enemiesInRange[loop].position - transform.position).magnitude;
                    }
                }
                
            }
            return largest;
        }  
        
    }

    

    void RemoveDeadEnemies(){
        for(int loop = 0; loop < enemiesInRange.Count; loop++){
            if(enemiesInRange[loop] != null){
                if(enemiesInRange[loop].parent == deadEnemies.transform){
                    enemiesInRange[loop] = null;
                }
            }
            
        }
    }

    Transform FirstEnemy(){
        for(int loop = 0; loop < enemiesInRange.Count; loop++){
            if(enemiesInRange[loop] != null){
                return enemiesInRange[loop];
            }
        }
        return null;
    }
    void AddEnemy(Transform enemy){

        if(HasNullEntry()){
            for(int loop = 0; loop < enemiesInRange.Count; loop++){
                if(enemiesInRange[loop] == null){
                    enemiesInRange[loop] = enemy;
                    return;
                }
            }
        }
        else{
            enemiesInRange.Add(enemy);
        }
        
    }
    void RemoveEnemy(Transform enemy){
        if(enemiesInRange.Contains(enemy)){
            for(int loop = 0; loop < enemiesInRange.Count; loop++){
                if(enemiesInRange[loop] == enemy){
                    enemiesInRange[loop] = null;
                    return;
                }
            }
        }
        else
        {
            return;
        }
    }

    bool HasNullEntry(){
        for(int loop = 0; loop < enemiesInRange.Count; loop++){
            if(enemiesInRange[loop] == null){
                return true;
            }
        }
        return false;
    }
}
