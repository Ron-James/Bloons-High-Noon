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
    bool stunned;

    public Transform Target { get => target; set => target = value; }
    public bool Stunned { get => stunned; set => stunned = value; }

    // Start is called before the first frame update
    void Start()
    {
        defaultRotation = transform.rotation;
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
        else if(stunned){
            return;
        }
        else if(target == null){
            barrel.transform.rotation = Quaternion.RotateTowards(barrel.transform.rotation, defaultRotation, restoreRotationSpeed * Time.deltaTime);
        }
       
    }


    IEnumerator StunTurret(float duration){
        stunned = true;
        float time = 0;

        while(true){
            time += Time.deltaTime;

            if(time >= duration){
                stunned = false;
                break;
            }
            else{
                yield return null;
            }
        }

    }
    public void Stun(float time){
        StartCoroutine(StunTurret(time));
    }


}
