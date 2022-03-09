using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Walker : MonoBehaviour
{
    public float walkSpeed;

    public GameObject position1;
    public GameObject position2;

    public bool Right;
    public bool Left;
    Rigidbody rb;
    float localX;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Right = false;
        Left = true;
        localX = transform.localScale.x;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Right)
        {
            rb.velocity = new Vector3(walkSpeed, 0, 0);
        } else
        {
            rb.velocity = new Vector3(-walkSpeed, 0, 0);
        }

        if (transform.position.x < position1.transform.position.x)
        {
                Left = false;
                Right = true;
                transform.localScale = new Vector3(-localX, transform.localScale.y, transform.localScale.z);
            rb.velocity = new Vector3(walkSpeed, 0, 0);
        }
        else if (transform.position.x > position2.transform.position.x)
        {
            Right = false;
            Left = true;
            transform.localScale = new Vector3(localX, transform.localScale.y, transform.localScale.z);
        }
    }
}
