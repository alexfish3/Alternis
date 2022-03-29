using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEvents : MonoBehaviour
{
    EssentialGameObjects essentialGameObjects;


    // Start is called before the first frame update
    void Start()
    {
        essentialGameObjects = GameObject.FindWithTag("Dont Destroy").GetComponent<EssentialGameObjects>();
    }

    public void playAudioOneShot(AudioClip clip)
    {
        essentialGameObjects.SFXObject.GetComponent<AudioSource>().PlayOneShot(clip);
    }
}
