using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapplingHook : MonoBehaviour
{
    [SerializeField] GameObject hook;
    [SerializeField] GameObject hookHolder;
    [SerializeField] GameObject player;
    [SerializeField] float hookTravelSpd;
    [SerializeField] float playerTravelSpd;

    public static bool fired;
    public bool hooked;
    GameObject hookedObject;

    [SerializeField] float maxDistance;
    float currentDistance;

    public GameObject HookedObject { get => hookedObject; set => hookedObject = value; }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0) && !fired){
            fired = true;
        }
        if(fired && !hooked){
            hook.transform.Translate(Vector3.forward * Time.deltaTime * hookTravelSpd);
            currentDistance = Vector3.Distance(transform.position, hook.transform.position);
            if(currentDistance >= maxDistance){
                ReturnHook();
            }
        }
        if(hooked && fired){
            hook.transform.SetParent(hookedObject.transform);
            transform.position = Vector3.MoveTowards(transform.position, hook.transform.position, playerTravelSpd * Time.deltaTime);
            float distanceToHook = Vector3.Distance(transform.position, hook.transform.position);
            
            this.GetComponent<Rigidbody>().useGravity = false;
            
            if(distanceToHook < 2){
                if(!this.GetComponent<PlayerController>().IsGrounded){
                    this.transform.Translate(Vector3.forward * Time.fixedDeltaTime * 7f);
                    this.transform.Translate(Vector3.up *Time.deltaTime * 10f);
                    StartCoroutine(Climb());
                }
            }
            
        }
        else{
            hook.transform.SetParent(hookHolder.transform);
            this.GetComponent<Rigidbody>().useGravity = true;
        }
    }
    void ReturnHook(){
        hook.transform.rotation= hookHolder.transform.rotation;
        hook.transform.position = hookHolder.transform.position;
        fired = false;
        hooked = false;
    }

    IEnumerator Climb(){
        yield return new WaitForSeconds(0.001f);
        ReturnHook();
    }
    
}
