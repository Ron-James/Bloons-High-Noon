using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildMenu : MonoBehaviour
{
    [SerializeField] GameObject meunu;
    [SerializeField] Tower [] towers;
    [SerializeField] Button [] buttons;
    [SerializeField] Button demolish;
    BuildPlate currentPlate;

    public BuildPlate CurrentPlate { get => currentPlate; set => currentPlate = value; }

    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void OpenMenu(BuildPlate buildPlate){
        meunu.SetActive(true);
        currentPlate = buildPlate;
        UpdateButtons();
        GameObject.Find("FirstPerson-AIO").GetComponent<FirstPersonAIO>().DisableCamera();
        GameObject.Find("FirstPerson-AIO").GetComponent<FirstPersonAIO>().playerCanMove = false;
    }

    public void CloseMenu(){
        meunu.SetActive(false);
        GameObject.Find("FirstPerson-AIO").GetComponent<FirstPersonAIO>().EnableCamera();
        GameObject.Find("FirstPerson-AIO").GetComponent<FirstPersonAIO>().playerCanMove = true;
    }

    public void Demolish(){
        currentPlate.Demolish();
        UpdateButtons();
    }
    public void Build(int index){
        currentPlate.BuildTower(index);
        UpdateButtons();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void UpdateButtons(){
        if(towers.Length != buttons.Length){
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
            if(currentPlate.BuildIndex > 0){
                demolish.gameObject.SetActive(true);
            }
            else{
                demolish.gameObject.SetActive(false);
            }
        }
    }
}
