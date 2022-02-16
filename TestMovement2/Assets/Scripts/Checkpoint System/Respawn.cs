using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawn : MonoBehaviour
{
    [SerializeField] private Checkpoints cp;
    [SerializeField] private Transform respawner;
    [SerializeField] private GameObject player;
    [SerializeField] private float maxFallHeight = -15;
    public bool useFallHeight;

    private void CurCheckpoint()
    {
        respawner = cp.getCurCheck();
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Death") && other.gameObject.layer == 7 && player.GetComponent<WorldSwap>().lightWorld)
        {
            CurCheckpoint();
            player.transform.position = respawner.position;
        }
    }
    private void didYouFall()
    {
        if(player.transform.position.y < maxFallHeight)
        {
            player.transform.position = respawner.position;
        }
    }
     
    void Start()
    {
        CurCheckpoint();
    }

    void Update()
    {
        CurCheckpoint();
        if(useFallHeight) didYouFall();
    }

}
