using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;

public class EnemyFollower : MonoBehaviour
{
    public PathCreator pathCreator;
    EndOfPathInstruction end; 
    [SerializeField] float speed = 10f;
    [SerializeField] ParticleSystem sparks;
    [SerializeField] ParticleSystem flames;

    float distanceTravelled;
    [SerializeField] bool isStopped;


    Vector3 currentPositionOnPath;
    bool isFrozen;
    bool isSlowed;
    bool isSlowedExpo;
    bool onFire = false;
    

    
    public bool IsStopped { get => isStopped; set => isStopped = value; }
    public Vector3 CurrentPositionOnPath { get => currentPositionOnPath; set => currentPositionOnPath = value; }
    public bool IsFrozen { get => isFrozen; set => isFrozen = value; }
    public bool IsSlowed { get => isSlowed; set => isSlowed = value; }
    public bool IsSlowedExpo { get => isSlowedExpo; set => isSlowedExpo = value; }
    public bool OnFire { get => onFire; set => onFire = value; }


    // Start is called before the first frame update
    void Start()
    {
        IsFrozen = false;
        distanceTravelled = 0;
        end = EndOfPathInstruction.Stop;
        if(GameManager.instance != null){
            pathCreator = GameManager.instance.Lane;
        }
        else{
            Debug.Log("Where GameManager");
        }
        flames.Stop();
        sparks.Stop();
    }
    private void OnEnable() {
        isSlowedExpo = false;
        isStopped = false;
        isFrozen = false;
        isSlowed = false;
        distanceTravelled = 0;
        flames.Stop();
        sparks.Stop();
        onFire = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(!isStopped){
            distanceTravelled += (speed * Time.deltaTime);
            transform.position = pathCreator.path.GetPointAtDistance(distanceTravelled, end);
            if(distanceTravelled > pathCreator.path.length){
                currentPositionOnPath = pathCreator.path.GetPoint(1);
            }
            else{
                currentPositionOnPath = pathCreator.path.GetPointAtDistance(distanceTravelled, end);
            }
            

            transform.rotation = pathCreator.path.GetRotationAtDistance(distanceTravelled, end);

        }
        
        if(OnFire == true && !flames.isPlaying){
            flames.Play();
        }
        else if(OnFire == false && flames.isPlaying){
            flames.Stop();
        }
    }
    
    public void SlowEnemy(float duration, float percentage){
        if(isSlowed){
            StartCoroutine(SlowDown(duration, percentage)); 
        }
        else{
            return;
        }
        
    }
    public void SlowEnemyIndef(float percentage){
        StartCoroutine(SlowDownIndef(percentage)); 
    }
    public void FreezeEnemy(float duration){
        StartCoroutine(Freeze(duration));
    }

    public void ResetPosition(){
        transform.position = pathCreator.path.GetPointAtDistance(0);
        distanceTravelled = 0;
    }
    IEnumerator Freeze(float duration){
        float time = 0;
        isFrozen = true;
        isStopped = true;
        sparks.Play();
        while(true){
            time += Time.deltaTime;
            if(time >= duration){
                isStopped = false;
                isFrozen = false;
                sparks.Stop();
                break;
            }
            else{
                yield return null;
            }
        }
    }

    public void StartStunToSlow(float duration, float slowTime, float slowPercent){
        StartCoroutine(StunToSlow(duration, slowTime, slowPercent));
    }

    IEnumerator StunToSlow(float duration, float slowTime, float slowPercent){
        float time = 0;
        isFrozen = true;
        isStopped = true;
        sparks.Play();

        while(true){
            time += Time.deltaTime;
            if(time >= duration){
                isStopped = false;
                isFrozen = false;
                StartCoroutine(StunSlowDown(slowTime, slowPercent));
                break;
            }
            else{
                yield return null;
            }
        }
    }
    IEnumerator StunSlowDown(float duration, float percentage){
        float regSpd = speed;
        float regFireRate = 1;
        float regTowerFireRate = 1;
        if(GetComponent<EnemyRanged>() != null){
            regFireRate = GetComponent<EnemyRanged>().FireRate;
            GetComponent<EnemyRanged>().FireRate = (1/percentage) * regFireRate;
            regTowerFireRate = GetComponent<EnemyRanged>().TowerFireRate;
            GetComponent<EnemyRanged>().TowerFireRate = regFireRate * (1/percentage);
        }
        
        speed = regSpd * percentage;
        float time = 0;
        isSlowed = true;
        while(true){
            time += Time.deltaTime;
            if(time >= duration){
                speed = regSpd;
                isSlowed = false;
                if(GetComponent<EnemyRanged>() != null){
                    GetComponent<EnemyRanged>().FireRate = regFireRate;
                    GetComponent<EnemyRanged>().TowerFireRate = regFireRate;
                }
                sparks.Stop();
                break;
            }
            else{
                yield return null;
            }
        }
    }
    IEnumerator SlowDown(float duration, float percentage){
        float regSpd = speed;
        float regFireRate = 1;
        float regTowerFireRate = 1;
        if(GetComponent<EnemyRanged>() != null){
            regFireRate = GetComponent<EnemyRanged>().FireRate;
            GetComponent<EnemyRanged>().FireRate = (1/percentage) * regFireRate;
            regTowerFireRate = GetComponent<EnemyRanged>().TowerFireRate;
            GetComponent<EnemyRanged>().TowerFireRate = regFireRate * (1/percentage);
        }
        
        speed = regSpd * percentage;
        float time = 0;
        isSlowed = true;
        while(true){
            time += Time.deltaTime;
            if(time >= duration){
                speed = regSpd;
                isSlowed = false;
                if(GetComponent<EnemyRanged>() != null){
                    GetComponent<EnemyRanged>().FireRate = regFireRate;
                    GetComponent<EnemyRanged>().TowerFireRate = regFireRate;
                }
                break;
            }
            else{
                yield return null;
            }
        }
    }

    IEnumerator SlowDownIndef(float percentage){
        float regSpd = speed;
        isSlowedExpo = true;
        speed = regSpd * percentage;
        Debug.Log(speed + " speed");
        Debug.Log(percentage + " percent");
        float regFireRate = 1;
        float regTowerFireRate = 1;
        if(GetComponent<EnemyRanged>() != null){
            regFireRate = GetComponent<EnemyRanged>().FireRate;
            GetComponent<EnemyRanged>().FireRate = (1/percentage) * regFireRate;
            regTowerFireRate = GetComponent<EnemyRanged>().TowerFireRate;
            GetComponent<EnemyRanged>().TowerFireRate = regFireRate * (1/percentage);
        }
        //float time = 0;
        while(true){
            if(!isSlowedExpo){
                speed = regSpd;
                if(GetComponent<EnemyRanged>() != null){
                    GetComponent<EnemyRanged>().FireRate = regFireRate;
                    GetComponent<EnemyRanged>().TowerFireRate = regFireRate;
                }
                break;
            }
            else{
                yield return null;
            }
        }
    }
}
