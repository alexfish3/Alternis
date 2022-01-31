using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoints : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private Transform[] checkpoints;
    [SerializeField] private Transform curCheck;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        int counter = 0;
        int counter2 = 0;
        foreach (GameObject go in GameObject.FindGameObjectsWithTag("Respawn"))
        {

            counter++;
            Debug.Log("How many respawns? " + counter);
        }
        Debug.Log("Hello we grabbing objects now with REspawn tags");
        if (counter > 0)
        {
            checkpoints = new Transform[counter];
            foreach (GameObject go in GameObject.FindGameObjectsWithTag("Respawn"))
            {
                
                checkpoints[counter2]=go.transform;
                counter2++;
            }

        }
        
        curCheck = checkpoints[0];
        //if player touches a new checkpoint it should make the currespawn the new checkpoint

    }
    void curChecker()
    {
        if (player.transform.position.y <= -30)
        {
            player.transform.position = curCheck.transform.position;
        }
        
    }
    void curUpdater()
    {
        for (int i = 0; i < checkpoints.Length; i++)
        {
            Debug.Log("CurUpdaterIterator:" + i);
            if ((int)player.transform.position.x ==(int) checkpoints[i].position.x) curCheck=checkpoints[i]; 
            Debug.Log("Current Player Position X:"+player.transform.position.x);
            Debug.Log("Current Iterator Checkpoint Position X:" + checkpoints[i].position.x);
        }
    }
    public Transform getCurCheck()
    {
        return curCheck;
    }
    // Update is called once per frame
    void Update()
    {
        curChecker();
        player = GameObject.FindGameObjectWithTag("Player");
        curUpdater();
    }
}
