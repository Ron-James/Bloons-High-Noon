using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretTargetTrigger : MonoBehaviour
{
    [SerializeField] List<Transform> enemiesInRange = new List<Transform>(0);
    GameObject areaTrigger;
    [SerializeField] float [] rangeUpgrades = new float [5];
    GameObject deadEnemies;

    public float[] RangeUpgrades { get => rangeUpgrades; set => rangeUpgrades = value; }

    // Start is called before the first frame update
    private void OnTriggerStay(Collider other) {
        
    }
    private void Start() {
        deadEnemies = GameManager.instance.DeadEnemies;
        areaTrigger = this.gameObject;
    }

    IEnumerator TargetDelay(float time){
        yield return new WaitForSeconds(time);
        GetComponentInParent<TurretAim>().Target = FurthestEnemy();
    }
    private void Update() {
        if(GetComponentInParent<TurretAim>().Target == null || GetComponentInParent<TurretAim>().Target.gameObject.GetComponent<EnemyHealth>().Health <= 0 || GetComponentInParent<TurretAim>().Target.parent == deadEnemies.transform){
            RemoveDeadEnemies();
            if(GetComponentInParent<TurretExpoDamage>() != null){
                StartCoroutine(TargetDelay(0.0001f));
            }
            else{
                GetComponentInParent<TurretAim>().Target = FurthestEnemy();
            }
            
        }
        if((GetComponentInParent<TurretExpoDamage>() != null && SecondFurthestEnemy() != null)){
            if(GetComponentInParent<TurretExpoDamage>().SecondTarget == null || GetComponentInParent<TurretExpoDamage>().SecondTarget.gameObject.GetComponent<EnemyHealth>().Health <= 0 || GetComponentInParent<TurretExpoDamage>().SecondTarget.parent == deadEnemies.transform){
                RemoveDeadEnemies();
                GetComponentInParent<TurretExpoDamage>().SecondTarget = SecondFurthestEnemy();
            }
            
            
        }
    }
    private void OnTriggerEnter(Collider other) {
        Debug.Log(other.name);
        if(!enemiesInRange.Contains(other.transform) && other.tag == "Enemy"){
            AddEnemy(other.transform);
        }
    }
    private void OnTriggerExit(Collider other) {
        if(GetComponentInParent<TurretAim>().Target == other.transform && GetComponentInParent<TurretAim>() != null){
            GetComponentInParent<TurretAim>().Target = null;
            
        }
        if(other.tag == "Enemy" && other.gameObject.GetComponent<EnemyFollower>().OnFire && GetComponentInParent<TurretExpoDamage>() != null){
            other.gameObject.GetComponent<EnemyFollower>().OnFire = false;
        }
        if(other.tag == "Enemy"){
            RemoveEnemy(other.transform);
            if(other.gameObject.GetComponent<EnemyFollower>().IsSlowedExpo){
                other.gameObject.GetComponent<EnemyFollower>().IsSlowedExpo = false;
            }
            if(GetComponentInParent<TurretExpoDamage>() != null){
                if(other.transform == GetComponentInParent<TurretExpoDamage>().SecondTarget){
                    GetComponentInParent<TurretExpoDamage>().SecondTarget = null;
                }
            }
        }
    }
    public float GetRange(){
        return transform.localScale.x;
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
    public int EnemiesInRangeCount(){
        return enemiesInRange.Count;
    }

    Transform SecondFurthestEnemy(){
        if(FirstEnemy() == null || EnemiesInRangeCount() <= 1){
            //Debug.Log("No enemies in range");
            return null;
        }
        else
        {
            float large = 0;
            Transform largest = null;
            Transform furthestEnemy = FurthestEnemy();
            if(FirstEnemy() != furthestEnemy){
                large = (FirstEnemy().position - transform.position).magnitude;
                largest = FirstEnemy();
            }
            
            
            for(int loop = 1; loop < enemiesInRange.Count; loop++){
                if(enemiesInRange[loop] != null){
                    if(enemiesInRange[loop] == GetComponentInParent<TurretAim>().Target){
                        continue;
                    }
                    else{
                        if((enemiesInRange[loop].position - transform.position).magnitude > large){
                        largest = enemiesInRange[loop];
                        large = (enemiesInRange[loop].position - transform.position).magnitude;
                    }
                    }
                    
                }
                
            }
            return largest;
        }  
        
    }
    Transform FurthestEnemy(){
        List<Transform> enemies = GetEnemiesInRange();
        if(enemies.Count == 0){
            //Debug.Log("No enemies in range");
            return null;
        }
        else
        {
            float large = enemies[0].GetComponent<EnemyFollower>().CurrentDistance;
            Transform largest = enemies[0];
            for(int loop = 0; loop < enemies.Count; loop++){
                if(enemies[loop] != null){
                    if(enemies[loop].GetComponent<EnemyFollower>().CurrentDistance < large){
                        largest = enemies[loop];
                        large = enemies[loop].GetComponent<EnemyFollower>().CurrentDistance;
                    }
                }
                
                
                
            }
            return largest;
        }  
        
    }

    

    void RemoveDeadEnemies(){
        for(int loop = 0; loop < enemiesInRange.Count; loop++){
            if(enemiesInRange[loop] != null){
                if(!enemiesInRange[loop].GetComponent<EnemyHealth>().isAlive){
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

    public void SwitchToFurthestTarget(){
        if(GetComponentInParent<TurretAim>().Target != FurthestEnemy()){
            GetComponentInParent<TurretAim>().Target = FurthestEnemy();
        }
        else{
            return;
        }
    }
    public List<Transform> GetEnemiesInRange(){
        List<Transform> inRange = new List<Transform>();
        for(int loop = 0; loop < enemiesInRange.Count; loop++){
            if(enemiesInRange[loop] != null){
                inRange.Add(enemiesInRange[loop]);

            }
            else{
                continue;
            }
        }
        return inRange;
    }

    public void FreezeEnemiesInRange(float duration){
        for(int loop = 0; loop < enemiesInRange.Count; loop++){
            if(enemiesInRange[loop] != null){
                if(!enemiesInRange[loop].gameObject.GetComponent<EnemyFollower>().IsFrozen){
                    enemiesInRange[loop].gameObject.GetComponent<EnemyFollower>().FreezeEnemy(duration);
                }
                
            }
            
        }
    }

    public void DamageAllEnemies(float damage){
        for(int loop = 0; loop < enemiesInRange.Count; loop++){
            if(enemiesInRange[loop] != null){
                enemiesInRange[loop].gameObject.GetComponent<EnemyHealth>().TakeDamage(damage);
            }
            
        }
    }
    public void UpgradeRange(float range){
        float y = areaTrigger.transform.localScale.y;
        areaTrigger.transform.localScale = new Vector3(range, y, range);
    }

    public void EnableRangeIndicator(){
        areaTrigger.GetComponent<MeshRenderer>().enabled = true;
    }
    public void DisableRangeIndicator(){
        areaTrigger.GetComponent<MeshRenderer>().enabled = false;
    }
}
