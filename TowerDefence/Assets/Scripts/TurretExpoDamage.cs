using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretExpoDamage : MonoBehaviour
{
    [SerializeField] float damageConstant = 0.5f;
    [SerializeField] float coolDownTime = 0.2f;

    bool coolingDown;
    // Start is called before the first frame update
    void Start()
    {
        coolingDown = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(GetComponent<TurretAim>().Target != null && !coolingDown && !GetComponent<TurretAim>().Stunned){
            StartCoroutine(ExponentialDamage());
        }
    }
    IEnumerator CoolDown(float period){
        coolingDown = true;
        float time = 0;
        while(true){
            time += Time.deltaTime;
            if(time >= period){
                coolingDown = false;
                break;
            }
            else{
                yield return null;
            }
        }
    }
    IEnumerator ExponentialDamage(){
        if(GetComponent<TurretAim>().Target == null || GetComponent<TurretAim>().Target.GetComponent<EnemyHealth>() == null || coolingDown){
            yield break;
        }
        else{
            float time = 0;
            while(true){
                time += Time.deltaTime;
                if(GetComponent<TurretAim>().Target == null || GetComponent<TurretAim>().Stunned){
                    StartCoroutine(CoolDown(coolDownTime));
                    break;
                }
                else{
                    float damage = damageConstant * Mathf.Exp(time);
                    GetComponent<TurretAim>().Target.GetComponent<EnemyHealth>().TakeDamage(damage);
                    yield return null;
                }


            }
        }
    }
}
