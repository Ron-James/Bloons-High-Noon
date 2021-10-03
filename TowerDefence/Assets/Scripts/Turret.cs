using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    [SerializeField] Transform barrel;
    [SerializeField] Transform target;
    [SerializeField] int range = 2;
    [SerializeField] float RotationSpeed = 100f;
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] Transform firePoint;
    [SerializeField] float fireRate = 3f;
    [SerializeField] Lane lane;
    [SerializeField] Island island;
    List<Transform> rangeOfEnemies = new List<Transform>();
    Quaternion defaultRot;
    float time;

    public Transform Target { get => target; set => target = value; }

    // Start is called before the first frame update
    void Start()
    {
        time = 0;
        defaultRot = barrel.transform.rotation;
        
    }

    void UpdateEnemiesInRange(){
        int effRange;

        if(range + island.Index > lane.Length){
            effRange = lane.Length - island.Index;
        }
        else{
            effRange = range;
        }
        rangeOfEnemies.Clear();
        for(int loop = 0; loop <= effRange; loop++){
            Island i = lane.GetIslandAt(island.Index + loop);
            Transform [] enemies = i.Enemies.GetComponentsInChildren<Transform>();
            for(int e = 0; e < enemies.Length; e++){
                if(!rangeOfEnemies.Contains(enemies[e])){
                    rangeOfEnemies.Add(enemies[e]);
                }
                

            }
            
        }
    }
    // Update is called once per frame
    void LateUpdate()
    {
        Aim();
        
        RotateToDefault();
    }
    private void Update() {
        if(Input.GetKeyDown(KeyCode.T)){
            Fire();
        }
        if(target != null){
            time += Time.deltaTime;
            if(time >= fireRate){
                Fire();
                time = 0;
            }

            
        }
        if(Input.GetKeyDown(KeyCode.P)){
            Fire();
        }

        
    }
    void Aim(){

        if(target != null){
            //Turning
            //float targetPlaneAngle = vector3AngleOnPlane(target.position, transform.position, -transform.up, transform.forward);
            //Vector3 newRot = new Vector3(0, targetPlaneAngle, 0);
            //transform.Rotate(newRot, Space.Self);

            //up/down
            Quaternion targetRot = Quaternion.LookRotation(target.position - barrel.transform.position);
            barrel.transform.rotation = Quaternion.RotateTowards(barrel.transform.rotation, targetRot, RotationSpeed * Time.deltaTime);
        }
        else{
            barrel.transform.rotation = Quaternion.RotateTowards(barrel.transform.rotation, defaultRot, RotationSpeed);
            return;
        }
        
        
    }
    
    void RotateToDefault(){
        if(barrel.transform.rotation != defaultRot && target == null){
            barrel.transform.rotation = Quaternion.RotateTowards(barrel.transform.rotation, defaultRot, RotationSpeed);
        }
    }

    float vector3AngleOnPlane(Vector3 from, Vector3 to, Vector3 planeNormal, Vector3 toZeroAngle){
        Vector3 projected = Vector3.ProjectOnPlane(from - to, planeNormal);
        float projectedAngle = Vector3.SignedAngle(projected, toZeroAngle, planeNormal);

        return projectedAngle;

    }
    private void OnTriggerStay(Collider other) {
        switch(other.tag){
            case "Enemy":
                target = other.transform;
            break;
        }
    }

    private void OnTriggerExit(Collider other) {
        switch(other.tag){
            case "Enemy":
                target = null;
            break;
        }
    }

    void SortClosestEnemy(){

        if(rangeOfEnemies.Count < 1){
            return;
        }
        else{
        //float closest = (enemiesInRange[0].transform.position - transform.position).magnitude;
            for(int loop = 0; loop < rangeOfEnemies.Count - 1; loop++){
                if((rangeOfEnemies[loop].position - transform.position).magnitude > (rangeOfEnemies[loop + 1].position - transform.position).magnitude){
                    Transform temp = rangeOfEnemies[loop + 1];
                    rangeOfEnemies[loop + 1] = rangeOfEnemies[loop];
                    rangeOfEnemies[loop] = temp;
                }
            }
            
        }
    }
    

    void Fire(){
        GameObject proj = Instantiate(bulletPrefab, firePoint.position, barrel.rotation, this.transform);
        proj.GetComponent<Projectile>().Turret = GetComponent<Turret>();
    }

    
    
}
