using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyRanged : MonoBehaviour
{
    Transform tempDestination;
    [SerializeField] Transform target;
    [SerializeField] float rotationSpeed = 25f;
    [SerializeField] float range = 25f;
    [SerializeField] float fireRate = 0.5f;
    [SerializeField] float damage = 1;

    float fireCount;
    // Start is called before the first frame update
    void Start()
    {
        fireCount = 0;
        //GetComponent<SphereCollider>().radius = range;
    }

    // Update is called once per frame
    void Update()
    {
        if(target != null && target.gameObject.GetComponentInParent<BuildPlate>().Health > 0){
            fireCount += Time.deltaTime;
            if(fireCount >= fireRate){
                Fire();
                fireCount = 0;
            }
        }
    }
    void Aim(){
        Quaternion targetRot = Quaternion.LookRotation(target.position - transform.position);
        Vector3 angles = targetRot.eulerAngles;
        angles.x = 0;
        angles.z = 0;
        targetRot = Quaternion.Euler(angles);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRot,  rotationSpeed * Time.deltaTime);
    }
    
    void Fire(){
        if(target == null){
            return;
        }
        else{
            RaycastHit hit;
            Vector3 direction = (target.position - transform.position).normalized; 
            Physics.Raycast(transform.position, direction, out hit, range);
            if(hit.collider.tag == "Tower"){
                //deal damage top tower
                hit.collider.gameObject.GetComponentInParent<BuildPlate>().TakeDamage(damage);
                if(target.gameObject.GetComponentInParent<BuildPlate>().Health <= 0 ){
                    target = null;
                    GetComponent<NavMeshAgent>().destination = tempDestination.position;
                }
            }
        }
        
    }
    private void OnTriggerEnter(Collider other) {
        switch(other.tag){
            case "Tower":
                if(target == null){
                    target = other.transform;
                    tempDestination.position = GetComponent<NavMeshAgent>().destination;
                    GetComponent<NavMeshAgent>().destination = transform.position;
                }
            break;
        }
    }
}
