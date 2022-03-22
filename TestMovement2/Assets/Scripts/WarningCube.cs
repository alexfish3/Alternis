using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarningCube : MonoBehaviour
{
    [SerializeField]
    private GameObject cam;

    private int camId;
    private GameObject player;

    void Start()
    {
        camId = cam.GetComponent<SearchLightDetect>().camId;
        player = cam.GetComponent<SearchLightDetect>().getPlayer();
    }

    void OnTriggerEnter (Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player.GetComponent<WorldSwap>().setWarning(camId, true);
        }
    }

    void OnTriggerExit (Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player.GetComponent<WorldSwap>().setWarning(camId, false);
        }
    }
}
