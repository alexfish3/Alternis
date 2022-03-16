using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SendToNextScene : MonoBehaviour
{

    EssentialGameObjects essentialGameObjects;

    // Start is called before the first frame update
    void Start()
    {
        essentialGameObjects = GameObject.FindWithTag("Dont Destroy").GetComponent<EssentialGameObjects>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            essentialGameObjects.sceneTransition.GetComponent<Animator>().SetTrigger("Fade In");
        }
    }
}
