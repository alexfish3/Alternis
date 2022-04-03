using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class MOVplay : MonoBehaviour
{
    private IEnumerator c;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;

        float timeToWait = (float)GameObject.Find("Video Player").GetComponent<VideoPlayer>().clip.length;
        c = waitTillVideoDone(timeToWait);
        StartCoroutine(c);
    }

    private IEnumerator waitTillVideoDone(float timeToWait)
    {
        yield return new WaitForSeconds(timeToWait);
        SceneManager.LoadScene(2);
    }

    void BreakPlay()
    {
        StopCoroutine(c);
        SceneManager.LoadScene(2);
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
