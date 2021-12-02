using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySound : MonoBehaviour
{
    [SerializeField] AudioSource engine;
    [SerializeField] AudioSource body;
    public Sound engineNoise;
    public Sound [] projectileHit;
    // Start is called before the first frame update
    void Start()
    {
        engineNoise.src = engine;
        foreach(Sound sound in projectileHit){
            sound.src = body;
        }
    }
    private void OnEnable() {
        engineNoise.PlayLoop();
    }

    public void ProjectileHitSound(){
        int rand = Random.Range(0, projectileHit.Length - 1);
        projectileHit[rand].PlayOnce();
    }



    
    
}
