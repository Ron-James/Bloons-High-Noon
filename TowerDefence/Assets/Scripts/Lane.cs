using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lane : MonoBehaviour
{
    [SerializeField] int index;
    [SerializeField] int length = 3;
    [SerializeField] float cellWidth = 18f;
    [SerializeField] float cellLength = 5f;
    [SerializeField] GameObject islandPrefab;
    [SerializeField] GameObject basicEnemy;
    [SerializeField] Transform endPoint;
    [SerializeField] Transform spawnPointMin;
    [SerializeField] Transform spawnPointMax;
    

    [SerializeField] Island [] islands;

    

    public Transform EndPoint { get => endPoint; set => endPoint = value; }
    public int Length { get => length; set => length = value; }

    [ExecuteInEditMode()]
    // Start is called before the first frame update
    void Start()
    {
        length = islands.Length;
        for(int loop = 0; loop < islands.Length; loop++){
            islands[loop].Index = loop;
        }
    }

    

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SpawnEnemy(GameObject enemy){
        Vector3 position;
        position.x = spawnPointMax.position.x;
        position.y = spawnPointMax.position.y;
        position.z = Random.Range(spawnPointMin.position.z, spawnPointMax.position.z);

        GameObject go = Instantiate(enemy, position, Quaternion.identity);
        

    }
    
    public Island GetIslandAt(int index){
        if(index < 0 || index > islands.Length){
            Debug.Log("can't get island, index out of bounds");
            return null;
        }
        else{
            return islands[index];
        }
    }
}
