using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class elevator : MonoBehaviour
{

    [SerializeField] GameObject positionStart;
    [SerializeField] GameObject positionEnd;
    [SerializeField] float speed;

    bool triggered = false;
    bool reachedDestination = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (reachedDestination == false && triggered == true)
        {
            this.transform.position = Vector3.MoveTowards(this.transform.position, positionEnd.transform.position, speed);
        }
        else if (triggered == false)
        {
            this.transform.position = Vector3.MoveTowards(this.transform.position, positionStart.transform.position, speed);
        }

        triggered = false;
    }

    void OnTriggerStay(Collider other)
    {
            triggered = true;
    }
    void OnTriggerExit(Collider other)
    {
        triggered = false;
    }

}
