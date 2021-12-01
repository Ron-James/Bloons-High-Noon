using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    bool canHook = false;
    [SerializeField] Sprite crosshair01, crosshair02;
    [SerializeField] Image crosshair;
    [SerializeField] LayerMask hookLayer;

    [SerializeField] float maxDistance;
    float currentDistance;
    Camera cam;
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
    Quaternion defaultRot;

    public GameObject HookedObject { get => hookedObject; set => hookedObject = value; }
    public Vector3 HookedPosition { get => hookedPosition; set => hookedPosition = value; }
    public bool Hooked { get => hooked; set => hooked = value; }
    public Collision HookCollision { get => hookCollision; set => hookCollision = value; }

    RaycastHit platformHit;

    // Start is called before the first frame update
    void Start()
    {
        defaultRot = hook.transform.localRotation;
        hook.gameObject.GetComponent<MeshRenderer>().enabled = false;
        player = GetComponentInParent<FirstPersonAIO>().gameObject;
        startPosition = hook.transform.localPosition;
        crosshair.sprite = crosshair01;
        cam = GetComponentInParent<Camera>();
        fired = false;
        BuildMenu.MenuIsOpen = false;
        PauseMenu.IsPaused = false;
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.TransformDirection(Vector3.forward), out hit, maxDistance, hookLayer))
        {
            Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.TransformDirection(Vector3.forward) * maxDistance, Color.red);
            canHook = true;
            {
                if (hit.transform.gameObject.tag == "hookable")
                {
                    Debug.Log("Can Grapple");
                    canHook = true;
                    
                }
                
            }
           
            
        }
        else
        {
            Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.TransformDirection(Vector3.forward) * 50f, Color.green);
            canHook = false;
            
        }
        if (canHook)
        {
            crosshair.sprite = crosshair02;
        }
        else if(!canHook)
        {
            crosshair.sprite = crosshair01;
        }


        if (Input.GetMouseButtonDown(0) && !fired && !BuildMenu.MenuIsOpen && !PauseMenu.IsPaused && !GameManager.gameOver && canHook)
        {
            StartCoroutine(Extend(hookTravelSpd, maxDistance));
        }
        if (fired)
        {
            LineRenderer rope = hook.GetComponent<LineRenderer>();
            rope.positionCount = 2;
            rope.SetPosition(0, firePoint.position);
            rope.SetPosition(1, hook.transform.position);
        }

        
        
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
        hook.transform.localRotation = defaultRot;
        GetComponentInParent<FirstPersonAIO>().EnableCamera();
        GetComponentInParent<FirstPersonAIO>().playerCanMove = true;
        
    }

    

    
    
    
    

    IEnumerator Reel(){
        //GetComponentInParent<FirstPersonAIO>().DisableCamera();
        
        player.GetComponent<Rigidbody>().useGravity = false;
        hook.transform.SetParent(hookedObject.transform);
        player.GetComponent<Collider>().enabled = false;
        while(true){
            //hook.transform.position = hookedPosition;
            player.transform.position = Vector3.MoveTowards(player.transform.position, hookedPosition, playerTravelSpd * Time.deltaTime);
            float distanceToHook = Vector3.Distance(player.transform.position, hookedPosition);
            if(distanceToHook < 1f || Input.GetMouseButtonDown(1)){
                
                ReturnHook();
                LineRenderer rope = hook.GetComponent<LineRenderer>();
                rope.positionCount = 0;
                GetComponentInParent<FirstPersonAIO>().playerCanMove = true;
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
        GetComponentInParent<FirstPersonAIO>().playerCanMove = false;
        GetComponentInParent<FirstPersonAIO>().DisableCamera();
        hook.gameObject.GetComponent<MeshRenderer>().enabled = true;
        fired = true;
        float w = (1/period) * 2 * Mathf.PI;
        float time = 0;
        float speed = amplitude / period;
        Vector3 startPos = hook.transform.localPosition;
        Hooked = false;
        float distance = 0;
        velocity = 0;
        while(true){
            time += Time.fixedDeltaTime;
            float d = Mathf.Abs(amplitude * Mathf.Sin(w * time));
            velocity = amplitude * w * Mathf.Cos(w * time);
            distance = Vector3.Distance(hook.transform.position, firePoint.transform.position);
            
            if(distance >= amplitude|| Hooked || hookedObject != null || Input.GetMouseButtonDown(1)){
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

                hook.transform.Translate(Vector3.up * Time.fixedDeltaTime * speed);
                
                //hook.transform.localPosition = startPos + Vector3.forward * d;
                yield return new WaitForFixedUpdate();
            }
        }

    }
    
}
