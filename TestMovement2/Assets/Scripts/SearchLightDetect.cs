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
    [Tooltip("The distance the camera sees, measured at the center of the cone. 8.5 is approximately equivalent to the original collider")]
    [SerializeField]
    private float distance;
    [Tooltip("The OriginPos empty")]
    [SerializeField]
    private GameObject origin;
    [Tooltip("The player (for respawning purposes)")]
    [SerializeField]
    private GameObject player;

    private float y; //diameter of cone base
    private int rayCountPerSide; //number of rays above/below the centre line
    private int rayCountTotal; //total number of rays including centre
    private static float RAY_DIVISOR; //used for calculating rayCount

    public Material emptyMaterial;
    public Material detectedMaterial;

    bool checkIfSeen(float distance) //checks if there is a player-tagged collider in the raycasts.
    {
        RaycastHit hit; //stores raycast return data

        if (Physics.Raycast(origin.transform.position, origin.transform.TransformDirection(Vector3.left), out hit, distance)) //centre line; just casts to local left
        {
            Debug.DrawLine(origin.transform.position, hit.point, Color.red); //shows as red line in scene view
            if (hit.collider.gameObject.CompareTag("Player")) return true;
        }

        for (int i = 1; i < (rayCountPerSide + 1); i++) //does a number of lines above the centre based on rayCountPerSide
        {   
            //if you imagine the top half of the searchlight being a right triangle, this is the short leg (on a scale of the long leg is 1)
            float rise = (0.2679f - ((0.2679f / rayCountPerSide) * (i - 1)));

            //uses rise to get direction, then pythagorean theorem to get distance (if they all were the same distance, the end of the light's cone would be curved)
            if (Physics.Raycast(origin.transform.position, origin.transform.TransformDirection(new Vector3(-1, rise, 0)), out hit, Mathf.Sqrt((distance * distance) + (rise * rise))))
            {
                //Debug.LogError("Reached");
                Debug.DrawLine(origin.transform.position, hit.point, Color.blue); //blue line in scene view
                if (hit.collider.gameObject.CompareTag("Player")) return true;
            }
        }

        for (int i = (rayCountPerSide + 1); i < (rayCountTotal); i++)
        {
            //if you imagine the bottom half of the searchlight being a right triangle, this is the short leg (on a scale of the long leg is 1)
            float rise = (0.2679f - ((0.2679f / rayCountPerSide) * (i - (rayCountPerSide + 1))));

            //uses rise to get direction, then pythagorean theorem to get distance (if they all were the same distance, the end of the light's cone would be curved)
            if (Physics.Raycast(origin.transform.position, origin.transform.TransformDirection(new Vector3(-1, -rise, 0)), out hit, Mathf.Sqrt((distance * distance) + (rise * rise))))
            {
                Debug.DrawLine(origin.transform.position, hit.point, Color.yellow); //yellow line in scene view
                if (hit.collider.gameObject.CompareTag("Player")) return true;
            }
        }

        return false; //none of the rays have hit anything
    }

    void Start()
    {
        RAY_DIVISOR = 2.5f; //the lower this value is, the more rays there are
        y = 0.5359f * distance; //calculates the diameter of the cone's base

        //determines the number of rays per side above the centre. the idea is that the longer the cone is, the more rays it needs to avoid something being small enough to slip between
        rayCountPerSide = (int)(y / RAY_DIVISOR);

        if (rayCountPerSide < 1) rayCountPerSide = 1; //minimum three total rays
        rayCountTotal = (rayCountPerSide * 2) + 1;
    }

    void Update()
    {
        if (checkIfSeen(distance)) //checks every frame, respawning the player if it spots them
        {
            player.GetComponent<Respawn>().DoRespawn();
            GetComponent<Renderer>().material = detectedMaterial;
            //Debug.LogError("Reached");
        }
        else
        {
            GetComponent<Renderer>().material = emptyMaterial;
        }
    }
}
