using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchLightDetect : MonoBehaviour
{
    [Tooltip("The distance the camera sees, measured at the center of the cone.")]
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
        RaycastHit hit;

        if (Physics.Raycast(origin.transform.position, origin.transform.TransformDirection(Vector3.left), out hit, distance))
        {
            Debug.DrawLine(origin.transform.position, hit.point, Color.red);
            if (hit.collider.gameObject.CompareTag("Player")) return true;
        }

        for (int i = 1; i < (rayCountPerSide + 1); i++)
        {
            float rise = (0.2679f - ((0.2679f / rayCountPerSide) * (i - 1)));
            if (Physics.Raycast(origin.transform.position, origin.transform.TransformDirection(new Vector3(-1, rise, 0)), out hit, Mathf.Sqrt((distance * distance) + (rise * rise))))
            {
                //Debug.LogError("Reached");
                Debug.DrawLine(origin.transform.position, hit.point, Color.blue);
                if (hit.collider.gameObject.CompareTag("Player")) return true;
            }
        }

        for (int i = (rayCountPerSide + 1); i < (rayCountTotal); i++)
        {
            float rise = (0.2679f - ((0.2679f / rayCountPerSide) * (i - (rayCountPerSide + 1))));
            if (Physics.Raycast(origin.transform.position, origin.transform.TransformDirection(new Vector3(-1, -rise, 0)), out hit, Mathf.Sqrt((distance * distance) + (rise * rise))))
            {
                Debug.DrawLine(origin.transform.position, hit.point, Color.yellow);
                if (hit.collider.gameObject.CompareTag("Player")) return true;
            }
        }

        return false;
    }

    void Start()
    {
        RAY_DIVISOR = 3.0f;
        y = 0.5359f * distance;

        rayCountPerSide = (int)(y / RAY_DIVISOR);
        if (rayCountPerSide < 1) rayCountPerSide = 1;
        rayCountTotal = (rayCountPerSide * 2) + 1;
    }

    void Update()
    {
        if (checkIfSeen(distance))
        {
            GetComponent<Renderer>().material = detectedMaterial;
            Debug.LogError("Reached");
            player.GetComponent<Respawn>().DoRespawn();
        }
        else
        {
            GetComponent<Renderer>().material = emptyMaterial;
        }
    }
}
