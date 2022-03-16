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
    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Death") && other.gameObject.layer == 7 && player.GetComponent<WorldSwap>().lightWorld)
        {
            DoRespawn();
        } else if (other.gameObject.CompareTag("Death"))
        {
            DoRespawn();
        }
    }
    private void didYouFall()
    {
        if(player.transform.position.y < maxFallHeight)
        {
            player.transform.position = respawner.position;
        }
    }

    public void DoRespawn()
    {
        Debug.LogError("Respawning");
        CurCheckpoint();
        player.transform.position = respawner.position;
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
