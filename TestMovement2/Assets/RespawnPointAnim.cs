using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnPointAnim : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            this.GetComponent<Animator>().SetTrigger("Shrink");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            this.GetComponent<Animator>().SetTrigger("Grow");
        }
    }
}
