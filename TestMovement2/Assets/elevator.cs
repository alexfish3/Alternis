using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class elevator : MonoBehaviour
{

    [SerializeField] GameObject[] transformPositions;
    [SerializeField] float speed;
    [SerializeField] bool reachedDestination = false;
    public float waitTime;

    bool triggered = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (reachedDestination == false)
        {
            this.transform.position = Vector3.MoveTowards(this.transform.position, transformPositions[0].transform.position, speed);
        }
        else if (reachedDestination == true)
        {
            this.transform.position = Vector3.MoveTowards(this.transform.position, transformPositions[1].transform.position, speed);
        }

        if (this.transform.position == transformPositions[0].transform.position && !triggered)
        {
            triggered = true;
            StartCoroutine(Wait());
        }
        else if (this.transform.position == transformPositions[1].transform.position && !triggered)
        {
            triggered = true;
            StartCoroutine(Wait());
        }
    }

    private IEnumerator Wait()
    {
        yield return new WaitForSeconds(waitTime);
        Debug.Log("TESt");
        if (reachedDestination)
        {
            reachedDestination = false;
        } else
        {
            reachedDestination = true;
        }
        triggered = false;
    }
}
