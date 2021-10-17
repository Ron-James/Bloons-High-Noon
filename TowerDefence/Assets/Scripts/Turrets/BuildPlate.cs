using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    [SerializeField] float health;
    float maxHealth;
    public Build Build { get => build; set => build = value; }
    public int BuildIndex { get => (int) build; }
    public float Health { get => health; set => health = value; }

    // Start is called before the first frame update
    void Start()
    {
        EnableCurrentBuild();
        if(BuildIndex > 0){
            health = towers[BuildIndex-1].health;
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

    public void BuildTower(int index){
        
        if(index > towerBuilds.Length || index < 0){
            return;
        }
        else{
            GameManager.instance.Purchase(towers[index - 1].cost);
            build = (Build) index;
            health = towers[index-1].health;
            maxHealth = health;
            EnableCurrentBuild();
        }
        
    }

    public void DestroyTower(){
        build = Build.empty;
        health = 0;
        EnableCurrentBuild();

    }

    public void TakeDamage(float damage){
        if(BuildIndex > 0){
            health -= damage;
            if(health <= 0){
                DestroyTower();
            }
        }
        else{
            return;
        }
    }
    
}
