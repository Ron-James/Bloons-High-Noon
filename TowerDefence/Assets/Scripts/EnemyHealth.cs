using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] int health;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void TakeDamage(int damage){
        health -= damage;
        if(health <= 0){
            Kill();
        }
    }

    public void Kill(){
        this.transform.SetParent(GameObject.Find("Dead Enemies").transform);
        this.transform.position = GameObject.Find("Dead Enemies").transform.position;
        this.gameObject.SetActive(false);
    }
}
