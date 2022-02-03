using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine;

public class SceneLoader : MonoBehaviour
{
    //fade to black
    //load a scene
    [SerializeField] Animation anim;
    [SerializeField] string clip = "FadeOut";
    [SerializeField] AudioSource audioToFade;
    [HideInInspector] string currentSceneToLoad;

    // public void LoadScene(string scene)
    // {
    //     anim.Play(clip);
    //     currentSceneToLoad = scene;
    //     StartCoroutine(WaitForAnimation());
    // }

    // //Wait for the length of the Animation Clip.
    // public IEnumerator WaitForAnimation()
    // {
    //     var t = Time.time;
    //     float length = anim.GetClip(clip).length;
    //     if(audioToFade) StartCoroutine(FadeAudioSource.StartFade(audioToFade, length, 0));
    //     while(Time.time < t + length) {yield return null;}
    //     ContinueLoadScene(currentSceneToLoad);
    // }

    public void ContinueLoadScene(string scene)
    {
        SceneManager.LoadScene(scene);
    }

    public void QuitGame()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }


    // public void FadeIn(float targetVolume){
    //     StartCoroutine(FadeAudioSource.StartFade(audioToFade, 2, targetVolume));
    // }
}
