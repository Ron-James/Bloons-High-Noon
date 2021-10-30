using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class MeshLine : MonoBehaviour
{
    
    [SerializeField] float width = 1f;
    [SerializeField] int pointCount = 2;
    [SerializeField] Vector3 [] points = new Vector3[2];


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Vector3[] GenerateTransforms(){
        Vector3[] pointVec = new Vector3[pointCount];
        for(int loop = 0; loop < pointCount; loop++){
        
        }
        return points;
    }

    Vector3 Rotate90CW(Vector3 aDir){
        return new Vector3(aDir.z, 0, -aDir.x);
    }
    // counter clockwise
    Vector3 Rotate90CCW(Vector3 aDir){
        return new Vector3(-aDir.z, 0, aDir.x);
    }

    Mesh GeneratePlane(Vector3 p1, Vector3 p2){
        Mesh mesh = new Mesh();
        float distance = Vector3.Distance(p1, p2);
        Vector3 direction = (p2 - p1).normalized;
        float heigth = p2.y - p1.y;

        Vector3 left = Rotate90CCW(direction);
        Vector3 right = -left;

        Vector3 pt3 = p1 + (right * width/2); 
        Vector3 pt2 = p1 + (left * width/2);
        Vector3 pt1 = pt3 + (direction * distance);
        Vector3 pt0 = pt2 + (direction * distance); 



        Vector3[] verts = new Vector3[4];
        int[] tris = new int[6];

        /*
        verts[0] = new Vector3(p1.x - (width/2), p1.y + heigth, p1.z) + (distance * direction);
        verts[1] = new Vector3(p1.x + (width/2), p1.y + heigth, p1.z) + (distance * direction);
        verts[2] = new Vector3(p1.x - (width/2), p1.y, p1.z);
        verts[3] = new Vector3(p1.x + (width/2), p1.y, p1.z);
        */
        
        verts = new Vector3[]{
            pt0, pt1, pt2, pt3
        };
        
        tris = new int []{
            0, 1, 3,
            2, 1, 3
        };

        mesh.Clear();
        mesh.vertices = verts;
        mesh.triangles = tris;
        mesh.RecalculateNormals();
        return mesh;
    }
}
