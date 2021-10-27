using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [Header("Spring Force Constants")]
    [SerializeField] float range;
    [SerializeField] float springSpd = 0.6f;

    [SerializeField] Transform fwdPt;
    [SerializeField] Transform backPt;
    [SerializeField] Rigidbody playerRb;

    [Header("Spring Force Constants")]
    [SerializeField] GameObject hook;

    public bool fired = false;
    public bool collided = false;
    float velocity;
    Coroutine extension = null;
    [SerializeField] Vector3 sphereCastOffset = new Vector3(0,1,0);


    

    // Start is called before the first frame update
    void Start()
    {
        collided = false;
        fired = false;
  
    }
    private void OnDisable() {
        StopAllCoroutines();
    }
    // Update is called once per frame
    void Update()
    {
        
        //Debug.DrawRay(hook.transform.position, (fwdPt.position - hook.transform.position).normalized, Color.red, 0.2f);
        if(Input.GetMouseButtonDown(0) && extension == null){
            extension = StartCoroutine(Extend(springSpd, range));
            
            
        }
    }

    public bool getCollided(){
        return collided;
    }
    public bool getFired(){
        return fired;
    }
    public float getVelocity(){
        return velocity;
    }

    public void setCollided(bool coll){
        collided = coll;
    }
    IEnumerator Extend(float period, float amplitude){
        fired = true;
        float w = (1/period) * 2 * Mathf.PI;
        float time = 0;
        Vector3 startPos = hook.transform.localPosition;
        collided = false;
        velocity = 0;
        while(true){
            time += Time.fixedDeltaTime;
            float d = Mathf.Abs(amplitude * Mathf.Sin(w * time));
            velocity = Mathf.Cos(w * time);
            Debug.Log(amplitude);
            if(time >= period/2 || d < 0.1f){
                hook.transform.localPosition = startPos;
                extension = null;
                fired = false;
                velocity = 0;
                
                break;
            }
            else{
                
                hook.transform.localPosition = startPos + Vector3.forward * d;
                yield return new WaitForFixedUpdate();
            }
        }

    }

}
