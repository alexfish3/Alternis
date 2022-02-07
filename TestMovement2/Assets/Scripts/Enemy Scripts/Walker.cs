using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Walker : MonoBehaviour
{
    public float walkSpeed;

    bool Right;
    bool Left;
    Rigidbody rb;
    float localX;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Right = true;
        Left = false;
        localX = transform.localScale.x;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag != "Player")
        {
            if (Right)
            {
                Left = true;
                Right = false;
                transform.localScale = new Vector3(-localX, transform.localScale.y, transform.localScale.z);
            }
            else
            {
                Right = true;
                Left = false;
                transform.localScale = new Vector3(localX, transform.localScale.y, transform.localScale.z);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            //Kill Player
            other.gameObject.active = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Right)
        {
            rb.velocity = new Vector3(walkSpeed, 0, 0);
        } else
        {
            rb.velocity = new Vector3(-walkSpeed, 0, 0);
        }
    }
}
