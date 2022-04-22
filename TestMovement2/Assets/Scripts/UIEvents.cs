using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIEvents : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] float cooldown;
    [SerializeField] GameObject lightWorld;
    [SerializeField] GameObject darkWorld;
    [SerializeField] Material lightWorldSkybox;
    [SerializeField] Material darkWorldSkybox;
    [SerializeField] GameObject gasMaskGameobject;


    [SerializeField] GameObject PP;
    [SerializeField] GameObject PPWS;
    [SerializeField] GameObject PPBW;
    [SerializeField] GameObject PPWSBW;


    EssentialGameObjects essentialGameObjects;
    private void Start()
    {
        essentialGameObjects = GameObject.FindWithTag("Dont Destroy").GetComponent<EssentialGameObjects>();
        cooldown = player.GetComponent<WorldSwap>().cooldown;
    }

    public void changeToDark()
    {
        player.GetComponent<FinalMovement>().isGrounded = false;
        RenderSettings.skybox = darkWorldSkybox;
        lightWorld.SetActive(false);
        darkWorld.SetActive(true);
        player.GetComponent<WorldSwap>().switchText.text = (cooldown * 10).ToString();
    }
    public void changeToLight()
    {
        player.GetComponent<FinalMovement>().isGrounded = false;
        RenderSettings.skybox = lightWorldSkybox;
        lightWorld.SetActive(true);
        darkWorld.SetActive(false);
        player.GetComponent<WorldSwap>().switchText.text = (cooldown * 10).ToString();
    }

    public void beginCooldownReset()
    {
        player.GetComponent<WorldSwap>().swappedWorlds = true;
        StartCoroutine(player.GetComponent<WorldSwap>().CoolDownTimer());
    }

    public void gasMaskShowFirstTime()
    {
        if (player.GetComponent<WorldSwap>().firstTimeSwap == true)
        {
            gasMaskGameobject.GetComponent<Animator>().SetTrigger("ShowFirstTime");
            player.GetComponent<WorldSwap>().firstTimeSwap = false;
        }
    }

    public void gasMaskShow()
    {
        if(player.GetComponent<WorldSwap>().firstTimeSwap == false)
        {
            gasMaskGameobject.GetComponent<Animator>().SetTrigger("Show");
        }
    }

    public void gasMaskHide()
    {
        gasMaskGameobject.GetComponent<Animator>().SetTrigger("Hide");
    }

    public void changePitchOnBGM(float newPitch)
    {
        StartCoroutine(essentialGameObjects.BGMObject.GetComponent<AdjustAudio>().changePitch(essentialGameObjects.BGMObject.GetComponent<AudioSource>(), 0.1f, newPitch));
    }

    public void pitchToOne()
    {
        essentialGameObjects.BGMObject.GetComponent<AdjustAudio>().pitchToOne();
    }

    public void determineBW(bool bw)
    {
        if(bw == true)
        {
            PP.SetActive(false);
            PPWS.SetActive(false);
            PPBW.SetActive(true);
            PPWSBW.SetActive(true);
        }
        else if (bw == false)
        {
            PP.SetActive(true);
            PPWS.SetActive(true);
            PPBW.SetActive(false);
            PPWSBW.SetActive(false);
        }
    }
}
