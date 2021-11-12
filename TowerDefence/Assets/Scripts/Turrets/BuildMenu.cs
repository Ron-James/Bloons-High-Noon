using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildMenu : MonoBehaviour
{
    [SerializeField] GameObject menu;
    [SerializeField] GameObject towerInfo;
    [SerializeField] GameObject crossHair;
    [SerializeField] GameObject towerOptions;
    [SerializeField] Tower [] towers;
    [SerializeField] Button [] buttons;
    [SerializeField] Button demolish;
    [SerializeField] Button repair;
    [SerializeField] Button unlock;
    [SerializeField] Text lockedText;
    [SerializeField] BuildPlate currentPlate;
    [SerializeField] bool unlocked = false;
    [SerializeField] TowerMenu towerMenu;

    public static bool MenuIsOpen;

    public BuildPlate CurrentPlate { get => currentPlate; set => currentPlate = value; }
    public bool Unlocked { get => unlocked; set => unlocked = value; }

    // Start is called before the first frame update
    void Start()
    {
        //towerMenu = GetComponentInChildren<TowerMenu>();
        MenuIsOpen = false;
    }
    public void OpenMenu(BuildPlate buildPlate){
        crossHair.SetActive(false);
        unlocked = buildPlate.Unlocked;
        MenuIsOpen = true;
        currentPlate = buildPlate;
        menu.SetActive(true);
        UpdateButtons();
        GameObject.Find("Player").GetComponent<FirstPersonAIO>().DisableCamera();
        GameObject.Find("Player").GetComponent<FirstPersonAIO>().playerCanMove = false;
    }

    public void CloseMenu(){
        crossHair.SetActive(true);
        unlocked = false;
        MenuIsOpen = false;
        currentPlate = null;
        menu.SetActive(false);
        //GetComponentInChildren<TowerMenu>().CloseTowerMenu();
        GameObject.Find("Player").GetComponent<FirstPersonAIO>().EnableCamera();
        GameObject.Find("Player").GetComponent<FirstPersonAIO>().playerCanMove = true;
    }
    public void UpgradeCurrentPlate(int index){
        if(currentPlate == null){
            return;
        }
        else{
            currentPlate.UpgradeCurrentBuild(index);
            currentPlate.UpgradeHealth();
            towerMenu.UpdateUpgradeButtons();
        }
    }
    public void Demolish(){
        //UpgradeCurrentPlate(0);
        towerMenu.UpdateUpgradeButtons();
        currentPlate.Demolish();
        UpdateButtons();
    }
    public void PreviewTower(Tower t){
        towerMenu.OpenTowerMenuPreview(t);
        towerOptions.SetActive(false);
        demolish.gameObject.SetActive(false);
    }
    public void ClosePreview(){
        towerOptions.SetActive(true);
        towerMenu.CloseTowerMenuPreview();
    }
    public void Build(int index){
        currentPlate.BuildTower(index);
        UpdateButtons();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void RepairBuildPlate(){
        GameManager.instance.Purchase((int)GameManager.instance.RepairCost);
        currentPlate.Repair();
        UpdateButtons();
    }
    bool CanRepair(){
        if(currentPlate.BuildIndex != 0 && GameManager.instance.Balance >= GameManager.instance.RepairCost && !currentPlate.HasFullHealth()){
            return true;
        }
        else{
            return false;
        }
    }
    public void UnlockPlate(){
        if(unlocked){
            UpdateButtons();
            return;
        }
        else{
            currentPlate.Unlocked = true;
            unlocked = true;
            GameManager.instance.Purchase(GameManager.UnlockPrice);
            UpdateButtons();
        } 
    }
    public void UpdateButtons(){
        if(unlocked){
            if(currentPlate.BuildIndex > 0){
                lockedText.gameObject.SetActive(false);
                unlock.gameObject.SetActive(false);
                towerOptions.SetActive(false);
                demolish.gameObject.SetActive(true);
                if(CanRepair()){
                    repair.gameObject.SetActive(true);
                }
                else{
                    repair.gameObject.SetActive(false);
                }
                towerMenu.OpenTowerMenu();
                towerMenu.UpdateUpgradeButtons();

            }
            else{
                towerMenu.CloseTowerMenu();
                lockedText.gameObject.SetActive(false);
                unlock.gameObject.SetActive(false);
                towerOptions.SetActive(true);
                if(towers.Length != buttons.Length || currentPlate == null){
                    Debug.Log("not enough towers or buttons");
                    return;
                }
                else{
                    for(int loop = 0; loop < buttons.Length; loop++){
                        if(GameManager.instance.Balance < towers[loop].cost || currentPlate.BuildIndex > 0){
                            buttons[loop].gameObject.SetActive(false);
                        }
                        else{
                            buttons[loop].gameObject.SetActive(true);
                        }
                    }
                
                }
            }
            
        }
        else if(!unlocked && GameManager.instance.Balance >= GameManager.UnlockPrice){
            lockedText.gameObject.SetActive(true);
            unlock.gameObject.SetActive(true);
            towerOptions.SetActive(false);
            demolish.gameObject.SetActive(false);
        }
        else{
            lockedText.gameObject.SetActive(true);
            unlock.gameObject.SetActive(false);
            towerOptions.SetActive(false);
            demolish.gameObject.SetActive(false);
        }
        
    }
}
