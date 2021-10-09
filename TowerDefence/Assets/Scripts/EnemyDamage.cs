using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    [SerializeField] int damage = 1;
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
            case "Build Plate":
                other.gameObject.GetComponent<BuildPlate>().TakeDamage(damage);
                GetComponent<EnemyHealth>().Kill();
            break;
            case "Tower":
                GameManager.instance.DamageTower(damage);
            break;
        }
    }
}
