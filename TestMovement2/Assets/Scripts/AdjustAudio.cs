using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdjustAudio : MonoBehaviour
{
    [SerializeField] float[] volumeOptions = new float[9];
    [SerializeField] bool isBGM;
    [SerializeField] bool isSFX;
    EssentialGameObjects essentialGameObjects;


    // Start is called before the first frame update
    void Start()
    {
        essentialGameObjects = GameObject.FindWithTag("Dont Destroy").GetComponent<EssentialGameObjects>();
    }

    // Update is called once per frame
    void Update()
    {
        if(isBGM == true)
        {
            this.gameObject.GetComponent<AudioSource>().volume = volumeOptions[essentialGameObjects.bgmVolume];
        }
        else if (isSFX == true)
        {
            this.gameObject.GetComponent<AudioSource>().volume = volumeOptions[essentialGameObjects.sfxVolume];
        }
    }
}
