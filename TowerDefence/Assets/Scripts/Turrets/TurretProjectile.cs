using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretProjectile : MonoBehaviour
{
    [SerializeField] Transform barrel;
    [SerializeField] float fireRate = 0.75f;
    [SerializeField] float damage = 1;
    [SerializeField] LayerMask enemyMask;
    [SerializeField] float slowDuration = 3f;
    [Range(0,1)][SerializeField] float slowPercentage = 0.5f;


    [SerializeField] LineRenderer lineRenderer;
    [SerializeField] bool iceShot = false;
    [SerializeField] bool alwaysSwitchToFurthestTarget = false;
    [SerializeField] Sound shotSound;
    Transform target;
    [SerializeField] float fireTime;

    [Header("Upgrade Percentages")]
    [SerializeField] float [] damageUpgrades = new float [5];
    [SerializeField] float [] fireRateUpgrades = new float [5];


    public float FireRate { get => fireRate; set => fireRate = value; }
    public float Damage { get => damage; set => damage = value; }
    public float[] DamageUpgrades { get => damageUpgrades; set => damageUpgrades = value; }
    public float[] FireRateUpgrades { get => fireRateUpgrades; set => fireRateUpgrades = value; }
    public bool IceShot { get => iceShot; set => iceShot = value; }

    UpgradeManager upgradeManager;

    // Start is called before the first frame update
    void Start()
    {
        upgradeManager = GetComponent<UpgradeManager>();
        
    }

    // Update is called once per frame
    void Update()
    {
        target = GetComponent<TurretAim>().Target;
        DamageTarget();
        
    }
    IEnumerator ShowProjectileDelayed(Vector3 endPoint, float time){
        StartCoroutine(ShowProjectileLine(endPoint));
        yield return new WaitForSeconds(time);
    }
    public void DamageTarget(){
        fireTime += Time.deltaTime;
        if(target != null && !GetComponent<TurretAim>().Stunned && target.GetComponentInParent<EnemyHealth>() != null){
            //fireTime += Time.deltaTime;
            if(fireTime >= (fireRate * (1/upgradeManager.FireRateUpgrade))){
                fireTime = 0;
                
                shotSound.PlayOnce();
                
                //StartCoroutine(ShowProjectileLine(hit.point));
                //Debug.Log("Enemy Hit");
                upgradeManager.firePoint.GetComponent<ParticleSystem>().Play();
                if(iceShot){
                    target.gameObject.GetComponent<EnemyFollower>().SlowEnemy(slowDuration, slowPercentage);
                }
                target.gameObject.GetComponentInParent<EnemyHealth>().TakeDamage(damage * upgradeManager.DamageUpgrade);
                if(alwaysSwitchToFurthestTarget){
                    GetComponentInChildren<TurretTargetTrigger>().SwitchToFurthestTarget();
                }
                if(target.gameObject.GetComponent<EnemyHealth>().Health <= 0 && GetComponent<TurretAim>().Target.gameObject.GetComponent<EnemyHealth>().Health <=0){
                    
                    //GetComponentInChildren<TurretTargetTrigger>().RemoveDeadEnemy(hit.collider.gameObject.transform);
                    //GameManager.instance.AddBalance(target.gameObject.GetComponent<EnemyHealth>().Enemy.value);
                    GetComponent<TurretAim>().Target = null;
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
        lineRenderer.SetPosition(0, upgradeManager.firePoint.position);
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

    

    public void TakeFirstShot(){
        if(target == null){
            return;
        }
        else{
            
        }
    }


}
