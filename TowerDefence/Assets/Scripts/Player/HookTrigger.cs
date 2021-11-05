using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookTrigger : MonoBehaviour
{
    [SerializeField] GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        player = GetComponentInParent<FirstPersonAIO>().gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
    private void OnTriggerEnter(Collider other) {
        switch(other.gameObject.layer){
            case 7:
                Debug.Log("Hooked");
                player.GetComponentInChildren<GrapplingHook>().HookedPosition = transform.position;
                player.GetComponentInChildren<GrapplingHook>().Hooked = true;
                player.GetComponentInChildren<GrapplingHook>().HookedObject = other.gameObject;
            break;
        }
    }
    
    
    private void OnCollisionEnter(Collision other) {
        
        if(other.collider.gameObject.layer == 7 && GrapplingHook.fired){
            Debug.Log("Hooked");
            player.GetComponentInChildren<GrapplingHook>().HookedPosition = transform.position;
            player.GetComponentInChildren<GrapplingHook>().Hooked = true;
            player.GetComponentInChildren<GrapplingHook>().HookCollision = other;
            player.GetComponentInChildren<GrapplingHook>().HookedObject = other.gameObject;
        }
        else{
            Physics.IgnoreCollision(GetComponent<Collider>(), other.collider, true);
        }
    }
    
}
