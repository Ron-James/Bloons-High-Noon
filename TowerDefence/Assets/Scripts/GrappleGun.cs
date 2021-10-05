using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrappleGun : MonoBehaviour
{
    [SerializeField] Transform firePt;
    [SerializeField] Transform endPt;
    [SerializeField] GameObject player;
    [SerializeField] Transform cameraPos;
    [SerializeField] LayerMask canGrapple;


    [Header("Gun Range and Fire Speed")]
    [SerializeField] float range = 15f; // range of grapple gun 
    [SerializeField] float shootSpeed = 5f; // speed of rope extension 

    [Header("Rope Constants")]
    [SerializeField] float springConst = 5f;
    [SerializeField] float damper = 7f;
    [SerializeField] float massScale = 4.5f;
    [SerializeField] float pullInRate = 0.1f;

    LineRenderer lineRenderer;
    SpringJoint joint;
    Vector3 grapplePoint;

    public SpringJoint Joint { get => joint; set => joint = value; }

    // Start is called before the first frame update
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0)){
            Grapple();
        }
        if(Input.GetMouseButtonDown(1) && joint != null){
            StartCoroutine(PullIn(pullInRate));
        }
        
        
    }

    private void LateUpdate() {
        DrawRope();
    }

    void Grapple(){
        RaycastHit hit;
        if(Physics.Raycast(cameraPos.position, cameraPos.forward, out hit, range, canGrapple)){
            grapplePoint = hit.point;
            joint = player.gameObject.AddComponent<SpringJoint>();
            joint.autoConfigureConnectedAnchor = false;
            joint.connectedAnchor = grapplePoint;

            float distance = Vector3.Distance(player.transform.position, grapplePoint);

            joint.maxDistance = distance * 0.95f;
            joint.minDistance = distance * 0.01f;

            joint.spring = springConst;
            joint.damper = damper;
            joint.massScale = massScale;

            lineRenderer.positionCount = 2;
            

        }
    }

    IEnumerator PullIn(float rate){
        if(!joint){
            yield return new WaitForFixedUpdate();   
        }
        else{
            while(true){
                joint.maxDistance -= rate;
                if(joint.maxDistance <= joint.minDistance){
                    joint.maxDistance = joint.minDistance;
                    StropGrappling();
                    break;
                }
                else{
                    yield return new WaitForFixedUpdate();
                }
            }   
        }
    }
    public void StropGrappling(){
        lineRenderer.positionCount = 0;
        Destroy(joint);
    }
    void DrawRope(){
        if(!joint){
            return;
        }
        lineRenderer.SetPosition(0, firePt.position);
        lineRenderer.SetPosition(1, grapplePoint);
    }
    
}
