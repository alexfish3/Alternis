using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneTransitionLevel : MonoBehaviour
{
    EssentialGameObjects essentialGameObjects;
    [SerializeField] bool FadeInMusic = true;
    [SerializeField] float delay;

    // Start is called before the first frame update
    void Start()
    {
        essentialGameObjects = GameObject.FindWithTag("Dont Destroy").GetComponent<EssentialGameObjects>();
        StartCoroutine(fadeOut());

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator fadeOut()
    {
        yield return new WaitForSeconds(delay);

        if(FadeInMusic == true)
        {
            essentialGameObjects.sceneTransition.GetComponent<Animator>().SetTrigger("Fade Out");
        }
        else
        {
            essentialGameObjects.sceneTransition.GetComponent<Animator>().SetTrigger("Fade Out Mute");
        }
    }
}
