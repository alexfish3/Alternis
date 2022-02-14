using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SplashScreen : MonoBehaviour
{
    [SerializeField] float timeToWait;
    [SerializeField] RenderTexture videoRenderTexture;
    [SerializeField] Texture firstFrameTexture;
    // Start is called before the first frame update
    void Start()
    {
        Graphics.Blit(firstFrameTexture, videoRenderTexture);
        StartCoroutine(waitTillVideoDone(timeToWait));
    }

    private IEnumerator waitTillVideoDone(float timeToWait)
    {
        yield return new WaitForSeconds(timeToWait);
        SceneManager.LoadScene(1);
    }
}
