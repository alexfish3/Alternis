using UnityEngine;

public class Checkpoints : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private Transform[] checkpoints;
    [SerializeField] private Transform curCheck;
    public bool debug;

    EssentialGameObjects essentialGameObjects;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == ("Player"))
        {
            curCheck = checkpoints[1];
        }
    }

    void CheckpointMaker()
    {
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

                checkpoints[counter2] = go.transform;
                counter2++;
            }

        }
    }
    void Start()
    {
        essentialGameObjects = GameObject.FindWithTag("Dont Destroy").GetComponent<EssentialGameObjects>();
        player = GameObject.FindGameObjectWithTag("Player");
        CheckpointMaker();
        curCheck = checkpoints[0];
        //if player touches a new checkpoint it should make the currespawn the new checkpoint

    }

    void curUpdater()
    {
        for (int i = 0; i < checkpoints.Length; i++)
        {
            if ((int)player.transform.position.x == (int)checkpoints[i].position.x)
            {
                if(checkpoints[i].GetComponent<referenceRespawn>().isReached == false)
                {                    
                    curCheck = checkpoints[i];
                    curCheck.GetComponent<referenceRespawn>().reachedCheckpoint();
                    essentialGameObjects.SFXObject.GetComponent<AudioSource>().PlayOneShot(essentialGameObjects.checkpointReached);
                }
            }

            if(debug)
            {
                Debug.Log("CurUpdaterIterator:" + i);
                Debug.Log("Current Player Position X:"+player.transform.position.x);
                Debug.Log("Current Iterator Checkpoint Position X:" + checkpoints[i].position.x);
            }
        }
    }
    public Transform getCurCheck()
    {
        return curCheck;
    }
    void Update()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        curUpdater();
    }
}
