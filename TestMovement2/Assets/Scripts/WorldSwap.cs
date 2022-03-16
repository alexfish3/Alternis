using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WorldSwap : MonoBehaviour
{
    public GameObject blurCamera; 

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
    [SerializeField] bool disableSwap;
    public float cooldown;
    [SerializeField] public bool lightWorld;
    [SerializeField] public bool darkWorld;
    [SerializeField] GameObject UICanvas;
    [SerializeField] GameObject canSwitchSignier;
    public TMP_Text switchText;

    private bool warning = false;
    private int receivedId = 0;

    [Header("Color Info")]
    [SerializeField] Color canUse;
    [SerializeField] Color cantUse;
    [SerializeField] Color disabled;
    [SerializeField] Color warningColor;

    EssentialGameObjects essentialGameObjects;
    // Start is called before the first frame update
    void Start()
    {
        switchText.text = (cooldown * 10).ToString();
        essentialGameObjects = GameObject.FindWithTag("Dont Destroy").GetComponent<EssentialGameObjects>();
    }

    // Update is called once per frame
    void Update()
    {
        if (essentialGameObjects.GetComponent<PauseMenu>().isPaused == false)
        {
            oxygenBar.GetComponent<Slider>().value = oxygen;

            if (disableSwap == true)
            {
                canSwitchSignier.GetComponent<Image>().color = disabled;
                switchText.enabled = false;
            }
            else
            {
                if (canSwitch == true)
                {
                    canSwitchSignier.GetComponent<Image>().color = canUse;
                    switchText.enabled = false;

                    if (warning)
                    {
                        canSwitchSignier.GetComponent<Image>().color = warningColor;
                        Debug.LogError("Baba");
                    }
                }
                else if (canSwitch == false)
                {
                    canSwitchSignier.GetComponent<Image>().color = cantUse;
                    switchText.enabled = true;
                }
            }

            if (Input.GetButtonDown("World Swap") && canSwitch == true && disableSwap == false)
            {
                switchText.text = (cooldown * 10).ToString();
                canSwitch = false;

                if (changingLevel == true)
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
            if (darkWorld == true && swappedWorlds == true)
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
    }

    private void FixedUpdate()
    {
        disableSwap = false;
    }

    public IEnumerator CoolDownTimer()
    {
        switchText.text = (cooldown * 10).ToString();
        for (float i = cooldown; i >= 0; i = i - 0.1f)
        {
            // Debug.Log("E");
            yield return new WaitForSeconds(0.1f);
            int roundedCounter = (int)(i * 10);

            switchText.text = roundedCounter.ToString();
        }

        canSwitch = true;
    }

    public IEnumerator looseOxygen()
    {
        changingLevel = true;
        for (int i = oxygen; i > 0; i--)
        {
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
        for (int i = oxygen; i < 100; i++)
        {
            if (stopOxygenChange == true)
            {
                stopOxygenChange = false;
                break;
            }

            oxygen++;
            yield return new WaitForSeconds(speed);
        }
        changingLevel = false;
    }

    void OnTriggerStay(Collider other)
    {
        if (other.tag == "DisableSwap")
        {
            disableSwap = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "DisableSwap")
        {
            disableSwap = false;
        }
    }

    public void setWarning(int key, bool x)
    {
        
        if (receivedId == 0)
        {
            if (x)
            {
                receivedId = key;
                warning = x;
            }
        }
        else if (key == receivedId)
        {
            if (x)
            {
                receivedId = key;
            }
            else
            {
                receivedId = 0;
            }

            warning = x;
        }
    }

    public void respawn()
    {
        darkWorld = false;
        lightWorld = true;
        canSwitch = true;
        UICanvas.GetComponent<Animator>().SetTrigger("Respawn");
    }

}
