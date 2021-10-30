using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class MeshLine : MonoBehaviour
{
    
    [SerializeField] float width = 1f;


    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    Mesh GeneratePlane(Vector3 p1, Vector3 p2){
        Mesh mesh = new Mesh();
        float distance = Vector3.Distance(p1, p2);
        float heigth = p2.y - p1.y;
        Vector3[] verts = new Vector3[4];
        int[] tris = new int[6];

        verts = new Vector3 []{
            new Vector3(p1.x - (width/2), heigth, p1.z + distance),
            new Vector3(p1.x + (width/2), heigth, p1.z + distance),
            new Vector3(p1.x - (width/2), p1.y, p1.z),
            new Vector3(p1.x + (width/2), p1.y, p1.z)
        };
        
        tris = new int []{
            0, 1, 3,
            2, 1, 3
        };

        mesh.Clear();
        mesh.vertices = verts;
        mesh.triangles = tris;
        return mesh;
        
        
    }
}
