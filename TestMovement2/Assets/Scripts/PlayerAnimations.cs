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

    void ResetTriggers()
    {
        ani.ResetTrigger("Walk");
        ani.SetTrigger("Sprint");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Idle
        if (rb.velocity.x == 0 && rb.velocity.y == 0)
        {
            ani.ResetTrigger("Walk");
            ani.ResetTrigger("Sprint");
            if (!ani.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
                ani.SetTrigger("Idle");
        }
        //Walk
        else if ((rb.velocity.x == 6 || rb.velocity.x == -6) && rb.velocity.y == 0)
        {
            ani.ResetTrigger("Idle");
            ani.ResetTrigger("Sprint");
            if (!ani.GetCurrentAnimatorStateInfo(0).IsName("Walk"))
             ani.SetTrigger("Walk");
        }
        //Sprint
        else if ((rb.velocity.x == 14 || rb.velocity.x == -14) && rb.velocity.y == 0)
        {
            ani.ResetTrigger("Walk");
            ani.ResetTrigger("Idle");
            if (!ani.GetCurrentAnimatorStateInfo(0).IsName("Sprint"))
                ani.SetTrigger("Sprint");
        }
        else
        {

        }
    }
}
