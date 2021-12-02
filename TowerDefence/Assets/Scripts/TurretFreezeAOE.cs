using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretFreezeAOE : MonoBehaviour
{
    [SerializeField] float fireRate = 0.9f;
    [SerializeField] float freezeDuration = 0.45f;
    [SerializeField] float damage = 0.4f;
    [SerializeField] GameObject lines;

    [SerializeField] GameObject electricLinePrefab;

    [Header("Upgrades")]
    [SerializeField] float [] fireRateUpgrades = new float [5];
    [SerializeField] float [] damageUpgrades = new float [5];
    [SerializeField] float [] freezeDurationUpgrades = new float [5];
    [SerializeField] float stunSlowTime = 0.5f;
    [SerializeField] [Range(0,1)] float stunSlowPercent= 0.5f;
    [SerializeField] bool stunSlow = false;
    [SerializeField] Sound zapSound;
    
    float fireCount;

    public float[] FireRateUpgrades { get => fireRateUpgrades; set => fireRateUpgrades = value; }
    public float[] DamageUpgrades { get => damageUpgrades; set => damageUpgrades = value; }
    public float[] FreezeDurationUpgrades { get => freezeDurationUpgrades; set => freezeDurationUpgrades = value; }
    public bool StunSlow { get => stunSlow; set => stunSlow = value; }

    TurretTargetTrigger targetTrigger;
    UpgradeManager upgradeManager;
    // Start is called before the first frame update
    void Start()
    {
        upgradeManager = GetComponent<UpgradeManager>();
        targetTrigger = GetComponentInChildren<TurretTargetTrigger>();
        fireCount = 0;
    }
    private void OnDisable() {
        StopAllCoroutines();
    }
    // Update is called once per frame
    void Update()
    {
        fireCount += Time.deltaTime; 
        if(targetTrigger.EnemiesInRangeCount() > 0 && fireCount >= (fireRate * (1/upgradeManager.FireRateUpgrade))){
            if(stunSlow){
                StunSlowEnemies(freezeDuration * upgradeManager.FreezeDurationUpgrade, stunSlowTime, stunSlowPercent);
            }
            else{
                targetTrigger.FreezeEnemiesInRange(freezeDuration * upgradeManager.FreezeDurationUpgrade);
            }
            fireCount = 0;
            targetTrigger.DamageAllEnemies(damage * upgradeManager.DamageUpgrade);
            ZapEnemies();
            
        }
    }
    IEnumerator DrawElectricLine(Transform enemy, Electric line, float duration){
        line.isPlaying = true;
        Debug.Log("zapped");
        line.transformPointA = upgradeManager.firePoint;
        line.transformPointB = enemy;
        line.gameObject.GetComponent<LineRenderer>().enabled = true;
        float time = 0;
        while(true){
            time += Time.deltaTime;   
            if(time >= duration){
                line.transformPointB = upgradeManager.firePoint;
                line.gameObject.GetComponent<LineRenderer>().enabled = false;
                line.isPlaying = false;
                break;
            }
            else{
                line.transformPointB = enemy;
                yield return null;
            }
        }    
    }

    public void StunSlowEnemies(float duration, float slowTime, float percent){
        List<Transform> inRange = targetTrigger.GetEnemiesInRange();
        if(inRange.Count == 0){
            return;
        }
        else{
            for(int loop = 0; loop < inRange.Count; loop++){
                inRange[loop].GetComponent<EnemyFollower>().StartStunToSlow(duration, slowTime, percent);
            }
        }
    }
    public void ZapEnemies(){
        List<Transform> inRange = targetTrigger.GetEnemiesInRange();
        int num = inRange.Count;
        if(num == 0){
            return;
        }
        else{
            zapSound.PlayOnce();
            Electric [] electricLines = lines.GetComponentsInChildren<Electric>();
            if(electricLines.Length < num){
                for(int k = 0; k < num - electricLines.Length; k++){
                    GameObject newLine = Instantiate(electricLinePrefab, upgradeManager.firePoint.position, Quaternion.identity);
                    newLine.transform.SetParent(lines.transform);
                    newLine.SetActive(false);
                }
                electricLines = lines.GetComponentsInChildren<Electric>();
            }
            for(int i = 0; i < num; i++){
                StartCoroutine(DrawElectricLine(inRange[i], electricLines[i], freezeDuration * upgradeManager.FreezeDurationUpgrade));
            }
        }
        
        
    }

    
}
