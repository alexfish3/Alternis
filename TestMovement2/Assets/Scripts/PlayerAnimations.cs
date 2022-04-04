using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    Rigidbody rb;
    CapsuleCollider c, unc;
    Animator ani;
    public bool isCrouch;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        ani = gameObject.GetComponentInChildren<Animator>();
        c = this.gameObject.GetComponent<CapsuleCollider>();
        unc = this.gameObject.GetComponent<CapsuleCollider>();
        c.center = new Vector3(0.009f, -0.53f, 0);
        c.radius = 0.569f;
        c.height = 1.844f;
        unc.center = new Vector3(-0.00898f, -0.04421f, 0);
        unc.radius = 0.569f;
        unc.height = 2.815f;

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
        if (ani.GetCurrentAnimatorStateInfo(0).IsName("Death") || ani.GetCurrentAnimatorStateInfo(0).IsName("Caught") && this.gameObject.GetComponent<Respawn>().death)
        {
            ani.ResetTrigger("Walk");
            ani.ResetTrigger("Sprint");
            ani.ResetTrigger("Fall");
            ani.ResetTrigger("Jump");
            ani.ResetTrigger("Crouch");
            ani.ResetTrigger("CrouchIdle");
            ani.ResetTrigger("Uncrouch");
            ani.ResetTrigger("CrouchWalk");
        }
        else if (rb.velocity.x < 1 && rb.velocity.x > -1 && rb.velocity.y < 1 && rb.velocity.y > -1 && !isCrouch && !this.gameObject.GetComponent<FinalMovement>().crouch && this.gameObject.GetComponent<FinalMovement>().isGrounded)
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
            else if (ani.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
            {
                rb.velocity = new Vector3(0, 0, 0);
            }
        }
        //Walk
        else if (((rb.velocity.x >= 1 || rb.velocity.x <= -1) && (rb.velocity.x < 8 && rb.velocity.x > -8)) && !isCrouch && this.gameObject.GetComponent<FinalMovement>().isGrounded)
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
        else if ((rb.velocity.x == 14 || rb.velocity.x == -14) && this.gameObject.GetComponent<FinalMovement>().isGrounded && !isCrouch)
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
            ani.ResetTrigger("Idle");
            ani.ResetTrigger("Sprint");
            ani.ResetTrigger("Fall");
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

        if (ani.GetCurrentAnimatorStateInfo(0).IsName("Crouch"))
        {
            this.gameObject.GetComponent<CapsuleCollider>().center = new Vector3(-0.009f, -0.529f, 0);
            this.gameObject.GetComponent<CapsuleCollider>().height = 1.844f;
            this.gameObject.GetComponent<CapsuleCollider>().radius = 0.5689f;
        } else if (ani.GetCurrentAnimatorStateInfo(0).IsName("Uncrouch"))
        {
            this.gameObject.GetComponent<CapsuleCollider>().center = new Vector3(-0.009f, -0.04421f, 0);
            this.gameObject.GetComponent<CapsuleCollider>().height = 2.815f;
            this.gameObject.GetComponent<CapsuleCollider>().radius = 0.5689f;
        }
    }
}
