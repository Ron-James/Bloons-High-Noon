using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScenarioStartTrigger : MonoBehaviour
{
    [SerializeField] Enemy basicEnemy;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other) {
        if(other.tag == "Player"){
            GameManager.instance.SpawnEnemy(basicEnemy);
        }
        
    }

}
