using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class message : MonoBehaviour
{
    [SerializeField] GameObject messageCanvas;
    [TextArea(5,1)]
    [SerializeField] string messageString;

    private void OnTriggerEnter(Collider player)
    {
        if(player.gameObject.tag == "Player")
        {
            messageCanvas.SetActive(true);
            messageCanvas.GetComponentInChildren<TMP_Text>().text = messageString;
        }
    }

    private void OnTriggerExit(Collider player)
    {
        if (player.gameObject.tag == "Player")
        {
            messageCanvas.SetActive(false);
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
