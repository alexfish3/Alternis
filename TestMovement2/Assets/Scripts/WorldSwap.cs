using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WorldSwap : MonoBehaviour
{
    [Header("Oxygen Info")]
    [SerializeField] int oxygen = 100;
    [SerializeField] float speed;
    public bool swappedWorlds;
    public bool stopOxygenChange = false;
    public bool changingLevel = false;
    public GameObject oxygenBar;

    [Header("Swapping Information")]
    public bool firstTimeSwap = true;
    [SerializeField] bool canSwitch;
    public float cooldown;
    [SerializeField] bool lightWorld;
    [SerializeField] bool darkWorld;
    [SerializeField] GameObject UICanvas;
    [SerializeField] GameObject canSwitchSignier;
    public TMP_Text switchText;

    [Header("Color Info")]
    [SerializeField] Color canUse;
    [SerializeField] Color cantUse;

    // Start is called before the first frame update
    void Start()
    {
        switchText.text = (cooldown * 10).ToString();
    }

    // Update is called once per frame
    void Update()
    {

        oxygenBar.GetComponent<Slider>().value = oxygen;

        if (canSwitch == true)
        {
            canSwitchSignier.GetComponent<Image>().color = canUse;
            switchText.enabled = false;
        }
        else if (canSwitch == false)
        {
            canSwitchSignier.GetComponent<Image>().color = cantUse;
            switchText.enabled = true;
        }

        if (Input.GetButtonDown("World Swap") && canSwitch == true)
        {
            switchText.text = (cooldown * 10).ToString();
            canSwitch = false;

            if(changingLevel == true)
            {
                stopOxygenChange = true;
            }

            // Currently light world, going to switch to dark
            if (lightWorld == true && darkWorld == false)
            {
                darkWorld = true;
                lightWorld = false;
                UICanvas.GetComponent<Animator>().SetTrigger("LightToDark");
            }
            // Currently dark world, going to switch to light
            else if (lightWorld == false && darkWorld == true)
            {
                darkWorld = false;
                lightWorld = true;
                UICanvas.GetComponent<Animator>().SetTrigger("DarkToLight");
            }
        }

        // Loose Oxygen
        if(darkWorld == true && swappedWorlds == true)
        {
            swappedWorlds = false;
            StartCoroutine(looseOxygen());
        }

        // Gain Oxygen
        if (lightWorld == true && swappedWorlds == true)
        {
            swappedWorlds = false;
            StartCoroutine(gainOxygen());
        }
    }

    public IEnumerator CoolDownTimer()
    {
        switchText.text = (cooldown * 10).ToString();
        for (float i = cooldown; i >= 0; i = i - 0.1f)
        {
            yield return new WaitForSeconds(0.1f);
            int roundedCounter = (int)(i * 10);

            switchText.text = roundedCounter.ToString();
        }

        canSwitch = true;
    }

    public IEnumerator looseOxygen()
    {
        changingLevel = true;
        Debug.Log("A2w");
        for (int i = oxygen; i > 0; i--)
        {
            Debug.Log("-");
            if (stopOxygenChange == true)
            {
                stopOxygenChange = false;
                break;
            }

            oxygen--;
            yield return new WaitForSeconds(speed);
        }
        changingLevel = false;
    }

    public IEnumerator gainOxygen()
    {
        changingLevel = true;
        Debug.Log("Aw");
        for (int i = oxygen; i < 100; i++)
        {
            Debug.Log("+");
            if (stopOxygenChange == true)
            {
                stopOxygenChange = false;
                break;
            }

            oxygen++;
            yield return new WaitForSeconds(speed);
        }
        Debug.Log("At Max Oxy");
        changingLevel = false;
    }
}
