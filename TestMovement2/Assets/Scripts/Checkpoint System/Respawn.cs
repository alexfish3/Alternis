using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawn : MonoBehaviour
{
    [SerializeField] GameObject playerSprite;
    [SerializeField] private Checkpoints cp;
    [SerializeField] private Transform respawner;
    [SerializeField] private GameObject player;
    [SerializeField] private float maxFallHeight = -15;
    [SerializeField] private float timer = 0;
    public bool useFallHeight;

    private void CurCheckpoint()
    {
        respawner = cp.getCurCheck();
    }
    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Death") && other.gameObject.layer == 7 && player.GetComponent<WorldSwap>().lightWorld)
        {
            timer = 0;
            DoRespawn();

        }
        if (other.gameObject.tag == "Death")
        {
            timer = 0;
            DoRespawn();

            //This is temporary because their is no animation;
        }
    }
    private void didYouFall()
    {
        if (player.transform.position.y < maxFallHeight)
        {
            player.transform.position = respawner.position;
        }
    }

    // Respawn
    private void animatorreset()
    {
        playerSprite.GetComponent<Animator>().ResetTrigger("Walk");
        playerSprite.GetComponent<Animator>().ResetTrigger("Sprint");
        playerSprite.GetComponent<Animator>().ResetTrigger("Fall");
        playerSprite.GetComponent<Animator>().ResetTrigger("Jump");
        playerSprite.GetComponent<Animator>().ResetTrigger("Crouch");
        playerSprite.GetComponent<Animator>().ResetTrigger("CrouchIdle");
        playerSprite.GetComponent<Animator>().ResetTrigger("Uncrouch");
        playerSprite.GetComponent<Animator>().ResetTrigger("CrouchWalk");

    }
    private void deathanimereset()
    {
        playerSprite.GetComponent<Animator>().ResetTrigger("Caught");
        playerSprite.GetComponent<Animator>().ResetTrigger("Death");
    }
    public void DoRespawn()
    {
        CurCheckpoint();
        if (!playerSprite.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Caught"))
        {
            playerSprite.GetComponent<Animator>().SetTrigger("Caught");
            player.transform.position = respawner.transform.position;

            StartCoroutine(Fade());
        }

    }

    IEnumerator Fade()
    {
       yield return new WaitForSeconds(3);
       player.transform.position = respawner.transform.position;
    }

    public void loseOxygenRespawn()
    {
        player.GetComponent<WorldSwap>().respawn();
        Debug.LogError("Respawning");
        CurCheckpoint();
        player.transform.position = respawner.position;
    }

    void Start()
    {
        CurCheckpoint();
        cp.GetComponent("Checkpoints");
    }

    void Update()
    {
        CurCheckpoint();
        if (useFallHeight) didYouFall();
    }

}
