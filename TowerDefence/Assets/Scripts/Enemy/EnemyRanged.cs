using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyRanged : MonoBehaviour
{
    Transform tempDestination;
    [SerializeField] Transform target;
    [SerializeField] Transform firePoint;
    [SerializeField] Transform barrel;

    [SerializeField] float rotationSpeed = 25f;
    [SerializeField] float fireRate = 0.5f;
    [SerializeField] float towerFireRate = 1f;
    [SerializeField] float damage = 1;
    [SerializeField] LineRenderer lineRenderer;
    [SerializeField] LayerMask layerMask;
    


    [SerializeField] float fireCount;
    float towerFireCount;
    Quaternion defaultBarrelRotation;

    public Transform Target { get => target; set => target = value; }

    // Start is called before the first frame update
    void Start()
    {
        
        fireCount = 0;
        defaultBarrelRotation = barrel.rotation;
        //GetComponent<SphereCollider>().radius = range;
    }

    // Update is called once per frame
    void Update()
    {
        Aim();
        if(target != null){
            if(target.gameObject.tag == "Build Plate" && target.gameObject.GetComponentInParent<BuildPlate>().BuildIndex > 0){
                fireCount += Time.deltaTime;
                if(fireCount >= fireRate){
                    Debug.Log("Fired enemy");
                    Fire();
                    fireCount = 0;
                }
            }
            else if(target.gameObject.tag == "Tower")
            {
                towerFireCount += Time.deltaTime;
                
                if(towerFireCount >= towerFireRate){
                    //Debug.Log("Fired enemy");
                    Fire();
                    towerFireCount = 0;
                }
            }
            
        }
    }
    void Aim(){
        if(target != null){
            Quaternion targetRot = Quaternion.LookRotation(target.position - barrel.position);
            barrel.rotation = Quaternion.RotateTowards(barrel.rotation, targetRot,  rotationSpeed * Time.deltaTime);
        }
        else{
            barrel.rotation = Quaternion.RotateTowards(barrel.rotation, defaultBarrelRotation,  rotationSpeed * Time.deltaTime);
        }
        
       
    }
    
    void Fire(){
        if(target == null){
            return;
        }
        else{
            RaycastHit hit;
            Vector3 direction = (target.position - transform.position).normalized;
            float range = (target.position - transform.position).magnitude;
            if(target.tag == "Build Plate"){
                if(Physics.Raycast(transform.position, direction, out hit, range, layerMask) && hit.collider.tag == "Build Plate"){
                //deal damage top tower
                Debug.Log(hit.collider.name + "shot this");
                StartCoroutine(ShowProjectileLine(hit.point));
                hit.collider.gameObject.GetComponentInParent<BuildPlate>().TakeDamage(damage);
                if(target.gameObject.GetComponentInParent<BuildPlate>().Health <= 0 ){
                    target = null;
                    //GetComponent<NavMeshAgent>().isStopped = false;
                }
            }
            }
            
            else if(target.tag == "Tower"){
                //Debug.Log("hit main tower");
                
                if(Physics.Raycast(transform.position, direction, out hit, range, layerMask) && hit.collider.tag == "Tower"){
                    StartCoroutine(ShowProjectileLine(hit.point));
                    GameManager.instance.DamageTower(damage);
                }
                
            
            }
        }

        
    }

    IEnumerator ShowProjectileLine(Vector3 point){
        float time = 0;
        lineRenderer.SetPosition(0, firePoint.position);
        lineRenderer.SetPosition(1, point);

        while(true){
            time += Time.deltaTime;
            if(time >= fireRate/5){
                lineRenderer.SetPosition(0, Vector3.zero);
                lineRenderer.SetPosition(1, Vector3.zero);
                break; 
            }
            else{
                yield return null;
            }
        }
        yield break;
    }
    
    
}
