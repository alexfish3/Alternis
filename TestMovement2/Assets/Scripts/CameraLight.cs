using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLight : MonoBehaviour
{
    [SerializeField]
    private GameObject cameraHeadBasket;
    
    [Header("Movement")]
    [Tooltip("Lower rotation limit (degrees below 0)")]
    [SerializeField]
    private float lowerRotLim;
    [Tooltip("Upper rotation limit (degrees above 0)")]
    [SerializeField]
    private float upperRotLim;
    [Tooltip("Rotation speed (degrees per second)")]
    [SerializeField]
    private float rotSpeed;
    [Tooltip("Time in seconds to wait at top/bottom")]
    [SerializeField]
    private float timeToWait;
    [Tooltip("Rotate up or down first? Check for down.")]
    [SerializeField]
    private bool rotDown = true;
    [Tooltip("Default starting angle. 0 is straight forward, increases counter-clockwise. Do not use negatives.")]
    [SerializeField]
    private float startingAngle;

    private float track;

    private float x = 0.0f;

    void Start()
    {
        upperRotLim = -upperRotLim;
        lowerRotLim = -lowerRotLim;
        cameraHeadBasket.transform.rotation = Quaternion.Euler(0.0f, 0.0f, startingAngle);

        StartCoroutine(RotateToPoints());
    }

    void OnEnable()
    {
        StartCoroutine(RotateToPoints());
    }


    IEnumerator RotateToPoints()
    {
        while (true)
        {
            if (rotDown)
            {
                while (x < lowerRotLim)
                {
                    cameraHeadBasket.transform.Rotate(0,0, rotSpeed * Time.deltaTime, Space.World);
                    x += rotSpeed * Time.deltaTime;
                    x = Mathf.Min(x, lowerRotLim);

                    yield return null;
                }

                rotDown = !rotDown;
                yield return new WaitForSeconds(timeToWait);
            }
            else
            {
               while (x > upperRotLim)
                {
                    cameraHeadBasket.transform.Rotate(0,0, -rotSpeed * Time.deltaTime, Space.World);
                    x -= rotSpeed * Time.deltaTime;
                    x = Mathf.Max(x, upperRotLim);

                    yield return null;
                }

                rotDown = !rotDown;
                yield return new WaitForSeconds(timeToWait);
            }
        }

        yield return null;
    }
}
