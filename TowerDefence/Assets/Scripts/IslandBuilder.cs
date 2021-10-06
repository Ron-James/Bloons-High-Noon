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
    bool hasStructure;
    Island island;

    
    // Start is called before the first frame update
    void Start()
    {
        island = GetComponent<Island>();
        UpdateButtons();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other) {
        
    }


    public void Build(Tower tower){
        Instantiate(tower.tower, buildPos.position, Quaternion.identity);
        hasStructure = true;
        GameManager.instance.Purchase(tower.cost);
    }

    public void OpenBuildMenu(){
        buildMenu.SetActive(true);
        UpdateButtons();
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    public void CloseBuildMenu(){
        buildMenu.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    void UpdateButtons(){
        if(towers.Length != buttons.Length){
            Debug.Log("not enough towers or buttons");
            return;
        }
        else{
            for(int loop = 0; loop < buttons.Length; loop++){
                if(GameManager.instance.Balance < towers[loop].cost || hasStructure){
                    buttons[loop].gameObject.SetActive(false);
                }
                else{
                    buttons[loop].gameObject.SetActive(true);
                    buttons[loop].onClick.AddListener(delegate{Build(towers[loop]);});
                }
            }
        }
    }
    
}
