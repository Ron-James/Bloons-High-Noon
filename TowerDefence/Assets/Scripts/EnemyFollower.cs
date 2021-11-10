using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;

public class EnemyFollower : MonoBehaviour
{
    public PathCreator pathCreator;
    EndOfPathInstruction end; 
    [SerializeField] float speed = 10f;

    float distanceTravelled;
    [SerializeField] bool isStopped;

    Vector3 currentPositionOnPath;
    public bool IsStopped { get => isStopped; set => isStopped = value; }
    public Vector3 CurrentPositionOnPath { get => currentPositionOnPath; set => currentPositionOnPath = value; }

    // Start is called before the first frame update
    void Start()
    {
        distanceTravelled = 0;
        end = EndOfPathInstruction.Stop;
        if(GameManager.instance != null){
            pathCreator = GameManager.instance.Lane;
        }
        else{
            Debug.Log("Where GameManager");
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        if(!isStopped){
            distanceTravelled += (speed * Time.deltaTime);
            transform.position = pathCreator.path.GetPointAtDistance(distanceTravelled, end);
            if(distanceTravelled > pathCreator.path.length){
                currentPositionOnPath = pathCreator.path.GetPoint(1);
            }
            else{
                currentPositionOnPath = pathCreator.path.GetPointAtDistance(distanceTravelled, end);
            }
            

            transform.rotation = pathCreator.path.GetRotationAtDistance(distanceTravelled, end);

        }
        if(Input.GetKeyDown(KeyCode.O)){
            ResetPosition();
        }
    }

    public void ResetPosition(){
        transform.position = pathCreator.path.GetPointAtDistance(0);
        distanceTravelled = 0;
    }
}
