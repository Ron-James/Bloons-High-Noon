using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuTrigger : MonoBehaviour
{
    [SerializeField] BuildMenu buildMenu;
    BuildPlate buildPlate;
    [SerializeField] GameObject buildPrompt;
    bool inRegion;
    Vector3 startPos;
    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
        //buildPrompt.SetActive(false);
        buildPrompt.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(buildPlate != null && GameManager.instance.FirstPerson){
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
            case "Respawn":
                transform.position = startPos;
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
