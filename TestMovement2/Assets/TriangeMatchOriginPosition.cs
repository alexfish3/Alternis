using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriangeMatchOriginPosition : MonoBehaviour
{
    [SerializeField]
    private GameObject originPosition;  

    // Update is called once per frame
    void Update()
    {
        this.transform.position = originPosition.transform.position;
    }
}
