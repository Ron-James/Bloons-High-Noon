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

    LineRenderer lineRenderer;
    SpringJoint joint;
    Vector3 grapplePoint;
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
        else if(Input.GetMouseButtonUp(0)){
            StropGrappling();
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

            joint.maxDistance = distance * 0.8f;
            joint.minDistance = distance * 0.25f;

            joint.spring = springConst;
            joint.damper = damper;
            joint.massScale = massScale;

            lineRenderer.positionCount = 2;

        }
    }

    IEnumerator PullIn(){
        if(!joint){
            yield return new WaitForFixedUpdate();
        }
        else{
            float time = 0;

            while(true){
                time += Time.fixedDeltaTime;
                
            }   
        }
    }
    void StropGrappling(){
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
    IEnumerator Extend(float freq, float range){
        //lineRenderer.enabled = true;
        lineRenderer.positionCount = 2;

        Vector3 startPos = firePt.position;
        Vector3 direction = ((cameraPos.position + range*Vector3.forward) - firePt.position).normalized;
        lineRenderer.SetPosition(0, startPos);
        float period = 1/freq;
        float time = 0;
        float w = freq * 2 * Mathf.PI;
        float distance = 0;
        while(true){
            time += Time.fixedDeltaTime;
            startPos = firePt.position;
            distance = Mathf.Abs(range * Mathf.Sin(w * time));

            if(time >= period /4){

                break;
            }
            else{
                endPt.localPosition = startPos + (direction * distance);
                lineRenderer.SetPosition(1, endPt.position);
                yield return new WaitForFixedUpdate();
            }

        }
    }
}
