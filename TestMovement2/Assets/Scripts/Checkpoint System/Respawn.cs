using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawn : MonoBehaviour
{
    [SerializeField] private Checkpoints cp;
    [SerializeField] private Transform respawner;
    [SerializeField] private GameObject player;
    // Start is called before the first frame update

    private void CurCheckpoint()
    {
        respawner = cp.getCurCheck();
    }
    void OnTriggerEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Death"))
        {
            CurCheckpoint();
            player.transform.position = respawner.position;
        }
    }
    private void didYouFall()
    {
        if(player.transform.position.y < -15)
        {
            player.transform.position = respawner.position;
        }
    }
    void Start()
    {
        CurCheckpoint();
        player = GameObject.FindGameObjectWithTag("Player");

    }
    void Update()
    {
        CurCheckpoint();
        didYouFall();
    }

}
