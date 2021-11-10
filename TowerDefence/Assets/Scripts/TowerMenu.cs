using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerMenu : MonoBehaviour
{
    Tower currentTower;
    [SerializeField] GameObject infoHolder;
    [SerializeField] Text towerName;
    [SerializeField] Text towerDescription;
    [SerializeField] Text upgrade1Desc;
    [SerializeField] Text upgrade2Desc;
    [SerializeField] Text upgrade3Desc;
    [SerializeField] Image towerPic;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void UpdateInfo(Tower t){
        currentTower = t;
        towerName.text = currentTower.towerName;
        towerDescription.text = currentTower.description;
        upgrade1Desc.text = currentTower.upgrade1Desc;
        upgrade2Desc.text = currentTower.upgrade2Desc;
        upgrade3Desc.text = currentTower.upgrade3Desc;


    }
    public void OpenTowerMenu(){
        if(GetComponentInParent<BuildMenu>().CurrentPlate != null){
            infoHolder.SetActive(true);
            UpdateInfo(GetComponentInParent<BuildMenu>().CurrentPlate.CurrentTower());
        }
        else{
            return;
        }
        
    }
    
}
