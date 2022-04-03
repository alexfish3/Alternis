using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    Rigidbody rb;
    Animator ani;
    public bool isCrouch;
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
        if (rb.velocity.x == 0 && rb.velocity.y == 0 && !isCrouch && !this.gameObject.GetComponent<FinalMovement>().crouch)
        {
            isCrouch = false;
            ani.ResetTrigger("Walk");
            ani.ResetTrigger("Sprint");
            ani.ResetTrigger("Fall");
            ani.ResetTrigger("Jump");
            ani.ResetTrigger("Crouch");
            ani.ResetTrigger("CrouchIdle");
            ani.ResetTrigger("Uncrouch");
            ani.ResetTrigger("CrouchWalk");
            if (!ani.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
                ani.SetTrigger("Idle");
        }
        //Walk
        else if ((rb.velocity.x == 6 || rb.velocity.x == -6) && rb.velocity.y == 0 && !isCrouch)
        {
            ani.ResetTrigger("Idle");
            ani.ResetTrigger("Sprint");
            ani.ResetTrigger("Fall");
            ani.ResetTrigger("Jump");
            ani.ResetTrigger("Crouch");
            ani.ResetTrigger("CrouchIdle");
            ani.ResetTrigger("Uncrouch");
            ani.ResetTrigger("CrouchWalk");
            if (!ani.GetCurrentAnimatorStateInfo(0).IsName("Walk"))
                ani.SetTrigger("Walk");
        }
        //Sprint
        else if ((rb.velocity.x == 14 || rb.velocity.x == -14) && rb.velocity.y == 0 && !isCrouch)
        {
            ani.ResetTrigger("Walk");
            ani.ResetTrigger("Idle");
            ani.ResetTrigger("Fall");
            ani.ResetTrigger("Crouch");
            ani.ResetTrigger("CrouchIdle");
            ani.ResetTrigger("Uncrouch");
            ani.ResetTrigger("CrouchWalk");
            if (!ani.GetCurrentAnimatorStateInfo(0).IsName("Sprint"))
                ani.SetTrigger("Sprint");
        }
        //Jump
        //Check if player is not grounded
        else if (rb.velocity.y > 5)
        {
            ani.ResetTrigger("Crouch");
            ani.ResetTrigger("CrouchIdle");
            ani.ResetTrigger("Uncrouch");
            ani.ResetTrigger("CrouchWalk");
            if (!ani.GetCurrentAnimatorStateInfo(0).IsName("Jump"))
                ani.SetTrigger("Jump");
        }
        //Fall
        //Check if player is not grounded
        else if (rb.velocity.y < -2 && !this.gameObject.GetComponent<FinalMovement>().isGrounded)
        {
            ani.ResetTrigger("Walk");
            ani.ResetTrigger("Idle");
            ani.ResetTrigger("Sprint");
            ani.ResetTrigger("Jump");
            ani.ResetTrigger("Crouch");
            ani.ResetTrigger("CrouchIdle");
            ani.ResetTrigger("Uncrouch");
            if (!ani.GetCurrentAnimatorStateInfo(0).IsName("Fall"))
                ani.SetTrigger("Fall");
        }
        //Crouch
        else if (this.gameObject.GetComponent<FinalMovement>().crouch && !isCrouch)
        {
            ani.ResetTrigger("Walk");
            ani.ResetTrigger("Idle");
            ani.ResetTrigger("Sprint");
            ani.ResetTrigger("Jump");
            ani.ResetTrigger("Fall");
            ani.ResetTrigger("CrouchIdle");
            ani.ResetTrigger("Uncrouch");
            ani.ResetTrigger("Crouch");
            if (!ani.GetCurrentAnimatorStateInfo(0).IsName("Crouch"))
            {
                ani.SetTrigger("Crouch");
            }
        }
        //CrouchIdle
        else if (isCrouch && rb.velocity.x == 0 && this.gameObject.GetComponent<FinalMovement>().crouch)
        {
            ani.ResetTrigger("Walk");
            ani.ResetTrigger("Idle");
            ani.ResetTrigger("Sprint");
            ani.ResetTrigger("Jump");
            ani.ResetTrigger("Fall");
            ani.ResetTrigger("Crouch");
            ani.ResetTrigger("CrouchWalk");
            if (!ani.GetCurrentAnimatorStateInfo(0).IsName("CrouchIdle"))
                ani.SetTrigger("CrouchIdle");
        }
        //CrouchWalk
        else if (isCrouch && (rb.velocity.x > 2 || rb.velocity.x < -2) && this.gameObject.GetComponent<FinalMovement>().crouch)
        {
            ani.ResetTrigger("Walk");
            ani.ResetTrigger("Idle");
            ani.ResetTrigger("Sprint");
            ani.ResetTrigger("Jump");
            ani.ResetTrigger("Fall");
            ani.ResetTrigger("Crouch");
            ani.ResetTrigger("CrouchIdle");
            if (!ani.GetCurrentAnimatorStateInfo(0).IsName("CrouchWalk"))
                ani.SetTrigger("CrouchWalk");
        }
        //Uncrouch
        else if (!this.gameObject.GetComponent<FinalMovement>().crouch && isCrouch)
        {
            ani.ResetTrigger("Walk");
            ani.ResetTrigger("Idle");
            ani.ResetTrigger("Sprint");
            ani.ResetTrigger("Jump");
            ani.ResetTrigger("Fall");
            ani.ResetTrigger("CrouchIdle");
            ani.ResetTrigger("Crouch");
            ani.ResetTrigger("CrouchWalk");
            if (!ani.GetCurrentAnimatorStateInfo(0).IsName("Uncrouch"))
            {
                ani.SetTrigger("Uncrouch");
            }
        }

        if (ani.GetCurrentAnimatorStateInfo(0).IsName("Crouch"))
        {
            isCrouch = true;
        } else if (ani.GetCurrentAnimatorStateInfo(0).IsName("Uncrouch"))
        {
            isCrouch = false;
        }
    }
}
