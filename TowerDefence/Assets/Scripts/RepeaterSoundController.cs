using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepeaterSoundController : MonoBehaviour
{
    [SerializeField] AudioClip shotSound;
    AudioSource src;
    // Start is called before the first frame update
    void Start()
    {
        src = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayShotSound(){
        src.clip = shotSound;
        src.Play();
    }
}
