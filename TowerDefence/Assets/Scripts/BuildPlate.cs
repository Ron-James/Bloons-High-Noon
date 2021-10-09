using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Build{
    empty = 0,
    basic = 1
}

public class BuildPlate : MonoBehaviour
{
    [SerializeField] Build build = Build.empty;
    [SerializeField] Tower [] towers;
    [SerializeField] Transform buildPosition;
    List<Transform> builds = new List<Transform>();

    int health;
    int maxHealth;
    public Build Build { get => build; set => build = value; }
    public int BuildIndex { get => (int) build; }

    // Start is called before the first frame update
    void Start()
    {
        SetUpTowers();
        EnableCurrentBuild();
        if(BuildIndex > 0){
            health = towers[BuildIndex-1].health;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void SetUpTowers(){
        builds.Clear();
        for(int loop = 0; loop < towers.Length; loop++){
            GameObject go = Instantiate(towers[loop].tower, buildPosition.position, Quaternion.identity, this.transform);
            builds.Add(go.transform);
            go.SetActive(false);
        }
    }

    void EnableCurrentBuild(){
        
        if((int) build == 0){
            for(int loop = 0; loop < builds.Count; loop++){
                builds[loop].gameObject.SetActive(false);
            }
        }
        else{
            for(int loop = 0; loop < builds.Count; loop++){
                if(((int) build - 1) == loop){
                    builds[loop].gameObject.SetActive(true);
                }
                else{
                    builds[loop].gameObject.SetActive(false);
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
        
        if(index > builds.Count || index < 0){
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

    public void TakeDamage(int damage){
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
