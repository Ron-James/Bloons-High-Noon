using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretProjectile : MonoBehaviour
{
    [SerializeField] Transform barrel;
    [SerializeField] Transform firePoint;
    [SerializeField] float fireRate = 0.75f;
    [SerializeField] int damage = 1;
    [SerializeField] float range = 25f;
    [SerializeField] float projectileSpeed;


    Transform target;
    float fireTime;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        target = GetComponent<TurretAim>().Target;
        DamageTarget();
    }

    public void DamageTarget(){
        if(target != null){
            fireTime += Time.deltaTime;
            RaycastHit hit;
            if(Physics.Raycast(firePoint.position, barrel.transform.forward, out hit, range) && fireTime >= fireRate){
                fireTime = 0;
                if(hit.collider.tag == "Enemy"){
                    hit.collider.gameObject.GetComponent<EnemyHealth>().TakeDamage(damage);
                }
            }
             
        }
        else{
            return;
        }
    }

    void Fire(){
        //do some projectile firing here
    }

}
