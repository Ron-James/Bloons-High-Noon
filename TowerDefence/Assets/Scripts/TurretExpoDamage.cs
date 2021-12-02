using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretExpoDamage : MonoBehaviour
{
    [SerializeField] float damageConstant = 0.0000005f;
    [SerializeField] float coolDownTime = 0.2f;

    Transform secondTarget;
    [SerializeField] LineRenderer lineRenderer;
    [Range(0, 1)] [SerializeField] float slowPercentage = 0.5f;


    [Header("Upgrades")]
    [SerializeField] float[] cooldownUpgrades = new float[5];
    [SerializeField] float[] damageUpgrades = new float[5];


    bool coolingDown;

    public Transform SecondTarget { get => secondTarget; set => secondTarget = value; }
    public float[] CooldownUpgrades { get => cooldownUpgrades; set => cooldownUpgrades = value; }
    public float[] DamageUpgrades { get => damageUpgrades; set => damageUpgrades = value; }
    public bool DamageSecondTarget { get => damageSecondTarget; set => damageSecondTarget = value; }
    public bool Slow { get => slow; set => slow = value; }
    public bool CoolingDown { get => coolingDown; set => coolingDown = value; }

    [SerializeField] LineRenderer secondLine;
    [SerializeField] bool damageSecondTarget;
    [SerializeField] bool slow;
    [SerializeField] ParticleSystem flames;
    [SerializeField] Sound flameThrower;
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
        CoolingDown = false;
        damageSecondTarget = false;
        targetTrigger = GetComponentInChildren<TurretTargetTrigger>();
        upgradeManager = GetComponent<UpgradeManager>();
        aimScript = GetComponent<TurretAim>();
    }

    // Update is called once per frame
    void Update()
    {
        if (aimScript.Target != null && !coolingDown && !aimScript.Stunned && damageCo == null)
        {
            damageCo = StartCoroutine(DamageDelay());
        }

    }
    private void OnEnable()
    {
        flames.Stop();
    }
    private void OnDisable()
    {
        StopAllCoroutines();
        flames.Stop();
    }
    IEnumerator CoolDown(float period)
    {
        coolingDown = true;
        float time = 0;
        while (true)
        {
            time += Time.deltaTime;
            if (time >= period)
            {
                coolingDown = false;

                break;
            }
            else
            {
                yield return null;
            }
        }
    }


    IEnumerator DamageDelay()
    {
        yield return new WaitForSeconds(0.001f);
        StartCoroutine(ExponentialDamage());
    }
    IEnumerator ExponentialDamage()
    {
        if (aimScript.Target == null || aimScript.Target.GetComponent<EnemyHealth>() == null || CoolingDown)
        {
            yield break;
        }
        else
        {
            if (upgradeManager.firePoint.GetComponentInChildren<Light>() == null)
            {
                Debug.Log("Can't find light");
            }
            else
            {
                upgradeManager.firePoint.GetComponentInChildren<Light>().enabled = true;
            }
            flameThrower.PlayOnce();
            aimScript.Target.GetComponent<EnemyFollower>().OnFire = true;
            if (slow && !aimScript.Target.GetComponent<EnemyFollower>().IsSlowedExpo)
            {
                aimScript.Target.GetComponent<EnemyFollower>().SlowEnemyIndef(slowPercentage);
            }
            float time = 0;
            if (aimScript.Target.GetComponent<EnemyFollower>() != null && aimScript.Target.GetComponent<EnemyFollower>().OnFire == false)
            {
                aimScript.Target.GetComponent<EnemyFollower>().OnFire = true;
            }
            flames.Play();
            while (true)
            {

                float damage = damageConstant * upgradeManager.DamageUpgrade * Mathf.Exp(time);
                time += Time.deltaTime;
                if (aimScript.Target == null || aimScript.Stunned)
                {
                    StartCoroutine(CoolDown(coolDownTime * (1 / upgradeManager.CooldownUpgrade)));
                    lineRenderer.SetPosition(0, Vector3.zero);
                    lineRenderer.SetPosition(1, Vector3.zero);
                    damageCo = null;
                    flames.Stop();
                    flameThrower.StopSource();
                    if (upgradeManager.firePoint.GetComponentInChildren<Light>() == null)
                    {
                        Debug.Log("Can't find light");
                    }
                    else
                    {
                        upgradeManager.firePoint.GetComponentInChildren<Light>().enabled = false;
                    }
                    //aimScript.Target.GetComponent<EnemyFollower>().IsSlowed = false;
                    break;
                }
                else
                {
                    if (aimScript.Target.GetComponent<EnemyHealth>().Health <= 0)
                    {
                        aimScript.Target = null;
                        StartCoroutine(CoolDown(coolDownTime * (1 / upgradeManager.CooldownUpgrade)));
                        lineRenderer.SetPosition(0, Vector3.zero);
                        lineRenderer.SetPosition(1, Vector3.zero);
                        damageCo = null;
                        flames.Stop();
                        flameThrower.StopSource();
                        if (upgradeManager.firePoint.GetComponentInChildren<Light>() == null)
                        {
                            Debug.Log("Can't find light");
                        }
                        else
                        {
                            upgradeManager.firePoint.GetComponentInChildren<Light>().enabled = false;
                        }
                        //aimScript.Target.GetComponent<EnemyFollower>().IsSlowed = false;
                        break;
                    }
                    lineRenderer.SetPosition(0, upgradeManager.firePoint.position);
                    lineRenderer.SetPosition(1, aimScript.Target.position);
                    aimScript.Target.GetComponent<EnemyHealth>().TakeDamage(damage);

                    yield return null;
                }


            }
        }
    }
}
