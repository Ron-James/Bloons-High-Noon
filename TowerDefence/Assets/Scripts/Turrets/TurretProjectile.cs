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
    [Range(0, 1)] [SerializeField] float slowPercentage = 0.5f;


    [SerializeField] LineRenderer lineRenderer;
    [SerializeField] bool iceShot = false;
    [SerializeField] bool alwaysSwitchToFurthestTarget = false;
    [SerializeField] Sound shotSound;
    Transform target;
    [SerializeField] float fireTime;

    [Header("Area Damage Things")]
    [SerializeField] float radius = 18f;
    [SerializeField] float blastDamage = 1f;
    [SerializeField] bool doesAreaDamage = false;

    [Header("Upgrade Percentages")]
    [SerializeField] float[] damageUpgrades = new float[5];
    [SerializeField] float[] fireRateUpgrades = new float[5];


    public float FireRate { get => fireRate; set => fireRate = value; }
    public float Damage { get => damage; set => damage = value; }
    public float[] DamageUpgrades { get => damageUpgrades; set => damageUpgrades = value; }
    public float[] FireRateUpgrades { get => fireRateUpgrades; set => fireRateUpgrades = value; }
    public bool IceShot { get => iceShot; set => iceShot = value; }
    public bool AlwaysSwitchToFurthestTarget { get => alwaysSwitchToFurthestTarget; set => alwaysSwitchToFurthestTarget = value; }
    public bool DoesAreaDamage { get => doesAreaDamage; set => doesAreaDamage = value; }

    UpgradeManager upgradeManager;
    TurretAim turretAim;
    Coroutine damageCo;
    // Start is called before the first frame update
    void Start()
    {
        upgradeManager = GetComponent<UpgradeManager>();

    }

    // Update is called once per frame
    void Update()
    {
        turretAim = GetComponent<TurretAim>();
        target = turretAim.Target;
        fireTime += Time.deltaTime;
        if (fireTime >= (fireRate * (1 / upgradeManager.FireRateUpgrade)))
        {
            //Debug.Log(fireTime + "fire Time");
            DamageTarget();
        }

    }
    IEnumerator ShowProjectileDelayed(Vector3 endPoint, float time)
    {
        StartCoroutine(ShowProjectileLine(endPoint));
        yield return new WaitForSeconds(time);

    }
    IEnumerator DamageDelay(float time)
    {
        yield return new WaitForSeconds(time);
        DamageTarget();
        fireTime = 0;
        damageCo = null;
    }
    public void DamageTarget()
    {

        //fireTime += Time.deltaTime;
        if (target != null && target.GetComponentInParent<EnemyHealth>() != null)
        {
            //fireTime += Time.deltaTime;
            fireTime = 0;
            shotSound.PlayOnce();
            if (target.GetComponentInChildren<EnemySound>() != null)
            {
                target.GetComponentInChildren<EnemySound>().ProjectileHitSound();
            }
            else
            {
                Debug.Log("cant find enemy sound");
            }

            //StartCoroutine(ShowProjectileLine(hit.point));
            //Debug.Log("Enemy Hit");
            StartCoroutine(FlashNozzle(fireRate * 0.4f));
            upgradeManager.firePoint.GetComponent<ParticleSystem>().Play();
            if (iceShot)
            {
                target.gameObject.GetComponent<EnemyFollower>().SlowEnemy(slowDuration, slowPercentage);
            }
            if (doesAreaDamage)
            {
                AreaDamage(radius, target);
            }
            target.gameObject.GetComponentInParent<EnemyHealth>().TakeDamage(damage * upgradeManager.DamageUpgrade);
            if (AlwaysSwitchToFurthestTarget)
            {
                GetComponentInChildren<TurretTargetTrigger>().SwitchToFurthestTarget();
            }
            if (target.gameObject.GetComponent<EnemyHealth>().Health <= 0 && turretAim.Target.gameObject.GetComponent<EnemyHealth>().Health <= 0)
            {

                //GetComponentInChildren<TurretTargetTrigger>().RemoveDeadEnemy(hit.collider.gameObject.transform);
                //GameManager.instance.AddBalance(target.gameObject.GetComponent<EnemyHealth>().Enemy.value);
                turretAim.Target = null;
            }

        }
        else
        {
            return;
        }



    }

    void Fire()
    {
        //do some projectile firing here
    }

    void DrawRayLine(Vector3 from, Vector3 to)
    {
        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, from);
        lineRenderer.SetPosition(1, to);
        StartCoroutine(HideLineDelayed());
    }

    void HideLine()
    {
        lineRenderer.SetPosition(0, Vector3.zero);
        lineRenderer.SetPosition(1, Vector3.zero);

    }

    IEnumerator HideLineDelayed()
    {
        HideLine();
        yield return new WaitForSeconds(fireRate / 2);
    }

    IEnumerator ShowProjectileLine(Vector3 hitPoint)
    {
        lineRenderer.SetPosition(0, upgradeManager.firePoint.position);
        lineRenderer.SetPosition(1, hitPoint);

        float time = 0;
        while (true)
        {
            time += Time.deltaTime;
            if (time >= fireRate / 4)
            {
                HideLine();
                break;
            }
            else
            {
                yield return null;
            }
        }
        yield break;
    }

    public void AreaDamage(float raduis, Transform enemy)
    {
        Collider[] enemies = Physics.OverlapSphere(enemy.position, raduis);
        if (enemies.Length == 0)
        {
            return;
        }
        else
        {
            if (enemy.GetComponent<EnemyFollower>() != null)
            {
                enemy.GetComponent<EnemyFollower>().ExplosionEffect();
                for (int loop = 0; loop < enemies.Length; loop++)
                {
                    if (enemies[loop].gameObject.GetComponent<EnemyHealth>() != null && enemies[loop].transform != enemy)
                    {
                        enemies[loop].GetComponent<EnemyHealth>().TakeDamage(blastDamage);
                    }
                    else
                    {
                        continue;
                    }
                }
            }
            else
            {
                return;
            }

        }
    }

        public void TakeFirstShot()
        {
            if (target == null)
            {
                return;
            }
            else
            {

            }
        }
        private void OnDisable()
        {
            StopAllCoroutines();
        }
        IEnumerator FlashNozzle(float duration)
        {
            if (upgradeManager.firePoint.GetComponentInChildren<Light>() == null)
            {
                Debug.Log("Can't find light");
                yield break;
            }
            else
            {
                Light nozzleLight = upgradeManager.firePoint.GetComponentInChildren<Light>();
                float time = 0;
                nozzleLight.enabled = true;
                while (true)
                {
                    time += Time.deltaTime;
                    if (time >= duration)
                    {
                        nozzleLight.enabled = false;
                        break;
                    }
                    else
                    {
                        yield return null;
                    }
                }

            }

        }

    }
