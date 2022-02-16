using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class Rotate : MonoBehaviour
{
    public float speed = 0f;
    
    public enum Direction {ForwardX, ForwardY, ForwardZ, ReverseX, ReverseY, ReverseZ};
    public Direction RotationDirection;

    [Header("PingPong")]
    public bool pingPong;
    public float min, max;
   
    void Update ()
    {
        ConstantRotation();
        PingPongRotation();

    }

    private void ConstantRotation()
    {
        if(!pingPong)
        {
            //Forward Direction
            if (RotationDirection == Direction.ForwardX)
            {
                SetDirection(Time.deltaTime * speed, 0, 0);
            }
            if (RotationDirection == Direction.ForwardY)
            {
                SetDirection(0, Time.deltaTime * speed, 0);
            }
            if (RotationDirection == Direction.ForwardZ)
            {
                SetDirection(0, 0, Time.deltaTime * speed);
            }

            //Reverse Direction
            if (RotationDirection == Direction.ReverseX)
            {
                SetDirection(-Time.deltaTime * speed, 0, 0);
            }
            if (RotationDirection == Direction.ReverseY)
            {
                SetDirection(0, -Time.deltaTime * speed, 0);
            }
            if (RotationDirection == Direction.ReverseZ)
            {
                SetDirection(0, 0, -Time.deltaTime * speed);
            }
        }
    }

    private void SetDirection(float x, float y, float z)
    {
        transform.Rotate(x, y, z, Space.Self);
    }

    private void PingPong(float x, float y, float z)
    {
        transform.localEulerAngles = new Vector3(x, y, z);
    }

    private void PingPongRotation()
    {
        if(pingPong)
        {
            //Forward Direction
            if (RotationDirection == Direction.ForwardX)
            {
                PingPong(Mathf.PingPong(Time.time * speed, max) - min, 0, 0);
            }
            if (RotationDirection == Direction.ForwardY)
            {
                PingPong(0, Mathf.PingPong(Time.time * speed, max) - min, 0);
            }
            if (RotationDirection == Direction.ForwardZ)
            {
                PingPong(0, 0, Mathf.PingPong(Time.time * speed, max) - min);
            }

            //Reverse Direction
            if (RotationDirection == Direction.ReverseX)
            {
                PingPong(Mathf.PingPong(-Time.time * speed, max) - min, 0, 0);
            }
            if (RotationDirection == Direction.ReverseY)
            {
                PingPong(0, Mathf.PingPong(-Time.time * speed, max) - min, 0);
            }
            if (RotationDirection == Direction.ReverseZ)
            {
                PingPong(0, 0, Mathf.PingPong(-Time.time * speed, max) - min);
            }
        }
    }
}