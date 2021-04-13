using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(MeshFilter))]
public class MattGen : MonoBehaviour
{
    Mesh mesh;
    public int xsize=20;
    public int zsize=20;

    //adding Random mode
    public bool random;

    //added Extreme lvl
    
    public int extremelvl=2;

    //vertices are kinda cordinate points
    Vector3[] vertices;

    
    int[] triangles;
    Vector2[] uvs;
    // Start is called before the first frame update
    void Start()
    {
        if (random)
        {
            extremelvl=Random.Range(2,5);
        }
        mesh=new Mesh();
        GetComponent<MeshFilter>().mesh=mesh;
        CreateShape();
        UpdateMesh();

    }
    void CreateShape(){
       vertices=new Vector3[(xsize+1)*(zsize+1)];
       
       for (int i =0,  z = 0; z <= zsize; z++)
       {
           for (int x = 0; x <= xsize; x++)
           {
               float y = Mathf.PerlinNoise(x*.3f,z*.3f)*extremelvl;
               vertices[i]=new Vector3(x,y,z);
               i++;
           }
       }

           

        triangles=new int[(xsize*zsize*6)];
       //ny række
        int vert=0;
        //
        int tris=0;
        for (int z = 0; z < zsize; z++)
        {
        for (int x = 0; x < xsize; x++)
        {
       triangles[tris+0]=vert+0;
       triangles[tris+1]=vert+xsize+1;
       triangles[tris+2]=vert+1;
       triangles[tris+3]=vert+1;
       triangles[tris+4]=vert+xsize+1;
       triangles[tris+5]=vert+xsize+2;
       vert++;
       tris += 6;
        } 
        vert++;   
        }
        
       uvs=new Vector2[vertices.Length];
       for (int i =0,  z = 0; z <= zsize; z++)
       {
           for (int x = 0; x <= xsize; x++)
           {
               uvs[i]=new Vector2((float)x/xsize,(float)z/zsize);
               i++;
           }
       }
    }
    void UpdateMesh(){
        mesh.Clear();
        mesh.vertices=vertices;
        mesh.triangles=triangles;
        mesh.uv=uvs;

        mesh.RecalculateNormals();
        

        //adding collider
        mesh.RecalculateBounds();
        MeshCollider meshCollider = gameObject.GetComponent<MeshCollider>();
        meshCollider.sharedMesh = mesh;
    }

    //only using this because its a good way to see what it is doing
     private void OnDrawGizmos() {
     if(vertices==null)
     return;


     for (int i = 0; i < vertices.Length; i++)
     {
         Gizmos.DrawSphere(vertices[i],.1f);
     }
    }
}
