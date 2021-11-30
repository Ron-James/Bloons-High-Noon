using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    [SerializeField] float damage = 1;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other) {
        switch(other.tag){
            case "Tower":
                Debug.Log("entered the tower");
                GameManager.instance.DamageTower(damage);
                if(GetComponent<EnemyHealth>().tutorialEnemy){
                    GetComponent<EnemyHealth>().TakeDamage(5000);
                }
                else{
                    GetComponent<EnemyHealth>().Kill();
                }
                
            break;
        }
    }
}
