using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretFreezeAOE : MonoBehaviour
{
    [SerializeField] float fireRate = 0.9f;
    [SerializeField] float freezeDuration = 0.45f;
    [SerializeField] float damage = 0.4f;

    [Header("Upgrades")]
    [SerializeField] float [] fireRateUpgrades = new float [5];
    [SerializeField] float [] damageUpgrades = new float [5];
    [SerializeField] float [] freezeDurationUpgrades = new float [5];
    
    float fireCount;

    public float[] FireRateUpgrades { get => fireRateUpgrades; set => fireRateUpgrades = value; }
    public float[] DamageUpgrades { get => damageUpgrades; set => damageUpgrades = value; }
    public float[] FreezeDurationUpgrades { get => freezeDurationUpgrades; set => freezeDurationUpgrades = value; }

    TurretTargetTrigger targetTrigger;
    UpgradeManager upgradeManager;
    // Start is called before the first frame update
    void Start()
    {
        upgradeManager = GetComponent<UpgradeManager>();
        targetTrigger = GetComponentInChildren<TurretTargetTrigger>();
        fireCount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        fireCount += Time.deltaTime; 
        if(targetTrigger.EnemiesInRangeCount() > 0 && fireCount >= (fireRate * (1/upgradeManager.FireRateUpgrade))){
            targetTrigger.FreezeEnemiesInRange(freezeDuration * upgradeManager.FreezeDurationUpgrade);
            fireCount = 0;
            targetTrigger.DamageAllEnemies(damage * upgradeManager.DamageUpgrade);
        }
    }

    
}
