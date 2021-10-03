using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Island : MonoBehaviour
{
    int index;
    [SerializeField] GameObject enemies;
    [SerializeField] Transform turretPosition;

    Lane lane;

    public int Index { get => index; set => index = value; }
    public GameObject Enemies { get => enemies; set => enemies = value; }
    public Lane Lane { get => lane; set => lane = value; }

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
            case "Enemy":
                //other.gameObject.transform.SetParent(enemies.transform);
                Debug.Log("triggered enemy");
            break;
        }
    }
}
