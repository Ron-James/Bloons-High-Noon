using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildMenuTrigger : MonoBehaviour
{
    [SerializeField] IslandBuilder islandBuilder;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerStay(Collider other) {
        switch(other.tag){
            case "Player":
                if(Input.GetKeyDown(KeyCode.X)){
                    islandBuilder.OpenBuildMenu();
                }
                Debug.Log("something");
            break;
        }
        
    }
}
