using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScenarioStartTrigger : MonoBehaviour
{
    [SerializeField] Enemy basicEnemy;
    bool started;
    // Start is called before the first frame update
    void Start()
    {
        started = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other) {
        if(other.tag == "Player" && !started){
            GameManager.instance.SpawnEnemy(basicEnemy);
            
            started = true;
        }
        
    }

}
