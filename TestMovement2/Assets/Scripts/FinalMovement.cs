using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalMovement : MonoBehaviour
{
    public float walkSpeed;
    public float friction;
    public float maxGravity;
    public float jumpHeight;
    public float ffSpeed;
    public float sprintSpeed;
    public float slideSpeed;
    public float crouchWalkSpeed;
    public float controllerSense;

    float jumpSpeed;
    float airSpeed;
    float curwalkSpeed;
    float idleTime;
    float jumpCount;
    bool isSprinting;
    bool isRight = false;
    bool isLeft = false;
    bool isJumping;
    bool walking;
    bool doubleJump;
    bool crouch;
    public bool slide;
    float localX;
    public bool isGrounded;
    Vector3 move;
    Rigidbody rb;

    //Check If On Ground
    private void OnCollisionStay(Collision other)
    {
        if (other.gameObject.tag == "Ground")
        {
            isGrounded = true;
        }
        else { isGrounded = false; }
    }

    //Check if in Air
    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject.tag == "Ground")
        {
            isGrounded = false;
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        localX = transform.localScale.x;
        crouch = false;
        slide = false;
        jumpCount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        //Walk
        if (Input.GetAxis("Horizontal") > controllerSense && !crouch)
        {
            walking = true;
            idleTime = 0;
            rb.velocity = new Vector3(curwalkSpeed, rb.velocity.y, 0);
        }
        else if (Input.GetAxis("Horizontal") < -controllerSense && !crouch)
        {
            walking = true;
            idleTime = 0;
            rb.velocity = new Vector3(-curwalkSpeed, rb.velocity.y, 0);
        }
        
        //Idle time so players can hold momentum when changing directions
        if (Input.GetAxis("Horizontal") == 0 && walking)
        {
            idleTime++;
        }
        if (!crouch && idleTime > 10)
        {
            walking = false;
            idleTime = 0;
        } else if (Input.GetAxis("Horizontal") < controllerSense && Input.GetAxis("Horizontal") > -controllerSense && !isGrounded)
        {
            walking = false;
            idleTime = 0;
        }


        //Jump
        if (Input.GetButtonDown("Jump") && jumpSpeed <= 7 && jumpCount < 2 && !slide)
        {
            jumpCount++;

            rb.velocity = new Vector3(rb.velocity.x, jumpSpeed, 0);

            jumpSpeed = jumpHeight;
            isJumping = true;
        }

        //Sprint
        if (Input.GetButtonDown("Dash") && walking && !crouch && isGrounded)
        {
            isSprinting = true;
            airSpeed = sprintSpeed;
            walking = false;
        }
        if (rb.velocity.x > 0 && isSprinting) //facing right
        {
            rb.velocity = new Vector3(sprintSpeed, rb.velocity.y, 0);
        }
        if (rb.velocity.x < 0 && isSprinting) // facing left
        {
            rb.velocity = new Vector3(-sprintSpeed, rb.velocity.y, 0);
        }

        if (Input.GetAxis("Horizontal") < 0.4 && Input.GetAxis("Horizontal") > -0.4)
        {
            isSprinting = false;
        }

        //Sprint Jump
        if (isSprinting && !isGrounded && (rb.velocity.x > walkSpeed || rb.velocity.x < -walkSpeed) && !crouch)
        {
            if (isRight)
            {
                rb.velocity = new Vector3(airSpeed, rb.velocity.y, 0);
            }
            if (isLeft)
            {
                rb.velocity = new Vector3(-airSpeed, rb.velocity.y, 0);
            }
        } else if (rb.velocity.x <= walkSpeed && rb.velocity.x >= -walkSpeed && isSprinting)
        {
            isSprinting = false;
        }
        if (airSpeed <= walkSpeed)
        {
            isSprinting = false;
        }

        //Right & Left
        if (Input.GetAxis("Horizontal") > controllerSense)
        {
            isRight = true;
            isLeft = false;
        }
        if (Input.GetAxis("Horizontal") < -controllerSense)
        {
            isRight = false;
            isLeft = true;
        }

        //Crouch
        if (Input.GetAxis("Vertical") < -0.3)
        {
            crouch = true;
            transform.localScale = new Vector3(transform.localScale.x, 0.5f, transform.localScale.z);
        } else
        {
            crouch = false;
            transform.localScale = new Vector3(transform.localScale.x, 1f, transform.localScale.z);
        }

        //Crouch Walk
        if (Input.GetAxis("Horizontal") > controllerSense && crouch && !slide)
        {
            isSprinting = false;
            rb.velocity = new Vector3(crouchWalkSpeed, rb.velocity.y, 0);
        }
        else if (Input.GetAxis("Horizontal") < -controllerSense && crouch && !slide)
        {
            isSprinting = false;
            rb.velocity = new Vector3(-crouchWalkSpeed, rb.velocity.y, 0);
        }

        //Slide
        //if (crouch && Input.GetAxis("Horizontal") > 0.2 && Input.GetButtonDown("Dash") && !slide)
       // {
       //     slide = true;
        //    rb.velocity = new Vector3(slideSpeed, rb.velocity.y, 0);
       // } else if (crouch && Input.GetAxis("Horizontal") < -0.2 && Input.GetButtonDown("Dash") && !slide)
      //  {
      //      slide = true;
      //      rb.velocity = new Vector3(-slideSpeed, rb.velocity.y, 0);
      //  }
      //  if (slide && rb.velocity.x < 2 && rb.velocity.x > -2)
      //  {
       //     slide = false;
      //  }

        //Disable Sprint
        if (rb.velocity.x <= walkSpeed && rb.velocity.x >= -walkSpeed && isSprinting)
        {
            isSprinting = false;
        }

        //Reset Current Walk Speed
        if (!walking && !isSprinting)
        {
            curwalkSpeed = 0;
        }


        //Sprite Change Direction
        if (isRight && !slide)
        {
            transform.localScale = new Vector3(localX, transform.localScale.y, transform.localScale.z);
        }
        else if (isLeft && !slide)
        {
            transform.localScale = new Vector3(-localX, transform.localScale.y, transform.localScale.z);
        }
    }

    void FixedUpdate()
    {
        //Walk Speed Increase
        if (curwalkSpeed < walkSpeed && walking)
        {
            curwalkSpeed += 0.5f;
        }

        //Gravity
        if (!isGrounded && rb.velocity.y > maxGravity)
        {
            rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y - 2, 0);
        } else if (!isGrounded && rb.velocity.y <= maxGravity)
        {
            rb.velocity = new Vector3(rb.velocity.x, maxGravity, 0);
        } else if (isGrounded)
        {
            rb.velocity = new Vector3(rb.velocity.x, 0, 0);
        }

        //FastFall
        if (!isGrounded && Input.GetAxis("Vertical") < -0.4)
        {
            rb.velocity = new Vector3(rb.velocity.x, ffSpeed, 0);
        }

        //Friction
        if (rb.velocity.x > friction)
            rb.velocity = new Vector3(rb.velocity.x-friction, rb.velocity.y, 0);
        else if (rb.velocity.x < -friction)
            rb.velocity = new Vector3(rb.velocity.x+friction, rb.velocity.y, 0);
        else
            rb.velocity = new Vector3(0, rb.velocity.y, 0);


        //Reduce Air Sprint Speed
        if (isSprinting && !isGrounded && airSpeed > walkSpeed)
        {
            airSpeed -= 0.25f;
        }

        //Jumping Height
        if (isJumping)
        {
            rb.velocity = new Vector3(rb.velocity.x, jumpSpeed, 0);

            jumpSpeed--;
            if (jumpSpeed <= 0) // how soon you can double jump after jumping once
            {
                isJumping = false;
            }
        }
        //Reset Double Jump
        if (isGrounded && !isJumping)
            jumpCount = 0;
    }
}
