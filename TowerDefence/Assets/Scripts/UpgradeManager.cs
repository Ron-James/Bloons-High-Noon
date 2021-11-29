using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class UpgradeManager : MonoBehaviour
{
    [System.Serializable]
    public class UpgradeModel{
        public GameObject prefab;
        public Transform firePoint;
        public GameObject basePiece;
        public GameObject barrel;
        public Upgrade upgrade;

        public bool IsNull(){
            if(prefab == null || firePoint == null){
                return true;
            }
            else{
                return false;
            }
        }
    }
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

    [Header("Upgrade Models")]
    
    [SerializeField] UpgradeModel [] upgradeModels = new UpgradeModel[4];
    public Transform firePoint;
    public GameObject basePiece;
    public GameObject barrel;



    float basicRange;
    TurretTargetTrigger targetTrigger;

    public float DamageUpgrade { get => damageUpgrade; set => damageUpgrade = value; }
    public float FireRateUpgrade { get => fireRateUpgrade; set => fireRateUpgrade = value; }
    
    public float FreezeDurationUpgrade { get => freezeDurationUpgrade; set => freezeDurationUpgrade = value; }
    public float CooldownUpgrade { get => cooldownUpgrade; set => cooldownUpgrade = value; }
    public UpgradeModel[] UpgradeModels { get => upgradeModels; set => upgradeModels = value; }




    // Start is called before the first frame update
    void Start()
    {
        //UpdateUpgradeModel(0);
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

    public void UpdateUpgradeModel(int upgrade){
        if(upgrade >= upgradeModels.Length || upgrade < 0 || upgradeModels[upgrade].IsNull()){
            return;
        }
        else{
            for(int loop = 0; loop < upgradeModels.Length; loop++){
                if(loop == upgrade){
                    upgradeModels[loop].prefab.SetActive(true);
                    firePoint = upgradeModels[loop].firePoint;
                    basePiece = upgradeModels[loop].basePiece;
                    barrel = upgradeModels[loop].barrel;

                }
                else{
                    upgradeModels[loop].prefab.SetActive(false);
                }
            }
        }
        
    }
    public void Upgrade1(){
        upgrade = Upgrade.upgrade1;
        UpdateUpgradeModel((int) upgrade);
        switch((int)turretType){
            case 0:
                fireRateUpgrade = GetComponent<TurretProjectile>().FireRateUpgrades[4]; //significantly improved fire rate
                targetTrigger.UpgradeRange(targetTrigger.RangeUpgrades[3] * basicRange);
                damageUpgrade = GetComponent<TurretProjectile>().DamageUpgrades[1];

            break;
            case 1:
                CooldownUpgrade = GetComponent<TurretExpoDamage>().CooldownUpgrades[4]; //significantly improved cooldown time
                targetTrigger.UpgradeRange(targetTrigger.RangeUpgrades[3] * basicRange); // improved range
            break;
            case 2:
                targetTrigger.UpgradeRange(targetTrigger.RangeUpgrades[4] * basicRange); //improved range
                fireRateUpgrade = GetComponent<TurretFreezeAOE>().FreezeDurationUpgrades[3];//Improved freeze duration
                //damageUpgrade = GetComponent<TurretFreezeAOE>().DamageUpgrades[1]; // mild Damage Downgrade
            break;
        }
    }
    public void Upgrade2(){
        upgrade = Upgrade.upgrade2;
        UpdateUpgradeModel((int) upgrade);
        switch((int)turretType){
            case 0:
                damageUpgrade = GetComponent<TurretProjectile>().DamageUpgrades[4]; //significantly improved fire rate
                //targetTrigger.UpgradeRange(targetTrigger.RangeUpgrades[3] * basicRange);
                fireRateUpgrade = GetComponent<TurretProjectile>().FireRateUpgrades[0];
                targetTrigger.UpgradeRange(targetTrigger.RangeUpgrades[4] * basicRange);
            break;
            case 1:
                CooldownUpgrade = GetComponent<TurretExpoDamage>().CooldownUpgrades[0]; //significantly slower cooldown time
                //targetTrigger.UpgradeRange(targetTrigger.RangeUpgrades[3] * basicRange); // improved range
                DamageUpgrade = GetComponent<TurretExpoDamage>().DamageUpgrades[3];
            break;
            case 2:
                freezeDurationUpgrade = GetComponent<TurretFreezeAOE>().FreezeDurationUpgrades[4];
                //fireRateUpgrade = GetComponent<TurretFreezeAOE>().FireRateUpgrades[3];
                //targetTrigger.UpgradeRange(targetTrigger.RangeUpgrades[3] * basicRange); //improved range
                damageUpgrade = GetComponent<TurretFreezeAOE>().DamageUpgrades[0]; // sign Damage Downgrade
            break;
        }
    }

    public void Upgrade3(){
        upgrade = Upgrade.upgrade3;
        UpdateUpgradeModel((int) upgrade);
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
                fireRateUpgrade = GetComponent<TurretFreezeAOE>().FireRateUpgrades[0];
                //freezeDurationUpgrade = GetComponent<TurretFreezeAOE>().FreezeDurationUpgrades[1];
                //freezeDurationUpgrade = GetComponent<TurretFreezeAOE>().FreezeDurationUpgrades[0];//Improved freeze duration
                damageUpgrade = GetComponent<TurretFreezeAOE>().DamageUpgrades[4]; // mild Damage Downgrade
                GetComponent<TurretFreezeAOE>().StunSlow = true;
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
        UpdateUpgradeModel((int) upgrade);

    }
}

