using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIEvents : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] float cooldown;
    [SerializeField] GameObject lightWorld;
    [SerializeField] GameObject darkWorld;
    [SerializeField] GameObject gasMaskGameobject;

    private void Start()
    {
        cooldown = player.GetComponent<WorldSwap>().cooldown;
    }

    public void changeToDark()
    {
        lightWorld.SetActive(false);
        darkWorld.SetActive(true);
        player.GetComponent<WorldSwap>().switchText.text = (cooldown * 10).ToString();
    }
    public void changeToLight()
    {
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
}
