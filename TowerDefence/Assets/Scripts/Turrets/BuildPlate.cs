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
    public Build Build { get => build; set => build = value; }
    public int BuildIndex { get => (int) build; }
    public float Health { get => health; set => health = value; }
    public bool Unlocked { get => unlocked; set => unlocked = value; }

    // Start is called before the first frame update
    void Start()
    {
        DisableHealthIndicator();
        buildIndex = BuildIndex;
        EnableCurrentBuild();
        if(BuildIndex > 0){
            health = towers[BuildIndex-1].health;
        }
    }
    public Tower CurrentTower(){
        if(buildIndex > 0){
            return towers[BuildIndex - 1];
        }
        else{
            return null;
        }
        
    }
    // Update is called once per frame
    void Update()
    {
        
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
        float refund = towers[(int) build - 1].cost * GameManager.instance.SellPercentage;
        GameManager.instance.AddBalance((int) refund);
        build = Build.empty;
        health = 0;
        EnableCurrentBuild();
    }

    public void Repair(){
        if(build == Build.empty){
            Debug.Log("build cannot be repaired");
            return;
        }
        else
        {
            health = maxHealth;
        }
    }

    public bool HasFullHealth(){
        if(health == maxHealth){
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
        if(buildIndex == 0){
            DisableHealthIndicator();
        }
        else{
            healthIndicator.fillAmount = health/maxHealth;
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
            maxHealth = health;
            EnableCurrentBuild();
            UpdateHealthBar();
            
        }
        
    }

    public void DestroyTower(){
        build = Build.empty;
        health = 0;
        EnableCurrentBuild();
        DisableHealthIndicator();

    }

    public void TakeDamage(float damage){
        if(BuildIndex > 0){
            health -= damage;
            if(health <= maxHealth * GameManager.HelthIndicatorThrsh){
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
    
}
