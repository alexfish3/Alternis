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
    public bool Delay;
    Rigidbody rb;
    float localX;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Right = false;
        Left = true;
        localX = transform.localScale.x;
        Delay = false;
        rb.velocity = new Vector3(-walkSpeed, 0, 0);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (transform.position.x < position1.transform.position.x && Left)
        {
            rb.velocity = new Vector3(0, 0, 0);
            Left = false;
            Right = true;
           // transform.localScale = new Vector3(-localX, transform.localScale.y, transform.localScale.z);

            StartCoroutine(Wait());
        }
        else if (transform.position.x > position2.transform.position.x && Right)
        {
            rb.velocity = new Vector3(0, 0, 0);
            Right = false;
            Left = true;
           // transform.localScale = new Vector3(localX, transform.localScale.y, transform.localScale.z);

            StartCoroutine(Wait());
        }

        IEnumerator Wait()
        {
            yield return new WaitForSeconds(3);

            if (Right) { 
            transform.localScale = new Vector3(-localX, transform.localScale.y, transform.localScale.z);
            rb.velocity = new Vector3(walkSpeed, 0, 0);
            }
            else {
                transform.localScale = new Vector3(localX, transform.localScale.y, transform.localScale.z);
                rb.velocity = new Vector3(-walkSpeed, 0, 0);
            }
        }
    }
}

