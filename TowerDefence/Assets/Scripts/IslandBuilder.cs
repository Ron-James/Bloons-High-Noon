using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IslandBuilder : MonoBehaviour
{
    [SerializeField] Transform buildPos;
    [SerializeField] GameObject buildMenu;

    [SerializeField] Tower [] towers;
    [SerializeField] Button [] buttons;
    [SerializeField] Button demolish;
    bool hasStructure;
    Tower built;
    GameObject structure;


    
    // Start is called before the first frame update
    void Start()
    {
        UpdateButtons();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other) {
        
    }


    public void Build(Tower tower){
        structure = Instantiate(tower.tower, buildPos.position, Quaternion.identity);
        hasStructure = true;
        built = tower;
        GameManager.instance.Purchase(tower.cost);
        UpdateButtons();
    }
    public void Demolish(){
        if(!hasStructure){
            Debug.Log("Has no structure");
        }
        else{
            Destroy(structure);
            hasStructure = false;
            GameManager.instance.AddBalance(built.CalculateSellValue());
            structure = null;
            built = null;
            UpdateButtons();
        }
    }

    public void OpenBuildMenu(){
        buildMenu.SetActive(true);
        UpdateButtons();
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        GameObject.Find("Player").GetComponent<CameraController>().DisableCameraControl();
    }
    public void CloseBuildMenu(){
        buildMenu.SetActive(false);
        GameObject.Find("Player").GetComponent<CameraController>().EnableCameraControl();
    }
    public void UpdateButtons(){
        if(towers.Length != buttons.Length){
            Debug.Log("not enough towers or buttons");
            return;
        }
        else{
            for(int loop = 0; loop < buttons.Length; loop++){
                if(GameManager.instance.Balance < towers[loop].cost || hasStructure){
                    buttons[loop].gameObject.SetActive(false);
                    if(hasStructure){
                        demolish.gameObject.SetActive(true);
                    }
                    else{
                        demolish.gameObject.SetActive(false);
                    }
                }
                else{
                    buttons[loop].gameObject.SetActive(true);
                }
            }
        }
    }
    
}
