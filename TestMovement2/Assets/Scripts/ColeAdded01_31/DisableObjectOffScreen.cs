using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableObjectOffScreen : MonoBehaviour
{
    GameObject Player;
    float playerPos;
    float objectPos;
    int playerDistance = 70;
    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        playerPos = Player.transform.position.x;
        objectPos = this.transform.position.x;

        if (playerPos - objectPos < playerDistance && playerPos - objectPos > -playerDistance)
        {
            this.GetComponent<MeshRenderer>().enabled = true;
            this.GetComponent<Light>().enabled = true;
        } else
        {
            this.GetComponent<MeshRenderer>().enabled = false;
            this.GetComponent<Light>().enabled = false;
        }
    }
}
