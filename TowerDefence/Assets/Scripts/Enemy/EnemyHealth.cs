using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    
    [SerializeField] Enemy enemy;


    float maxHealth;
    float health;

    public float Health { get => health; set => health = value; }
    public Enemy Enemy { get => enemy; set => enemy = value; }

    // Start is called before the first frame update
    void Start()
    {
        health = enemy.health;
        maxHealth = health;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void TakeDamage(float damage){
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

    public void Revive(Vector3 position){
        if(transform.parent != GameObject.Find("Dead Enemies")){
            return;
        }
        else{
            health = maxHealth;
            this.transform.SetParent(GameObject.Find("Alive Enemies").transform);
            this.transform.position = position;
            this.gameObject.SetActive(true);
        }
    }
}
