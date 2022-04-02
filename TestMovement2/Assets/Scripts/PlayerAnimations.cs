using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    Rigidbody rb;
    Animator ani;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        ani = gameObject.GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if ((rb.velocity.x == 6 || rb.velocity.x == -6) && rb.velocity.y == 0)
        {
            // ani.SetTrigger("Walk");
            Debug.Log("Walk");
        }
    }
}
