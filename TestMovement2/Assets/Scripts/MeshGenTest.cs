using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshGenTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Mesh x = new Mesh();

        Vector3[] verts = new Vector3[3];
        Vector2[] uv = new Vector2[3];
        int[] tris = new int[3];

        verts[0] = new Vector3(0,0,50);
        verts[1] = new Vector3(0,100,0);
        verts[2] = new Vector3(100,100,0);

        uv[0] = new Vector2(0,0);
        uv[1] = new Vector2(0,4);
        uv[2] = new Vector2(4,4);

        tris[0] = 0;
        tris[1] = 1;
        tris[2] = 2;

        x.vertices = verts;
        x.uv = uv;
        x.triangles = tris;

        GetComponent<MeshFilter>().mesh = x;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
