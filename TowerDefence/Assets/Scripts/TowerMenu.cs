using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerMenu : MonoBehaviour
{
    Tower currentTower;
    [SerializeField] GameObject infoHolder;
    [SerializeField] Text towerName;
    [SerializeField] Button repairButton;
    [SerializeField] Text towerDescription;
    [SerializeField] Text upgrade1Desc;
    [SerializeField] Text upgrade2Desc;
    [SerializeField] Text upgrade3Desc;
    [SerializeField] Text sellPrice;
    [SerializeField] Image towerPic;
    [SerializeField] BuildMenu buildMenu;
    [SerializeField] Text [] costText = new Text[3];
    [SerializeField] Button [] upgradeButtons = new Button [3];
    [SerializeField] Button previewReturn;



    // Start is called before the first frame update
    void Start()
    {
        buildMenu = GetComponentInParent<BuildMenu>();
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
        costText[0].text = "Cost: " + t.upgrade1Cost.ToString();
        costText[1].text = "Cost: " + t.upgrade2Cost.ToString();
        costText[2].text = "Cost: " + t.upgrade3Cost.ToString();
        sellPrice.gameObject.SetActive(true);
        sellPrice.text = "Sell Value: " + ((int) buildMenu.CurrentPlate.SellValue() * GameManager.sellPercentage).ToString();
        UpdateUpgradeButtons();
        previewReturn.gameObject.SetActive(false);
        if(CanRepair()){
            repairButton.interactable = true;
        }
        else{
            repairButton.interactable = false;
        }

    }
    public void UpdatePreviewInfo(Tower t){
        sellPrice.gameObject.SetActive(false);
        currentTower = t;
        towerName.text = currentTower.towerName;
        towerDescription.text = currentTower.description;
        upgrade1Desc.text = currentTower.upgrade1Desc;
        upgrade2Desc.text = currentTower.upgrade2Desc;
        upgrade3Desc.text = currentTower.upgrade3Desc;
        costText[0].text = "Cost: " + t.upgrade1Cost.ToString();
        costText[1].text = "Cost: " + t.upgrade2Cost.ToString();
        costText[2].text = "Cost: " + t.upgrade3Cost.ToString();
        //UpdateUpgradeButtons();

    }
    public void OpenTowerMenu(){
        if(buildMenu.CurrentPlate != null){
            infoHolder.SetActive(true);
            UpdateInfo(buildMenu.CurrentPlate.CurrentTower());
        }
        else{
            return;
        }
        
    }

    public void OpenTowerMenuPreview(Tower t){
        UpdatePreviewInfo(t);
        infoHolder.SetActive(true);
        previewReturn.gameObject.SetActive(true);
        upgradeButtons[0].interactable = false;
        upgradeButtons[1].interactable = false;
        upgradeButtons[2].interactable = false;

    }
    public void CloseTowerMenuPreview(){
        infoHolder.SetActive(false);
        previewReturn.gameObject.SetActive(false);
        upgradeButtons[0].interactable = true;
        upgradeButtons[1].interactable = true;
        upgradeButtons[2].interactable = true;

    }
    bool CanRepair(){
        if(buildMenu.CurrentPlate == null){
            return false;
        }
        if(buildMenu.CurrentPlate.BuildIndex > 0 && GameManager.instance.Balance >= GameManager.instance.RepairCost && buildMenu.CurrentPlate.Health < buildMenu.CurrentPlate.MaxHealth){
            return true;
        }
        else{
            return false;
        }
    }
    public void UpdateUpgradeButtons(){
        Debug.Log("Updated Upgrade buttons");
        if(buildMenu.CurrentPlate.CurrentUpgrade == 0){
            if(GameManager.instance.Balance >= buildMenu.CurrentPlate.CurrentTower().upgrade1Cost){
                upgradeButtons[0].interactable = true;
                //upgradeButtons[0].interactable = true;
            }
            else{
                upgradeButtons[0].interactable = false;
            }
            if(GameManager.instance.Balance >= buildMenu.CurrentPlate.CurrentTower().upgrade2Cost){
                upgradeButtons[1].interactable = true;
            }
            else{
                upgradeButtons[1].interactable = false;
            }
            if(GameManager.instance.Balance >= buildMenu.CurrentPlate.CurrentTower().upgrade3Cost){
                upgradeButtons[2].interactable = true;
            }
            else{
                upgradeButtons[2].interactable = false;
            }
        }
        else if(buildMenu.CurrentPlate.CurrentUpgrade > 0){
            upgradeButtons[0].interactable = false;
            upgradeButtons[1].interactable = false;
            upgradeButtons[2].interactable = false;
        }
    }
    public void CloseTowerMenu(){
        infoHolder.SetActive(false);
    }
    
}
