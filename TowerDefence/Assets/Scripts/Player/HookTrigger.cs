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
        switch(other.tag){
            case "Hookable":
                Debug.Log("Hooked");
                player.GetComponentInChildren<GrapplingHook>().HookedPosition = transform.position;
                player.GetComponentInChildren<GrapplingHook>().Hooked = true;
                player.GetComponentInChildren<GrapplingHook>().HookedObject = other.gameObject;
            break;
        }
    }
    
}
