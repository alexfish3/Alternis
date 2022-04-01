using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnPointAnim : MonoBehaviour
{
    public bool reached;

    private void Update()
    {
        this.GetComponent<Animator>().SetBool("Reached", reached);
    }

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
