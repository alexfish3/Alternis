using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
Author: Will Doran
Version: 2.0 (3/6/22)
Project: ALTERNIS

This class controls the searchlights' detection using a vertical cone of raycasts. The numbers are scaled such that the cone has an angle of 30 degrees.
*/

public class SearchLightDetect : MonoBehaviour
{
    [Tooltip("The distance the camera sees, measured at the center of the cone. 8.5 is approximately equivalent to the sPosal collider")]
    [SerializeField]
    private float distance;
    [Tooltip("The OriginPos empty")]
    [SerializeField]
    private GameObject sPos;
    [Tooltip("The player (for respawning purposes)")]
    [SerializeField]
    private GameObject player;
    [Tooltip("Unique numerical ID for this searchlight within the scene. Cannot be 0")]
    [SerializeField]
    public int camId;

    [Header("Rendering")]
    [Tooltip("The Triangle empty")]
    [SerializeField]
    private GameObject trianglePosition;
    [Tooltip("The Cube cube")]
    [SerializeField]
    private GameObject warningCube;

    private float y; //diameter of cone base
    private float radius; //radius of cone base
    private int rayCountPerSide; //number of rays above/below the centre line
    private int rayCountTotal; //total number of rays including centre
    private static float RAY_DIVISOR; //used for calculating rayCount

    private Vector3[] verts;
    private Vector2[] uv;
    private int[] tris;
    private int triCount;
    private int vertCount;

    void drawTriangle()
    {
        int j = 1;

        for (int i = 0; i < triCount; i += 3)
        {
            tris[i] = 0;
            tris[i + 1] = j + 1;
            tris[i + 2] = j;

            j++;

            //Debug.Log(j);
            //Debug.Log(i);
        }

        uv[0] = new Vector2(0,1);

        for (int i = 1; i < vertCount; i += 2)
        {
            uv[i] = new Vector2(0, 0);

            if (i < vertCount - 1) uv[i + 1] = new Vector2(1, 0);
        }

        Mesh area = new Mesh();
        area.vertices = verts;
        area.uv = uv;
        area.triangles = tris;
        trianglePosition.GetComponent<MeshFilter>().mesh = area;
    }

    int checkIfSeen(float distance) //checks if there is a player-tagged collider in the raycasts.
    {
        RaycastHit hit; //stores raycast return data
        int x = 0;

        if (Physics.Raycast(sPos.transform.position, sPos.transform.TransformDirection(Vector3.left), out hit, distance)) //centre line; just casts to local left
        {
            Debug.DrawLine(sPos.transform.position, hit.point, Color.red); //shows as red line in scene view
            if (hit.collider.gameObject.CompareTag("PlayerDetecter")) x = 1;
            if (hit.collider.gameObject.CompareTag("Player")) x = 2;

            verts[rayCountPerSide + 1] = trianglePosition.transform.InverseTransformPoint(hit.point);
        }
        else
        {
            verts[rayCountPerSide + 1] = trianglePosition.transform.InverseTransformPoint(sPos.transform.position + sPos.transform.TransformDirection(Vector3.left) * distance);
        }

        for (int i = 1; i < (rayCountPerSide + 1); i++) //does a number of lines above the centre based on rayCountPerSide
        {   
            //if you imagine the top half of the searchlight being a right triangle, this is the short leg (on a scale of the long leg is 1)
            float rise = (0.2679f - ((0.2679f / rayCountPerSide) * (i - 1)));
            float scaledRise = radius * rise;

            //uses rise to get direction, then pythagorean theorem to get distance (if they all were the same distance, the end of the light's cone would be curved)
            if (Physics.Raycast(sPos.transform.position, sPos.transform.TransformDirection(new Vector3(-1, rise, 0)), out hit, Mathf.Sqrt((distance * distance) + (scaledRise * scaledRise))))
            {
                //Debug.LogError("Reached");
                Debug.DrawLine(sPos.transform.position, hit.point, Color.blue); //blue line in scene view
                if (hit.collider.gameObject.CompareTag("PlayerDetecter")) x = 1;
                if (hit.collider.gameObject.CompareTag("Player")) x = 2;

                verts[i] = trianglePosition.transform.InverseTransformPoint(hit.point);
            }
            else
            {
                verts[i] = trianglePosition.transform.InverseTransformPoint(sPos.transform.position + sPos.transform.TransformDirection(new Vector3(-1, rise, 0)) * Mathf.Sqrt((distance * distance) + (scaledRise * scaledRise)));
            }
        }

        for (int i = (rayCountPerSide + 1); i < (rayCountTotal); i++)
        {
            //if you imagine the bottom half of the searchlight being a right triangle, this is the short leg (on a scale of the long leg is 1)
            float rise = ((0.2679f / rayCountPerSide) + ((0.2679f / rayCountPerSide) * (i - (rayCountPerSide + 1))));
            float scaledRise = radius * rise;

            //uses rise to get direction, then pythagorean theorem to get distance (if they all were the same distance, the end of the light's cone would be curved)
            if (Physics.Raycast(sPos.transform.position, sPos.transform.TransformDirection(new Vector3(-1, -rise, 0)), out hit, Mathf.Sqrt((distance * distance) + (scaledRise * scaledRise))))
            {
                Debug.DrawLine(sPos.transform.position, hit.point, Color.yellow); //yellow line in scene view
                if (hit.collider.gameObject.CompareTag("PlayerDetecter")) x = 1;
                if (hit.collider.gameObject.CompareTag("Player")) x = 2;

                verts[i + 1] = trianglePosition.transform.InverseTransformPoint(hit.point);
            }
            else
            {
                verts[i + 1] = trianglePosition.transform.InverseTransformPoint(sPos.transform.position + sPos.transform.TransformDirection(new Vector3(-1, -rise, 0)) * Mathf.Sqrt((distance * distance) + (scaledRise * scaledRise)));
            }
        }

        drawTriangle();
        return x;
    }

    void Start()
    {
        RAY_DIVISOR = 0.6f; //the lower this value is, the more rays there are
        y = 0.5359f * distance; //calculates the diameter of the cone's base
        radius = 0.5f * y;

        //determines the number of rays per side above the centre. the idea is that the longer the cone is, the more rays it needs to avoid something being small enough to slip between
        rayCountPerSide = (int)(y / RAY_DIVISOR);

        if (rayCountPerSide < 1) rayCountPerSide = 1; //minimum three total rays
        rayCountTotal = (rayCountPerSide * 2) + 1;

        triCount = (rayCountTotal - 1) * 3; //math works trust me
        vertCount = rayCountTotal + 1; //+1 for origin vert

        verts = new Vector3[vertCount]; 
        uv = new Vector2[vertCount];
        tris = new int[triCount];

        verts[0] = trianglePosition.transform.InverseTransformPoint(trianglePosition.transform.position); //origin vert
    }

    void Update()
    {
        int signal = checkIfSeen(distance);

        if (signal > 0) //checks every frame, respawning the player if it spots them
        {
            if (player.GetComponent<WorldSwap>().lightWorld)
            {
                if (signal == 2)
                {
                    player.GetComponent<Respawn>().DoRespawn();
                    //Debug.LogError("Reached");
                }
            }
            // else
            // {
            //     player.GetComponent<WorldSwap>().setWarning(camId, true);
            //     Debug.LogError("Reached");
            // } 
        }
        // else
        // {
        //     player.GetComponent<WorldSwap>().setWarning(camId, false);
        //     Debug.LogError("ReachF");
        // }
    }

    public GameObject getPlayer()
    {
        return player;
    }
}
