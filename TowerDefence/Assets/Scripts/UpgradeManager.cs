using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]


public class UpgradeManager : MonoBehaviour
{
    public enum Upgrade{
        none = 0,
        upgrade1 = 1,
        upgrade2 = 2,
        upgrade3 = 3
    }
    public enum TurretType{
        repeater = 0,
        expo = 1,
        ice = 2
    }

    [SerializeField] TurretType turretType = TurretType.repeater;
    [SerializeField] Upgrade upgrade = Upgrade.none;

    [Header("General Upgrades")]
    [SerializeField] float damageUpgrade = 1;
    [SerializeField] float fireRateUpgrade = 1;


    [Header("CoolDown Upgrade")]
    [SerializeField] float cooldownUpgrade = 1;
    
    [Header("Freeze AOE Upgrades")]
    [SerializeField] float freezeDurationUpgrade = 1;


    float basicRange;
    TurretTargetTrigger targetTrigger;

    public float DamageUpgrade { get => damageUpgrade; set => damageUpgrade = value; }
    public float FireRateUpgrade { get => fireRateUpgrade; set => fireRateUpgrade = value; }
    
    public float FreezeDurationUpgrade { get => freezeDurationUpgrade; set => freezeDurationUpgrade = value; }
    public float CooldownUpgrade { get => cooldownUpgrade; set => cooldownUpgrade = value; }




    // Start is called before the first frame update
    void Start()
    {
        targetTrigger = GetComponentInChildren<TurretTargetTrigger>();
        basicRange = targetTrigger.GetRange();
        switch((int) turretType){
            case 0:
                
            break;
        } 
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public int UpgradeIndex(){
        return (int) upgrade;
    }
    public void Upgrade1(){
        upgrade = Upgrade.upgrade1;
        switch((int)turretType){
            case 0:
                fireRateUpgrade = GetComponent<TurretProjectile>().FireRateUpgrades[4]; //significantly improved fire rate
                targetTrigger.UpgradeRange(targetTrigger.RangeUpgrades[3] * basicRange);

            break;
            case 1:
                CooldownUpgrade = GetComponent<TurretExpoDamage>().CooldownUpgrades[4]; //significantly improved cooldown time
                targetTrigger.UpgradeRange(targetTrigger.RangeUpgrades[3] * basicRange); // improved range
            break;
            case 2:
                targetTrigger.UpgradeRange(targetTrigger.RangeUpgrades[3] * basicRange); //improved range
                fireRateUpgrade = GetComponent<TurretFreezeAOE>().FreezeDurationUpgrades[3];//Improved freeze duration
                damageUpgrade = GetComponent<TurretFreezeAOE>().DamageUpgrades[1]; // mild Damage Downgrade
            break;
        }
    }
    public void Upgrade2(){
        upgrade = Upgrade.upgrade2;
        switch((int)turretType){
            case 0:
                damageUpgrade = GetComponent<TurretProjectile>().DamageUpgrades[4]; //significantly improved fire rate
                //targetTrigger.UpgradeRange(targetTrigger.RangeUpgrades[3] * basicRange);
                fireRateUpgrade = GetComponent<TurretProjectile>().FireRateUpgrades[1];
            break;
            case 1:
                CooldownUpgrade = GetComponent<TurretExpoDamage>().CooldownUpgrades[0]; //significantly slower cooldown time
                //targetTrigger.UpgradeRange(targetTrigger.RangeUpgrades[3] * basicRange); // improved range
                DamageUpgrade = GetComponent<TurretExpoDamage>().DamageUpgrades[3];
            break;
            case 2:
                freezeDurationUpgrade = GetComponent<TurretFreezeAOE>().FreezeDurationUpgrades[3];
                fireRateUpgrade = GetComponent<TurretFreezeAOE>().FireRateUpgrades[0];
            break;
        }
    }

    public void Upgrade3(){
        upgrade = Upgrade.upgrade3;
        switch((int)turretType){
            case 0:
                damageUpgrade = GetComponent<TurretProjectile>().DamageUpgrades[0]; //significantly improved fire rate
                targetTrigger.UpgradeRange(targetTrigger.RangeUpgrades[4] * basicRange);
                GetComponent<TurretProjectile>().IceShot = true;
            break;
            case 1:
                GetComponent<TurretExpoDamage>().Slow = true;
                GetComponent<TurretExpoDamage>().DamageSecondTarget = true;
                damageUpgrade  = GetComponent<TurretExpoDamage>().DamageUpgrades[0];

                //CooldownUpgrade = GetComponent<TurretExpoDamage>().CooldownUpgrades[4]; //significantly improved cooldown time
                //targetTrigger.UpgradeRange(targetTrigger.RangeUpgrades[3] * basicRange); // improved range
            break;
            case 2:
                targetTrigger.UpgradeRange(targetTrigger.RangeUpgrades[1] * basicRange); //improved range
                //freezeDurationUpgrade = GetComponent<TurretFreezeAOE>().FreezeDurationUpgrades[0];//Improved freeze duration
                damageUpgrade = GetComponent<TurretFreezeAOE>().DamageUpgrades[4]; // mild Damage Downgrade
            break;
        }
    }
    public void ResestUpgrade(){
        Debug.Log("Reset Upgrade");
        damageUpgrade = 1;
        upgrade = Upgrade.none;
        cooldownUpgrade = 1;
        targetTrigger.UpgradeRange(basicRange);
        fireRateUpgrade = 1;
        freezeDurationUpgrade = 1;
        if(GetComponent<TurretExpoDamage>() != null){
            GetComponent<TurretExpoDamage>().Slow = false;
            GetComponent<TurretExpoDamage>().DamageSecondTarget = false;
        }
        else if(GetComponent<TurretProjectile>()!= null){
            GetComponent<TurretProjectile>().IceShot = false;
        }

    }
}

