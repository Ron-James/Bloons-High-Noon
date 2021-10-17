using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuTrigger : MonoBehaviour
{
    [SerializeField] BuildMenu buildMenu;
    BuildPlate buildPlate;
    [SerializeField] GameObject buildPrompt;
    bool inRegion;
    // Start is called before the first frame update
    void Start()
    {
        //buildPrompt.SetActive(false);
        buildPrompt.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(buildPlate != null){
            buildPrompt.SetActive(true);
            if(Input.GetKeyDown(KeyCode.B)){
                buildMenu.OpenMenu(buildPlate);
            } 
        }
        else{
            buildPrompt.SetActive(false);
        }
    }
    private void OnTriggerEnter(Collider other) {
        switch(other.tag){
            case "MenuTrigger":
                buildPlate = other.gameObject.GetComponentInParent<BuildPlate>();
            break;
        }
    }
    private void OnTriggerExit(Collider other) {
        switch(other.tag){
            case "MenuTrigger":
                buildPlate = null;
            break;
        }
    }
}
