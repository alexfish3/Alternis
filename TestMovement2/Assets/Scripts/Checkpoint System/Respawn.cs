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
    [SerializeField] GameObject UICanvas;
    public bool death;
    bool hazardDeath;
    public bool useFallHeight;
    EssentialGameObjects essentialGameObjects;


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
        else if (other.gameObject.tag == "Death")
        {
            timer = 0;
            hazardDeath = true;
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
        death = true;

        CurCheckpoint();
        if (!playerSprite.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Caught") && !playerSprite.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Death"))
        {
            if (!hazardDeath)
            {
                playerSprite.GetComponent<Animator>().ResetTrigger("Walk");
                playerSprite.GetComponent<Animator>().ResetTrigger("Sprint");
                playerSprite.GetComponent<Animator>().ResetTrigger("Fall");
                playerSprite.GetComponent<Animator>().ResetTrigger("Jump");
                playerSprite.GetComponent<Animator>().ResetTrigger("Crouch");
                playerSprite.GetComponent<Animator>().ResetTrigger("CrouchIdle");
                playerSprite.GetComponent<Animator>().ResetTrigger("Uncrouch");
                playerSprite.GetComponent<Animator>().ResetTrigger("CrouchWalk");
                playerSprite.GetComponent<Animator>().SetTrigger("Caught");
            }
            else
            {
                playerSprite.GetComponent<Animator>().ResetTrigger("Walk");
                playerSprite.GetComponent<Animator>().ResetTrigger("Sprint");
                playerSprite.GetComponent<Animator>().ResetTrigger("Fall");
                playerSprite.GetComponent<Animator>().ResetTrigger("Jump");
                playerSprite.GetComponent<Animator>().ResetTrigger("Crouch");
                playerSprite.GetComponent<Animator>().ResetTrigger("CrouchIdle");
                playerSprite.GetComponent<Animator>().ResetTrigger("Uncrouch");
                playerSprite.GetComponent<Animator>().ResetTrigger("CrouchWalk");
                playerSprite.GetComponent<Animator>().SetTrigger("Death");
            }
            StartCoroutine(Fade());
        }

    }

    IEnumerator Fade()
    {
        player.GetComponent<WorldSwap>().respawnBool = true;
        player.GetComponent<FinalMovement>().respawnBool = true;
        playerSprite.GetComponent<Animator>().ResetTrigger("Walk");
        playerSprite.GetComponent<Animator>().ResetTrigger("Sprint");
        playerSprite.GetComponent<Animator>().ResetTrigger("Fall");
        playerSprite.GetComponent<Animator>().ResetTrigger("Jump");
        playerSprite.GetComponent<Animator>().ResetTrigger("Crouch");
        playerSprite.GetComponent<Animator>().ResetTrigger("CrouchIdle");
        playerSprite.GetComponent<Animator>().ResetTrigger("Uncrouch");
        playerSprite.GetComponent<Animator>().ResetTrigger("CrouchWalk");
        yield return new WaitForSeconds(1);
        UICanvas.GetComponent<Animator>().SetTrigger("Respawn");
        player.transform.position = respawner.transform.position;
        this.gameObject.GetComponent<WorldSwap>().respawn();
        playerSprite.GetComponent<Animator>().ResetTrigger("Shift");
        hazardDeath = false;
        death = false;
        player.GetComponent<WorldSwap>().respawnBool = false;
        player.GetComponent<FinalMovement>().respawnBool = false;
        essentialGameObjects.BGMObject.GetComponent<AdjustAudio>().pitchToOne();
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
        essentialGameObjects = GameObject.FindWithTag("Dont Destroy").GetComponent<EssentialGameObjects>();
    }

    void Update()
    {
        CurCheckpoint();
        if (useFallHeight) didYouFall();
    }

}
