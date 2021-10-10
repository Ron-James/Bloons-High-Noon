using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretAim : MonoBehaviour
{
    [SerializeField] Transform barrel;
    [SerializeField] float rotationSpeed;
    [SerializeField] float restoreRotationSpeed;
    [SerializeField] Transform target;

    Quaternion defaultRotation;

    public Transform Target { get => target; set => target = value; }

    // Start is called before the first frame update
    void Start()
    {
        defaultRotation = Quaternion.Euler(0,0,0);
    }
    private void OnEnable() {
        
    }
    // Update is called once per frame
    void Update()
    {
        Aim(target);
    }

    public void Aim(Transform target){
        if(target != null){
            Quaternion targetRot = Quaternion.LookRotation(target.position - barrel.transform.position);
            barrel.transform.rotation = Quaternion.RotateTowards(barrel.transform.rotation, targetRot, rotationSpeed * Time.deltaTime);
        }
        else{
            barrel.transform.rotation = Quaternion.RotateTowards(barrel.transform.rotation, defaultRotation, restoreRotationSpeed * Time.deltaTime);
        }
       
    }
}
