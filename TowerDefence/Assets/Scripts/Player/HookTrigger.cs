using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookTrigger : MonoBehaviour
{
    [SerializeField] GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other) {
        switch(other.tag){
            case "Hookable":
               GetComponentInParent<GrapplingHook>().hooked = true;
               GetComponentInParent<GrapplingHook>().HookedObject = other.gameObject;
            break;
        }
    }
}
