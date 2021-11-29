using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretAim : MonoBehaviour
{
    [Header("Base and Barrel")]

    [SerializeField] float rotationSpeed;
    [SerializeField] float restoreRotationSpeed;
    [SerializeField] Transform target;
    
    

    Quaternion defaultRotation;
    Quaternion defaultBaseRotation;
    bool stunned;
    [SerializeField] UpgradeManager upgradeManager;

    public Transform Target { get => target; set => target = value; }
    public bool Stunned { get => stunned; set => stunned = value; }

    // Start is called before the first frame update
    void Start()
    {
        upgradeManager = GetComponent<UpgradeManager>();
        defaultRotation = upgradeManager.barrel.transform.rotation;
        defaultBaseRotation = upgradeManager.basePiece.transform.rotation;
        
    }
    private void OnEnable() {
        
    }
    // Update is called once per frame
    void Update()
    {
        if(GetComponent<TurretExpoDamage>() != null){
            if(GetComponent<TurretExpoDamage>().CoolingDown){
                Restore();
            }
            else{
                Aim(target);
            }
        }
        else{
            Aim(target);
        }
        
    }

    public void Aim(Transform target){
        if(target != null){
            Quaternion baseRot = Quaternion.LookRotation(target.position - upgradeManager.basePiece.transform.position);
            Vector3 baseAngles = baseRot.eulerAngles;
            baseAngles = new Vector3(0, baseAngles.y, 0);
            baseRot = Quaternion.Euler(baseAngles);

            Quaternion targetRot = Quaternion.LookRotation(target.position - upgradeManager.barrel.transform.position);

            upgradeManager.barrel.transform.rotation = Quaternion.RotateTowards(upgradeManager.barrel.transform.rotation, targetRot, rotationSpeed * Time.deltaTime);
            upgradeManager.basePiece.transform.rotation = Quaternion.RotateTowards(upgradeManager.basePiece.transform.rotation, baseRot, rotationSpeed * Time.deltaTime);
        }
        else if(stunned){
            return;
        }
        else if(target == null){
            upgradeManager.barrel.transform.rotation = Quaternion.RotateTowards(upgradeManager.barrel.transform.rotation, defaultRotation, restoreRotationSpeed * Time.deltaTime);
            upgradeManager.basePiece.transform.rotation = Quaternion.RotateTowards(upgradeManager.basePiece.transform.rotation, defaultBaseRotation, rotationSpeed * Time.deltaTime);
        }
       
    }

    public void Restore(){
        upgradeManager.barrel.transform.rotation = Quaternion.RotateTowards(upgradeManager.barrel.transform.rotation, defaultRotation, restoreRotationSpeed * Time.deltaTime);
        upgradeManager.basePiece.transform.rotation = Quaternion.RotateTowards(upgradeManager.basePiece.transform.rotation, defaultBaseRotation, rotationSpeed * Time.deltaTime);
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
    private void OnDisable() {
        StopAllCoroutines();
    }

}
