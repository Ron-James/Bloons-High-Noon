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
    [SerializeField] float maxPlatformdist = 1.5f;


    public static bool fired;
    [SerializeField] bool hooked;
    GameObject hookedObject;

    [SerializeField] float maxDistance;
    float currentDistance;
    float velocity;
    [SerializeField]Vector3 startPosition;
    [SerializeField] float climbUpTime = 0.5f;
    [SerializeField] float climbForwardTime = 0.5f;
    [SerializeField] float maxClimbHeight = 15f;
    [SerializeField] float climbSpeed = 1.5f;
    [SerializeField] float playerHeight = 1f;
    [SerializeField] LayerMask platformLayer;
    [SerializeField] LayerMask grappleLayer;
    Transform grappleGun;
    Collision hookCollision;
    

    public GameObject HookedObject { get => hookedObject; set => hookedObject = value; }
    public Vector3 HookedPosition { get => hookedPosition; set => hookedPosition = value; }
    public bool Hooked { get => hooked; set => hooked = value; }
    public Collision HookCollision { get => hookCollision; set => hookCollision = value; }

    RaycastHit platformHit;

    // Start is called before the first frame update
    void Start()
    {
        hook.gameObject.GetComponent<MeshRenderer>().enabled = false;
        player = GetComponentInParent<FirstPersonAIO>().gameObject;
        startPosition = hook.transform.localPosition;
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0) && !fired){
            StartCoroutine(Extend(hookTravelSpd, maxDistance));
        }
        if(fired){
            LineRenderer rope = hook.GetComponent<LineRenderer>();
            rope.positionCount = 2;
            rope.SetPosition(0, firePoint.position);
            rope.SetPosition(1, hook.transform.position);
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
        ResetLine();
        hook.gameObject.GetComponent<MeshRenderer>().enabled = false;
        LineRenderer rope = hook.GetComponent<LineRenderer>();
        rope.positionCount = 0;

        fired = false;
        Hooked = false;
        player.GetComponent<Rigidbody>().useGravity = true;
        hook.transform.SetParent(firePoint);
        hookedObject = null;
        player.GetComponent<Collider>().enabled = true;
        hook.transform.localPosition = startPosition;
        hook.transform.rotation = firePoint.rotation;
        
    }

    IEnumerator ClimbUpForward(float durationFor, float durationUp){
        float time = 0;
        Vector3 point = Vector3.zero;
        float distance = 0;
        if(Physics.Raycast(player.transform.position - (Vector3.up * playerHeight), Vector3.up, out platformHit, maxClimbHeight, platformLayer)){
            distance = Vector3.Distance(player.transform.position , platformHit.point) + (playerHeight*maxPlatformdist);
            point = distance * Vector3.up;
            float pointHeight = point.y;
            while(true){
                time += Time.deltaTime;
                float currentHeight = player.transform.position.y;
                if(time >= durationUp || player.transform.position.y > pointHeight){
                    StartCoroutine(ClimbForward(durationFor));
                    break;
                }
                else if(Input.GetKeyDown(KeyCode.Space)){
                    ReturnHook();
                    GetComponentInParent<FirstPersonAIO>().playerCanMove = true;
                    break;
                } 
                else{
                    player.transform.Translate(Vector3.up * Time.deltaTime * climbSpeed);
                    yield return null;
                }
        }
        }
        else{
            ReturnHook();
            GetComponentInParent<FirstPersonAIO>().playerCanMove = true;
            Debug.Log("Falling");
            yield break;
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
            if(time >= duration){
                ReturnHook();
                GetComponentInParent<FirstPersonAIO>().playerCanMove = true;
                break;
            }
            else if(Input.GetKeyDown(KeyCode.Space)){
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
                //StartCoroutine(ClimbUp(durationUp));
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
            else if(Input.GetKeyDown(KeyCode.Space)){
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
            if(distanceToHook < 1f){
                if(!GetComponentInParent<FirstPersonAIO>().IsGrounded){
                    StartCoroutine(ClimbUpForward(climbForwardTime, climbUpTime));
                    LineRenderer rope = hook.GetComponent<LineRenderer>();
                    rope.positionCount = 0;
                }
                else{
                    ReturnHook();
                    LineRenderer rope = hook.GetComponent<LineRenderer>();
                    rope.positionCount = 0;
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

    void DrawLine(Vector3 from, Vector3 to){
        hook.GetComponent<LineRenderer>().positionCount = 2;
        hook.GetComponent<LineRenderer>().SetPosition(0, from);
        hook.GetComponent<LineRenderer>().SetPosition(0, to);

    }
    void ResetLine(){
        hook.GetComponent<LineRenderer>().positionCount = 0;
    }
    IEnumerator Extend(float period, float amplitude){
        hook.gameObject.GetComponent<MeshRenderer>().enabled = true;
        fired = true;
        float w = (1/period) * 2 * Mathf.PI;
        float time = 0;
        float speed = amplitude / period;
        Vector3 startPos = hook.transform.localPosition;
        Hooked = false;
        velocity = 0;
        while(true){
            time += Time.fixedDeltaTime;
            float d = Mathf.Abs(amplitude * Mathf.Sin(w * time));
            velocity = amplitude * w * Mathf.Cos(w * time);
            
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
                Vector3 localPos = startPos + Vector3.forward * d;
                Vector3 newPos = transform.TransformPoint(localPos);

                hook.transform.Translate(Vector3.forward * Time.fixedDeltaTime * speed);
                
                //hook.transform.localPosition = startPos + Vector3.forward * d;
                yield return new WaitForFixedUpdate();
            }
        }

    }
    
}
