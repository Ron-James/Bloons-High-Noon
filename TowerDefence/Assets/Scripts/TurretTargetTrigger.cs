using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretTargetTrigger : MonoBehaviour
{
    // Start is called before the first frame update
    private void OnTriggerStay(Collider other) {
        switch(other.tag){
            case "Enemy":
                if(GetComponentInParent<TurretAim>().Target == null){
                    GetComponentInParent<TurretAim>().Target = other.transform;
                }
            break;
        }
    }
    private void OnTriggerExit(Collider other) {
        if(GetComponentInParent<TurretAim>().Target = other.transform){
            GetComponentInParent<TurretAim>().Target = null;
        }
    }
}
