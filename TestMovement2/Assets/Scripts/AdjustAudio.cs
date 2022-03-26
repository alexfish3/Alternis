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

        if (isBGM == true)
        {
            this.gameObject.GetComponent<AudioSource>().volume = volumeOptions[essentialGameObjects.bgmVolume];
        }
        else if (isSFX == true)
        {
            this.gameObject.GetComponent<AudioSource>().volume = volumeOptions[essentialGameObjects.sfxVolume];
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void updateAudio()
    {
        if (isBGM == true)
        {
            this.gameObject.GetComponent<AudioSource>().volume = volumeOptions[essentialGameObjects.bgmVolume];
        }
        else if (isSFX == true)
        {
            this.gameObject.GetComponent<AudioSource>().volume = volumeOptions[essentialGameObjects.sfxVolume];
        }
    }

    public float volumeToChangeTo()
    {
        return volumeOptions[essentialGameObjects.bgmVolume];
    }

    public IEnumerator FadeVolume(AudioSource audioSource, float duration, float targetVolume)
    {
        float currentTime = 0;
        float start = audioSource.volume;

        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(start, targetVolume, currentTime / duration);
            yield return null;
        }
        yield break;
    }

    public IEnumerator changePitch(AudioSource audioSource, float duration, float targetPitch)
    {
        float currentTime = 0;
        float start = audioSource.volume;

        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            audioSource.pitch = Mathf.Lerp(start, targetPitch, currentTime / duration);
            yield return null;
        }
        yield break;
    }

    public void pitchToOne()
    {
        this.gameObject.GetComponent<AudioSource>().pitch = 1f;
    }
}
