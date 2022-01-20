using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdatedMovement : MonoBehaviour
{
    // Start is called before the first frame update

    public Material crouchM;
    public Material defaultM;
    public Material ffM;
    public Material jumpM;
    public Material slideM;
    public Material SprintM;

    public float speed = 1f;
    public float maxSpeed = 25f;
    public float dashSpeed = 20f;
    public float gravity = -9.0f;
    public float jumpHeight = 1f;
    public int jumpMaxCount = 2;
    public float airDrag = 0.5f;
    public float fastFallSpeed = -5f;
    public int dashDelay = 120;
    public float slideSpeed = 10;
    public float slideLength = 0.5f;
    Rigidbody rb;
    Vector3 move;
    public bool isGrounded;
    bool dash;
    bool dashRecovery;
    int jumpCount;
    int dashTimer;
    Vector3 tempScale;
    public bool crouch;
    bool slide;
    float slideMomentum;

    private void OnCollisionStay(Collision other)
    {
        if (other.gameObject.tag == "Ground")
        {
            isGrounded = true;
            jumpCount = 0;
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


    void Start()
    {
        Vector3 tempScale = transform.localScale;
        dash = false;
        slide = false;
        rb = GetComponent<Rigidbody>();
        move = new Vector3(0, 0, 0);
    }


    void Update()
    {


        //Material Change
        if (isGrounded && Input.GetAxis("Horizontal") == 0)
            GetComponent<Renderer>().material = defaultM;
        else if (!isGrounded)
        {
            GetComponent<Renderer>().material = jumpM;
        }


        //Dash
        if (Input.GetAxis("Horizontal") != 0)
        {
            if (Input.GetButtonDown("Dash") && isGrounded && !crouch && dashRecovery == false && !crouch)
            {
                dash = true;
                GetComponent<Renderer>().material = SprintM;
            }
        }
        //Walking
        if (Input.GetAxis("Horizontal") != 0 && rb.velocity.x < maxSpeed && rb.velocity.x > -maxSpeed)
        {
            move.x = Input.GetAxis("Horizontal") * speed;

        }


        //Jump
        if (Input.GetButtonDown("Jump") && isGrounded == true)
        {
            move.y = jumpHeight;
            jumpCount++;
        }
        //DoubleJump
        if (Input.GetButtonDown("Jump") && isGrounded == false && jumpCount < jumpMaxCount && dashRecovery == false)
        {
            if (rb.velocity.y > 0)
            {
                move.y = jumpHeight;
            }
            else
            {
                move.y = jumpHeight + (rb.velocity.y * -1);
            }
            jumpCount++;
        }

        //Fast Falling
        if (Input.GetAxis("Vertical") < 0 && !isGrounded)
        {
            GetComponent<Renderer>().material = ffM;
            move.y = fastFallSpeed;
        }


        //Crouching
        if (isGrounded && !dashRecovery && !dash && Input.GetAxis("Vertical") < -0.3 && (Input.GetAxis("Horizontal") < 0.75 && Input.GetAxis("Horizontal") > -0.75))
        {
            if (slide == false)
                GetComponent<Renderer>().material = crouchM;
            crouch = true;
        } else if ((crouch && isGrounded) && Input.GetAxis("Vertical") >= -0.3 || Input.GetAxis("Horizontal") > 0.75 || Input.GetAxis("Horizontal") < -0.75)
        {
            if (crouch)
                GetComponent<Renderer>().material = defaultM;
            crouch = false;
        }

        //Slide
        if (crouch && Input.GetButtonDown("Dash") && Input.GetAxis("Horizontal") != 0 && slide == false)
        {
            slide = true;
            GetComponent<Renderer>().material = slideM;
            slideMomentum = slideSpeed;
            if (Input.GetAxis("Horizontal") > 0)
            {
                move.x += slideSpeed;
            }
            else if (Input.GetAxis("Horizontal") < 0)
            {
                move.x -= slideSpeed;
            }
        }
        //Slide Reset
        if (slide)
        {
            if (slideMomentum <= 0)
            {
                slideMomentum = slideSpeed;
                slide = false;
                GetComponent<Renderer>().material = defaultM;
                crouch = false;
            }
        }

        Debug.Log(slideMomentum);
        Debug.Log(slide);

        //Gravity
        if (rb.velocity.y > -20)
        {
            move.y += gravity * Time.deltaTime;
        }

        //Aerial Drag
        if (isGrounded == false)
        {
            if (rb.velocity.x > airDrag)
            {
                rb.velocity += new Vector3(-airDrag, 0, 0);
            }
            else if (rb.velocity.x < -airDrag)
            {
                rb.velocity += new Vector3(airDrag, 0, 0);
            }
        }


        //Dash
        if (dash == true && move.y < 1)
        {
            GetComponent<Renderer>().material = SprintM;
            if (rb.velocity.x > 1 && rb.velocity.x < maxSpeed && Input.GetAxis("Horizontal") > 0)
            {
                move.x = dashSpeed;
                GetComponent<Renderer>().material = SprintM;
            }
            else if (rb.velocity.x < -1 && rb.velocity.x > -maxSpeed && Input.GetAxis("Horizontal") < 0)
            {
                move.x = -dashSpeed;
                GetComponent<Renderer>().material = SprintM;
            }
            dashRecovery = true;
            dash = false;
            dashTimer = 0;
        }
        //Dash Maintain Velocity (Dash dance)
        if (dashRecovery == true && Input.GetAxis("Horizontal") != 0)
        {
            if (rb.velocity.x > 0 && rb.velocity.x < dashSpeed && Input.GetAxis("Horizontal") > 0)
            {
                move.x = dashSpeed;
                GetComponent<Renderer>().material = SprintM;
            }
            else if (rb.velocity.x < 0 && rb.velocity.x > -dashSpeed && Input.GetAxis("Horizontal") < 0)
            {
                move.x = -dashSpeed;
                GetComponent<Renderer>().material = SprintM;
            }
        }

        //Dash Recovery Frames
        if (dashRecovery == true)
        {
            dashTimer++;
            if (dashTimer > dashDelay)
            {
                if (move.x > 0 && Input.GetAxis("Horizontal") < 0)
                {
                    dashRecovery = false;
                }
                else if (move.x < 0 && Input.GetAxis("Horizontal") > 0)
                {
                    dashRecovery = false;
                }
                else if (Input.GetAxis("Horizontal") == 0 || rb.velocity.y > 0)
                {
                    dashRecovery = false;
                }
            }
            else if (rb.velocity.y > 0)
            {
                dashRecovery = false;
            }
        }

        //Crouch Speed
        if (crouch)
        {
            move.x = move.x / 1.75f;
        }
    }

    void FixedUpdate()
    {
        //SlideLength
        if (slide)
        {
            if (Input.GetAxis("Horizontal") > 0 && slideMomentum > 0)
            {
                move.x += slideMomentum;
                slideMomentum -= slideLength;
            }
            else if (Input.GetAxis("Horizontal") < 0 && slideMomentum > 0)
            {
                move.x -= slideMomentum;
                slideMomentum -= slideLength;
            }
        }

        rb.velocity += move;

        move = new Vector3(0, 0, 0);
    }
}
