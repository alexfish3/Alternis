using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class referenceRespawn : MonoBehaviour
{
    public bool isReached = false;
    public GameObject respectiveSpawner;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void reachedCheckpoint()
    {
        isReached = true;
        respectiveSpawner.GetComponent<RespawnPointAnim>().reached = true;
    }
}
