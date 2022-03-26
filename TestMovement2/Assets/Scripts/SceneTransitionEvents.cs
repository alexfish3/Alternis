using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionEvents : MonoBehaviour
{
    EssentialGameObjects essentialGameObjects;
    [SerializeField] GameObject bgmObject;

    // Start is called before the first frame update
    void Start()
    {
        essentialGameObjects = GameObject.FindWithTag("Dont Destroy").GetComponent<EssentialGameObjects>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void loadNextSceneFromBuildIndex()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public void loadLevel1()
    {
        SceneManager.LoadScene(2);
    }

    public void loadLevel2()
    {
        SceneManager.LoadScene(3);
    }

    public void loadLevel3()
    {
        SceneManager.LoadScene(4);
    }

    public void changeMusic(AudioClip clip)
    {
        essentialGameObjects.BGMObject.GetComponent<AudioSource>().clip = clip;
        bgmObject.SetActive(false);
    }

    public void MuteBGM()
    {
        StartCoroutine(essentialGameObjects.BGMObject.GetComponent<AdjustAudio>().FadeVolume(essentialGameObjects.BGMObject.GetComponent<AudioSource>(), .5f, 0));
    }
    public void unMuteBGM()
    {
        bgmObject.SetActive(true);
        StartCoroutine(essentialGameObjects.BGMObject.GetComponent<AdjustAudio>().FadeVolume(essentialGameObjects.BGMObject.GetComponent<AudioSource>(), .5f, essentialGameObjects.BGMObject.GetComponent<AdjustAudio>().volumeToChangeTo()));
    }
}
