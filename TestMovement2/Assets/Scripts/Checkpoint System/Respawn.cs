using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawn : MonoBehaviour
{
    [SerializeField] private Checkpoints cp;
    [SerializeField] private Transform respawner;
    [SerializeField] private GameObject player;
    [SerializeField] private float maxFallHeight = -15;
    [SerializeField] private Animation anime;
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
            anime.Play();
            //The Animation must always be bigger than the timer as the timer is when the animation and player stops for respawns so their should be a little bit of leg room at the end of the animation.
            while (anime.isPlaying)
            {
                //This is the timer for the intervals it's just adding the interval's until it reaches a certain point during the animation.
                //This is the exact time between 2 frame calls in float added to whatever the current timer number is
                //In order for us to figure out exactly what the perfect timer is just work with these numbers considering this will be the most consistent timing for the animation
                Debug.Log("The Timer for death is currently:" + timer);
                timer += Time.deltaTime;
                Debug.Log("The Timer for death after addition is:" + timer);
                if (timer > 3000)
                {
                    anime.Stop();
                    DoRespawn();
                }

            }
            //This is temporary because their is no animation;
            DoRespawn();
        }
        if (other.gameObject.tag == "Death")
        {
            timer = 0;
            anime.Play();
            //The Animation must always be bigger than the timer as the timer is when the animation and player stops for respawns so their should be a little bit of leg room at the end of the animation.
            while (anime.isPlaying)
            {
                //This is the timer for the intervals it's just adding the interval's until it reaches a certain value
                //This is the exact time between 2 frame calls in float added to whatever the current timer number is
                //In order for us to figure out exactly what the perfect timer is just work with these numbers considering this will be the most consistent timing for the animation
                Debug.Log("The Timer for death is currently:" + timer);
                timer += Time.deltaTime;
                Debug.Log("The Timer for death after addition is:" + timer);
                if (timer > 3000)
                {
                    anime.Stop();
                    DoRespawn();
                    anime.Rewind();
                }
            }
            //This is temporary because their is no animation;
            DoRespawn();
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
    public void DoRespawn()
    {
        CurCheckpoint();
        player.transform.position = respawner.position;
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
    }

    void Update()
    {
        CurCheckpoint();
        if (useFallHeight) didYouFall();
    }

}
