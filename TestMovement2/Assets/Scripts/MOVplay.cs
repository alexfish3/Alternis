using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class MOVplay : MonoBehaviour
{
    private IEnumerator c;
    VideoPlayer VideoPlayer;
    EssentialGameObjects essentialGameObjects;
    [SerializeField] bool returnToMainMenu;
    [SerializeField] int sceneToLoad;
    [SerializeField] AudioClip mainMenuMusic;

    // Start is called before the first frame update
    void Start()
    {
        essentialGameObjects = GameObject.FindWithTag("Dont Destroy").GetComponent<EssentialGameObjects>();

        Cursor.visible = false;

        VideoPlayer = GameObject.Find("Video Player").GetComponent<VideoPlayer>();
        VideoPlayer.SetTargetAudioSource(0, essentialGameObjects.SFXObject.GetComponent<AudioSource>());
        VideoPlayer.Play();

        float timeToWait = ((float)VideoPlayer.clip.length) + 0.5f;
        c = waitTillVideoDone(timeToWait);
        StartCoroutine(c);
    }

    private IEnumerator waitTillVideoDone(float timeToWait)
    {
        yield return new WaitForSeconds(timeToWait);
        if(returnToMainMenu == true)
        {
            essentialGameObjects.BGMObject.SetActive(true);
            essentialGameObjects.BGMObject.GetComponent<AudioSource>().Stop();
            essentialGameObjects.BGMObject.GetComponent<AudioSource>().clip = mainMenuMusic;
            essentialGameObjects.BGMObject.GetComponent<AudioSource>().Play();
            essentialGameObjects.BGMObject.GetComponent<AdjustAudio>().pitchToOne();
            SceneManager.LoadScene(sceneToLoad);
        }
        else
        {
            essentialGameObjects.sceneTransition.GetComponent<Animator>().SetTrigger("Fade In");
        }
    }

    void BreakPlay()
    {
        StopCoroutine(c);
        if (returnToMainMenu == true)
        {
            essentialGameObjects.BGMObject.SetActive(true);
            essentialGameObjects.BGMObject.GetComponent<AudioSource>().Stop();
            essentialGameObjects.BGMObject.GetComponent<AudioSource>().clip = mainMenuMusic;
            essentialGameObjects.BGMObject.GetComponent<AudioSource>().Play();
            SceneManager.LoadScene(sceneToLoad);
        }
        else
        {
            essentialGameObjects.sceneTransition.GetComponent<Animator>().SetTrigger("Fade In");
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            BreakPlay();
        }

        // ps4 controls
        if (GetComponent<ControllerType>().PS4 == true)
        {
            if (Input.GetButtonDown("Pause"))
            {
                BreakPlay();
            }
        }
        // xb1 controls
        else if (GetComponent<ControllerType>().XB1 == true)
        {
            if (Input.GetButtonDown("Pause"))
            {
                BreakPlay();
            }
        }
    }
}
