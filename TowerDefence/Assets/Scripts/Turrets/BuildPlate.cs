using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public enum Build{
    empty = 0,
    basic = 1,
    expo = 2,
    ice = 3
}

public class BuildPlate : MonoBehaviour
{
    [SerializeField] Build build = Build.empty;
    [SerializeField] Tower [] towers;
    [SerializeField] GameObject [] towerBuilds;
    [SerializeField] Transform buildPosition;
    [SerializeField] GameObject healthBar;
    
    [SerializeField] Image healthIndicator;
    [SerializeField] float health;
    [SerializeField] float maxHealth;
    [SerializeField] int buildIndex;
    [SerializeField] bool unlocked = true;
    [SerializeField] GameObject unlockedPlate;
    [SerializeField] GameObject lockedPlate;
    public Build Build { get => build; set => build = value; }
    public int BuildIndex { get => (int) build; }
    public float Health { get => health; set => health = value; }
    public bool Unlocked { get => unlocked; set => unlocked = value; }
    public int CurrentUpgrade { get => currentUpgrade; set => currentUpgrade = value; }
    public float MaxHealth { get => maxHealth; set => maxHealth = value; }

    [SerializeField] int currentUpgrade = 0;

    // Start is called before the first frame update
    void Start()
    {
        DisableHealthIndicator();
        buildIndex = BuildIndex;
        EnableCurrentBuild();
        if(BuildIndex > 0){
            health = towers[BuildIndex-1].health;
        }
        UpdatePlateLock();
        
    }
    public Tower CurrentTower(){
        
        if(buildIndex <= 0 || buildIndex > towers.Length){
            Debug.Log(buildIndex + " build index bad");
            return null;
        }
        else{
            return towers[buildIndex - 1];
        }
        
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    public void UpdatePlateLock(){
        if(!unlocked){
            lockedPlate.SetActive(true);
            unlockedPlate.SetActive(false);
        }
        else{
            lockedPlate.SetActive(false);
            unlockedPlate.SetActive(true);
        }
    }

    public void UpgradeCurrentBuild(int index){
        if(index > 3 || index < 0 || BuildIndex == 0 || EnabledPrefab().GetComponent<UpgradeManager>().UpgradeIndex() > 0){
            return;
        }
        else{
            switch(index){
                case 0:
                    CurrentUpgrade = 0;
                    EnabledPrefab().GetComponent<UpgradeManager>().ResestUpgrade();
                    
                break;
                case 1:
                    CurrentUpgrade = 1;
                    EnabledPrefab().GetComponent<UpgradeManager>().Upgrade1();
                    GameManager.instance.Purchase(towers[BuildIndex - 1].upgrade1Cost);
                break;
                case 2:
                    CurrentUpgrade = 2;
                    EnabledPrefab().GetComponent<UpgradeManager>().Upgrade2();
                    GameManager.instance.Purchase(towers[BuildIndex - 1].upgrade2Cost);
                break;
                case 3:
                    CurrentUpgrade = 3;
                    EnabledPrefab().GetComponent<UpgradeManager>().Upgrade3();
                    GameManager.instance.Purchase(towers[BuildIndex - 1].upgrade3Cost);
                break;
            }
            
        }
    }
    void EnableCurrentBuild(){
        
        if((int) build == 0){
            for(int loop = 0; loop < towerBuilds.Length; loop++){
                towerBuilds[loop].SetActive(false);
            }
        }
        else{
            for(int loop = 0; loop < towerBuilds.Length; loop++){
                if(((int) build - 1) == loop){
                    towerBuilds[loop].SetActive(true);
                }
                else{
                    towerBuilds[loop].SetActive(false);
                }
                
            }
        }
    }
    public void Demolish(){
        
        float refund = SellValue() * GameManager.instance.SellPercentage;
        GameManager.instance.AddBalance(refund);
        EnabledPrefab().GetComponent<UpgradeManager>().ResestUpgrade();
        
        health = 0;
        MaxHealth = 0;
        
        currentUpgrade = 0;
        build = Build.empty;
        EnableCurrentBuild();

        
        
        
    }

    public void Repair(){
        if(build == Build.empty){
            Debug.Log("build cannot be repaired");
            return;
        }
        else
        {
            health = MaxHealth;
            UpdateHealthBar();
            
        }
    }

    public bool HasFullHealth(){
        if(health == MaxHealth){
            return true;
        }
        else{
            return false;
        }
    }
    public void EnableHealthIndicator(){
        if(buildIndex > 0){
            healthBar.SetActive(true);
            UpdateHealthBar();
        }
    }
    public void DisableHealthIndicator(){
        healthBar.SetActive(false);
    }

    public void UpdateHealthBar(){
        if(buildIndex == 0 || health == MaxHealth){
            DisableHealthIndicator();
        }
        else{
            healthIndicator.fillAmount = health/MaxHealth;
        }
    }
    public void BuildTower(int index){
        
        if(index > towerBuilds.Length || index < 0){
            return;
        }
        else{
            GameManager.instance.Purchase(towers[index - 1].cost);
            build = (Build) index;
            buildIndex = BuildIndex;
            health = towers[index-1].health;
            MaxHealth = health;
            EnableCurrentBuild();
            UpdateHealthBar();
            
        }
        
    }

    public void DestroyTower(){
        health = 0;
        MaxHealth = 0; 
        build = Build.empty;
        EnableCurrentBuild();
        DisableHealthIndicator();

    }

    public void TakeDamage(float damage){
        if(BuildIndex > 0){
            health -= damage;
            if(health <= MaxHealth * GameManager.HelthIndicatorThrsh){
                EnableHealthIndicator();
            }
            UpdateHealthBar();
            if(health <= 0){
                DestroyTower();
                DisableHealthIndicator();
            }
        }
        else{
            return;
        }
    }
    public void EnableRangeIndicator(){
        if(BuildIndex == 0){
            return;
        }
        else{
            EnabledPrefab().GetComponentInChildren<TurretTargetTrigger>().EnableRangeIndicator();
        }
    }
    public void DisableRangeIndicator(){
        if(BuildIndex == 0){
            return;
        }
        else{
            EnabledPrefab().GetComponentInChildren<TurretTargetTrigger>().DisableRangeIndicator();
        }
    }

    public GameObject EnabledPrefab(){
        if(BuildIndex <= 0){
            return null;
        }
        else{
            return towerBuilds[BuildIndex - 1];
        }
        
    }
    public Tower EnabledTower(){
        if(BuildIndex <= 0){
            return null;
        }
        else{
            return towers[BuildIndex - 1];
        }
    }
    public float SellValue(){
        int upgrade = 0;
        if(BuildIndex > 0){
            upgrade = EnabledPrefab().GetComponent<UpgradeManager>().UpgradeIndex();
            switch(upgrade){
                default:
                return EnabledTower().cost;
                case 1:
                    return EnabledTower().cost + EnabledTower().upgrade1Cost;
                case 2:
                    return EnabledTower().cost + EnabledTower().upgrade2Cost;
                case 3: 
                    return EnabledTower().cost + EnabledTower().upgrade3Cost;

            }
        }
        else{
            return 0;
        }
        
    }

    public void UpgradeHealth(){
        int upgrade = 0;
        if(BuildIndex > 0){
            upgrade = EnabledPrefab().GetComponent<UpgradeManager>().UpgradeIndex();
            switch(upgrade){
                default:
                
                break;
                case 1:
                    MaxHealth = EnabledTower().upgrade1Health;
                    health = MaxHealth;
                break;
                case 2:
                    MaxHealth = EnabledTower().upgrade2Health;
                    health = MaxHealth;
                break;
                case 3: 
                    MaxHealth = EnabledTower().upgrade1Health;
                    health = MaxHealth;
                break;    
            }
        }   
        else{
            return;
        }
    }
    
    
}

