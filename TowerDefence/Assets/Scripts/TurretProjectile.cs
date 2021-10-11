using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretProjectile : MonoBehaviour
{
    [SerializeField] Transform barrel;
    [SerializeField] Transform firePoint;
    [SerializeField] float fireRate = 0.75f;
    [SerializeField] float damage = 1;
    [SerializeField] float range = 25f;
    [SerializeField] float projectileSpeed;


    [SerializeField] LineRenderer lineRenderer;

    [SerializeField] bool iceShot = false;
    [SerializeField] float iceDuration;
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
        if(target != null && !GetComponent<TurretAim>().Stunned){
            fireTime += Time.deltaTime;
            RaycastHit hit;
            if(Physics.Raycast(firePoint.position, barrel.transform.forward, out hit, range) && fireTime >= fireRate){
                fireTime = 0;
                if(hit.collider.tag == "Enemy"){
                    DrawRayLine(firePoint.position, hit.transform.position);
                    Debug.Log("Enemy Hit");
                    
                    if(iceShot){
                        hit.collider.gameObject.GetComponent<EnemyHealth>().TakeDamage(damage);
                        hit.collider.gameObject.GetComponent<EnemyPathing>().SlowDownEnemy(iceDuration);
                    }
                    else{
                        hit.collider.gameObject.GetComponent<EnemyHealth>().TakeDamage(damage);
                    }
                    if(hit.collider.gameObject.GetComponent<EnemyHealth>().Health <= 0){
                        GetComponent<TurretAim>().Target = null;
                        GameManager.instance.AddBalance(hit.collider.gameObject.GetComponent<EnemyHealth>().Enemy.value);
                    }
                    
                }
            }
             
        }
        else{
            fireTime = 0;
            return;
        }
    }

    void Fire(){
        //do some projectile firing here
    }

    void DrawRayLine(Vector3 from, Vector3 to){
        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, from);
        lineRenderer.SetPosition(1, to);
        StartCoroutine(HideLineDelayed());
    }

    void HideLine(){
        lineRenderer.positionCount = 0;
    }

    IEnumerator HideLineDelayed(){
        HideLine();
        yield return new WaitForSeconds(fireRate/2);
    }


}
