using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapplingHook : MonoBehaviour
{
    [SerializeField] GameObject hook;
    GameObject player;
    [SerializeField] Transform firePoint;
    [SerializeField] float hookTravelSpd = 2f;
    [SerializeField] float playerTravelSpd = 5f;
    [SerializeField] Vector3 hookedPosition;


    public static bool fired;
    [SerializeField] bool hooked;
    GameObject hookedObject;

    [SerializeField] float maxDistance;
    float currentDistance;
    float velocity;
    [SerializeField]Vector3 startPosition;
    [SerializeField] float climbUpTime = 0.5f;
    [SerializeField] float climbForwardTime = 0.5f;
    Transform grappleGun;
    

    public GameObject HookedObject { get => hookedObject; set => hookedObject = value; }
    public Vector3 HookedPosition { get => hookedPosition; set => hookedPosition = value; }
    public bool Hooked { get => hooked; set => hooked = value; }

    // Start is called before the first frame update
    void Start()
    {
        player = GetComponentInParent<FirstPersonAIO>().gameObject;
        startPosition = hook.transform.localPosition;
        
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
        hook.transform.localPosition = startPosition;
        fired = false;
        Hooked = false;
        player.GetComponent<Rigidbody>().useGravity = true;
        hook.transform.SetParent(firePoint);
        hook.transform.localPosition = Vector3.zero;
        hookedObject = null;
        player.GetComponent<Collider>().enabled = true;
        
    }

    IEnumerator ClimbUpForward(float durationUp, float durationFor){
        float time = 0;
        while(true){
            time += Time.deltaTime;
            if(time >= durationUp || player.GetComponent<FirstPersonAIO>().IsGrounded){
                StartCoroutine(ClimbForward(durationFor));
                break;
            }
            else{
                player.transform.Translate(Vector3.up * Time.fixedDeltaTime);
                yield return null;
            }
        }
    }

IEnumerator ClimbUp(float durationUp){
        float time = 0;
        while(true){
            time += Time.deltaTime;
            if(time >= durationUp || player.GetComponent<FirstPersonAIO>().IsGrounded){
                ReturnHook();
                GetComponentInParent<FirstPersonAIO>().playerCanMove = true;
                break;
            }
            else{
                player.transform.Translate(Vector3.up * Time.fixedDeltaTime);
                yield return null;
            }
        }
    }
    IEnumerator ClimbForward(float duration){
        float time = 0;
        while(true){
            time += Time.deltaTime;
            if(time >= duration || player.GetComponent<FirstPersonAIO>().IsGrounded){
                ReturnHook();
                GetComponentInParent<FirstPersonAIO>().playerCanMove = true;
                break;
            }
            else{
                player.transform.Translate(Vector3.forward * Time.fixedDeltaTime);
                yield return null;
            }
        }
    }
    IEnumerator ClimbForwardUp(float durationFor, float durationUp){
        float time = 0;
        while(true){
            time += Time.deltaTime;
            if(time >= durationFor || player.GetComponent<FirstPersonAIO>().IsGrounded){
                StartCoroutine(ClimbUp(durationUp));
                GetComponentInParent<FirstPersonAIO>().playerCanMove = true;
                break;
            }
            else{
                player.transform.Translate(Vector3.forward * Time.fixedDeltaTime);
                yield return null;
            }
        }
    }
    IEnumerator Climb(float duration){
        float time = 0;
        while(true){
            time += Time.deltaTime;
            if(time >= duration || player.GetComponent<FirstPersonAIO>().IsGrounded){
                ReturnHook();
                GetComponentInParent<FirstPersonAIO>().playerCanMove = true;
                break;
            }
            else{
                player.transform.Translate(Vector3.forward * Time.fixedDeltaTime * 10f);
                player.transform.Translate(Vector3.up * Time.deltaTime * 1f);
                yield return null;
            }
        }
        
        
    }
    

    IEnumerator Reel(){
        //GetComponentInParent<FirstPersonAIO>().DisableCamera();
        GetComponentInParent<FirstPersonAIO>().playerCanMove = false;
        player.GetComponent<Rigidbody>().useGravity = false;
        hook.transform.SetParent(hookedObject.transform);
        player.GetComponent<Collider>().enabled = false;
        while(true){
            //hook.transform.position = hookedPosition;
            player.transform.position = Vector3.MoveTowards(player.transform.position, hookedPosition, playerTravelSpd * Time.deltaTime);
            float distanceToHook = Vector3.Distance(player.transform.position, hookedPosition);
            if(distanceToHook < 0.5f || !Hooked){
                if(!GetComponentInParent<FirstPersonAIO>().IsGrounded){
                    StartCoroutine(ClimbUpForward(climbUpTime, climbForwardTime));
                }
                else{
                    ReturnHook();
                    GetComponentInParent<FirstPersonAIO>().playerCanMove = true;
                }
                //GetComponentInParent<FirstPersonAIO>().EnableCamera();
                break;
            }
            else{
                yield return null;
            }
            
        }
    }
    public void HookObject(){
        hooked = true;
    }
    IEnumerator Extend(float period, float amplitude){
        fired = true;
        float w = (1/period) * 2 * Mathf.PI;
        float time = 0;
        Vector3 startPos = hook.transform.localPosition;
        Hooked = false;
        velocity = 0;
        while(true){
            time += Time.fixedDeltaTime;
            float d = Mathf.Abs(amplitude * Mathf.Sin(w * time));
            velocity = Mathf.Cos(w * time);
            
            if(time >= period/4 || Hooked || hookedObject != null){
                if(!Hooked){
                    ReturnHook();
                    
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
