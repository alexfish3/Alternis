using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public bool isGrounded;
    private int dashCoolCount = 0;
    public int dashCooldown = 20;
    private bool doubleJump = false;
    public float dashSpeed = 5f;
    public float doubleJumpBoost = 1.25f;
    public float speed = 200;
    public float ffSpeed = 0.1f;
    public float jumpHeight = 100;
    public float gravity = -9.0f;
    public float groundFriction = 0.1f;
    public float airFriction = 0.3f;
    public float maxGroundVel = 8f;
    public float maxAirVel = 6f;
    Vector3 move;
    Rigidbody rb;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    //Check if Grounded
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Ground")
        {
            isGrounded = true;
        }
        else { isGrounded = false; }
    }

    //Check if in Air
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Ground")
        {
            isGrounded = false;
        }
    }

    void FixedUpdate()
    {
        //rb.AddForce(move * speed * Time.deltaTime);
        //Set Move to Real Velocity

        Debug.Log(dashCoolCount);
        if (dashCoolCount > 0)
        {
            dashCoolCount--;
        }
        rb.velocity = new Vector3(move.x * speed * Time.deltaTime, move.y * jumpHeight * Time.deltaTime, 0);
    }

    void Update()
    {

        //Grounded Movement
        if (isGrounded == true)
        {
            Debug.Log("Im Grounded");
            doubleJump = false;
            //Ground Jump
            if (Input.GetKeyDown("space"))
            {
                move.y = 1 * jumpHeight;
            }
            //Grounded Moving
            if (Input.GetKey("a") && move.x > -maxGroundVel)
            {
                Debug.Log("A");
                move.x -= 0.05f * speed;
            }
            else if (Input.GetKey("d") && move.x < maxGroundVel)
            {
                Debug.Log("D");
                move.x += 0.05f * speed;
            }
            if (Input.GetKey("l") && dashCoolCount == 0)
            {
                if (move.x > 2)
                {
                    dashCoolCount = dashCooldown;
                    move.x += dashSpeed;
                }
                if (move.x < -2)
                {
                    dashCoolCount = dashCooldown;
                    move.x -= dashSpeed;
                }
            }
            //Grounded Friction
            if (move.x > 0)
            {
                move.x -= groundFriction;
            }
            if (move.x < 0)
            {
                move.x += groundFriction;
            }
            //Reset Velocity When Near 0
            if (move.x < 1 && move.x > -1)
                move.x = 0;
        } 
        //Aerial Movement
        if (isGrounded == false)
        {
            //Air Jump
            if (Input.GetKeyDown("space") && doubleJump == false)
            {
                move.y = doubleJumpBoost * jumpHeight;
                doubleJump = true;
            }
            //Air Movement
            if (Input.GetKey("a") && move.x > -maxAirVel)
            {
                move.x -= 0.05f * speed;
            }
            else if (Input.GetKey("d") && move.x < maxAirVel)
            {
                move.x += 0.05f * speed;
            }
            if (Input.GetKey("s"))
            {
                move.y -= ffSpeed;
            }
            //Air Friction
            if (move.x > 0)
            {
                move.x -= airFriction;
            }
            if (move.x < 0)
            {
                move.x += airFriction;
            }
            //Reset Velocity When Near 0
            if (move.x < 1 && move.x > -1)
                move.x = 0;
        }

        //move.x = Input.GetAxisRaw("Horizontal") * speed;





        //Gravity
        move.y += gravity * Time.deltaTime;

        //Set Move Velocity
        move = new Vector3(move.x, move.y, 0);
    }
}
