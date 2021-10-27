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
    float velocity;

    public GameObject HookedObject { get => hookedObject; set => hookedObject = value; }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0) && !fired){
            StartCoroutine(Extend(hookTravelSpd, maxDistance));
        }
        /*
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
        */
    }
    void ReturnHook(){
        hook.transform.rotation= hookHolder.transform.rotation;
        hook.transform.position = hookHolder.transform.position;
        fired = false;
        hooked = false;
    }

    IEnumerator Climb(){
        while(true){
            if(Physics.Raycast(player.transform.position, Vector3.down, 3f)){
                player.GetComponent<Rigidbody>().useGravity = true;
                break;
            }
            else{
                player.transform.Translate(Vector3.forward * Time.fixedDeltaTime * 1.5f);
                player.transform.Translate(Vector3.up *Time.deltaTime * 1.5f);
                yield return null;
            }
        }
    }

    IEnumerator Reel(){
        player.GetComponent<Rigidbody>().useGravity = false;
        hook.transform.SetParent(hookedObject.transform);
        while(true){
            player.transform.position = Vector3.MoveTowards(player.transform.position, hook.transform.position, playerTravelSpd * Time.deltaTime);
            float distanceToHook = Vector3.Distance(transform.position, hook.transform.position);
            if(distanceToHook < 2){
                StartCoroutine(Climb());
                break;
            }
            else{
                yield return null;
            }
            
        }
    }
    IEnumerator Extend(float period, float amplitude){
        fired = true;
        float w = (1/period) * 2 * Mathf.PI;
        float time = 0;
        Vector3 startPos = hook.transform.localPosition;
        hooked = false;
        velocity = 0;
        while(true){
            time += Time.fixedDeltaTime;
            float d = Mathf.Abs(amplitude * Mathf.Sin(w * time));
            velocity = Mathf.Cos(w * time);
            Debug.Log(amplitude);
            if(time >= period/4 || hooked){
                if(!hooked){
                    hook.transform.localPosition = startPos;
                    fired = false;
                    velocity = 0;
                    break;
                }
                else{
                    StartCoroutine(Reel());
                    break;
                }
                
            }
            else{
                
                hook.transform.localPosition = startPos + Vector3.forward * d;
                yield return new WaitForFixedUpdate();
            }
        }

    }
    
}
