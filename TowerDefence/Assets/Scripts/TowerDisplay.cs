using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerDisplay : MonoBehaviour
{
    [SerializeField] Tower tower;
    [SerializeField] Text towerName;
    [SerializeField] Text cost;
    
    // Start is called before the first frame update
    void Start()
    {
        towerName.text = tower.towerName;
        cost.text = "Cost: " + tower.cost.ToString();
        
    }
    private void OnEnable() {
        GetComponentInParent<IslandBuilder>().UpdateButtons();
    }

}
