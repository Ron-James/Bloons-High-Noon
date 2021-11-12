using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretExpoDamage : MonoBehaviour
{
    [SerializeField] float damageConstant = 0.0000005f;
    [SerializeField] float coolDownTime = 0.2f;
    [SerializeField] Transform firePoint;
    [SerializeField] Transform secondTarget;
    [SerializeField] LineRenderer lineRenderer;
    [Range(0, 1)] [SerializeField] float slowPercentage = 0.5f;


    [Header("Upgrades")]
    [SerializeField] float [] cooldownUpgrades = new float [5];
    [SerializeField] float [] damageUpgrades = new float [5];


    bool coolingDown;

    public Transform SecondTarget { get => secondTarget; set => secondTarget = value; }
    public float[] CooldownUpgrades { get => cooldownUpgrades; set => cooldownUpgrades = value; }
    public float[] DamageUpgrades { get => damageUpgrades; set => damageUpgrades = value; }
    public bool DamageSecondTarget { get => damageSecondTarget; set => damageSecondTarget = value; }
    public bool Slow { get => slow; set => slow = value; }

    [SerializeField] LineRenderer secondLine;
    [SerializeField] bool damageSecondTarget;
    [SerializeField] bool slow;
    TurretTargetTrigger targetTrigger;
    UpgradeManager upgradeManager;
    TurretAim aimScript;
    Coroutine damageCo;
    Coroutine secondDamageCo;
    // Start is called before the first frame update
    void Start()
    {
        damageCo = null;
        secondDamageCo = null;
        coolingDown = false;
        damageSecondTarget = false;
        targetTrigger = GetComponentInChildren<TurretTargetTrigger>();
        upgradeManager = GetComponent<UpgradeManager>();
        aimScript = GetComponent<TurretAim>();
    }

    // Update is called once per frame
    void Update()
    {
        if(aimScript.Target != null && !coolingDown && !aimScript.Stunned && damageCo == null){
            damageCo = StartCoroutine(ExponentialDamage());
        }
        
        if(damageSecondTarget && secondTarget != null && !coolingDown && !aimScript.Stunned && secondDamageCo == null){
            StartCoroutine(ExponentialDamageSecond());
        }
        
    }
    IEnumerator CoolDown(float period){
        coolingDown = true;
        float time = 0;
        while(true){
            time += Time.deltaTime;
            if(time >= period){
                coolingDown = false;
                break;
            }
            else{
                yield return null;
            }
        }
    }
    
    IEnumerator ExponentialDamageSecond(){
        if(secondTarget == null || secondTarget.GetComponent<EnemyHealth>() == null || coolingDown || secondTarget.GetComponent<EnemyHealth>().Health <= 0){
            yield break;
        }
        else{
            if(slow && !secondTarget.GetComponent<EnemyFollower>().IsSlowedExpo){
                secondTarget.GetComponent<EnemyFollower>().SlowEnemyIndef(slowPercentage);
            }
            float time = 0;
            while(true){
                float damage = damageConstant * upgradeManager.DamageUpgrade * Mathf.Exp(time);
                time += Time.deltaTime;
                if(secondTarget == null || aimScript.Stunned){
                    StartCoroutine(CoolDown(coolDownTime * (1/upgradeManager.CooldownUpgrade)));
                    secondLine.SetPosition(0, Vector3.zero);
                    secondLine.SetPosition(1, Vector3.zero);
                    secondDamageCo = null;
                    //secondTarget.GetComponent<EnemyFollower>().IsSlowed = false;
                    break;
                }
                else{
                    secondLine.SetPosition(0, firePoint.position);
                    secondLine.SetPosition(1, secondTarget.position);
                    secondTarget.GetComponent<EnemyHealth>().TakeDamage(damage);
                    if(secondTarget.GetComponent<EnemyHealth>().Health <= 0){
                        secondTarget = null;
                    }
                    yield return null;
                }


            }
        }
    }
    IEnumerator ExponentialDamage(){
        if(aimScript.Target == null || aimScript.Target.GetComponent<EnemyHealth>() == null || coolingDown){
            yield break;
        }
        else{
            if(slow && !aimScript.Target.GetComponent<EnemyFollower>().IsSlowedExpo){
                aimScript.Target.GetComponent<EnemyFollower>().SlowEnemyIndef(slowPercentage);
            }
            float time = 0;
            while(true){
                float damage = damageConstant * upgradeManager.DamageUpgrade * Mathf.Exp(time);
                time += Time.deltaTime;
                if(aimScript.Target == null || aimScript.Stunned){
                    StartCoroutine(CoolDown(coolDownTime * (1/upgradeManager.CooldownUpgrade)));
                    lineRenderer.SetPosition(0, Vector3.zero);
                    lineRenderer.SetPosition(1, Vector3.zero);
                    damageCo = null;
                    //aimScript.Target.GetComponent<EnemyFollower>().IsSlowed = false;
                    break;
                }
                else{
                    lineRenderer.SetPosition(0, firePoint.position);
                    lineRenderer.SetPosition(1, aimScript.Target.position);
                    aimScript.Target.GetComponent<EnemyHealth>().TakeDamage(damage);
                    if(aimScript.Target.GetComponent<EnemyHealth>().Health <= 0){
                        aimScript.Target = null;
                    }
                    yield return null;
                }


            }
        }
    }
}
