using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchLightDetect : MonoBehaviour
{
    private bool inVolume = false;

    public Material emptyMaterial;
    public Material detectedMaterial;

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            GetComponent<Renderer>().material = detectedMaterial;
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            GetComponent<Renderer>().material = emptyMaterial;
        }
    }
}
