using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretProjectile : MonoBehaviour
{
    [SerializeField] Transform barrel;
    [SerializeField] Transform firePoint;
    [SerializeField] float fireRate = 0.75f;
    [SerializeField] float damage = 1;
    [SerializeField] LayerMask enemyMask;


    [SerializeField] LineRenderer lineRenderer;
    [SerializeField] bool iceShot = false;
    Transform target;
    [SerializeField] float fireTime;
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
            float Range = (target.position - firePoint.position).magnitude;
            if(Physics.Raycast(firePoint.position, barrel.transform.forward, out hit, Mathf.Infinity, enemyMask) && fireTime >= fireRate){
                fireTime = 0;
                if(hit.collider.tag == "Enemy"){
                    StartCoroutine(ShowProjectileLine(hit.point));
                    //Debug.Log("Enemy Hit");
                    
                    if(iceShot){
                        hit.collider.gameObject.GetComponent<EnemyHealth>().TakeDamage(damage);
                        hit.collider.gameObject.GetComponent<EnemyPathing>().SlowDownEnemy(GameManager.instance.EnemyIceDuration);
                        if(hit.collider.gameObject.GetComponent<EnemyHealth>().Health <= 0 && GetComponent<TurretAim>().Target.gameObject.GetComponent<EnemyHealth>().Health <=0){
                            GetComponent<TurretAim>().Target = null;
                            //GetComponentInChildren<TurretTargetTrigger>().RemoveDeadEnemy(hit.collider.gameObject.transform);
                            GameManager.instance.AddBalance(hit.collider.gameObject.GetComponent<EnemyHealth>().Enemy.value);
                        }
                    }
                    else{
                        hit.collider.gameObject.GetComponent<EnemyHealth>().TakeDamage(damage);
                        if(hit.collider.gameObject.GetComponent<EnemyHealth>().Health <= 0 && GetComponent<TurretAim>().Target.gameObject.GetComponent<EnemyHealth>().Health <=0){
                            GetComponent<TurretAim>().Target = null;
                            //GetComponentInChildren<TurretTargetTrigger>().RemoveDeadEnemy(hit.collider.gameObject.transform);
                            GameManager.instance.AddBalance(hit.collider.gameObject.GetComponent<EnemyHealth>().Enemy.value);
                        }
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
        lineRenderer.SetPosition(0, Vector3.zero);
        lineRenderer.SetPosition(1, Vector3.zero);

    }

    IEnumerator HideLineDelayed(){
        HideLine();
        yield return new WaitForSeconds(fireRate/2);
    }

    IEnumerator ShowProjectileLine(Vector3 hitPoint){
        lineRenderer.SetPosition(0, firePoint.position);
        lineRenderer.SetPosition(1, hitPoint);

        float time = 0;
        while(true){
            time += Time.deltaTime;
            if(time>= fireRate/4){
                HideLine();
                break;
            }
            else{
                yield return null;
            }
        }
        yield break;
    }


}
