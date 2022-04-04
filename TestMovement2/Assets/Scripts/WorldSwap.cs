using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WorldSwap : MonoBehaviour
{
    [SerializeField] GameObject playerSprite;
    public GameObject blurCamera; 

    [Header("Oxygen Info")]
    [SerializeField] int oxygen = 100;
    [SerializeField] int maxOxygen = 120;
    [SerializeField] int oxygenRegainSpeed = 6;
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
    [SerializeField] GameObject radiation;
    public TMP_Text switchText;

    public Color tempColor;


    private bool warning = false;
    private int receivedId = 0;

    EssentialGameObjects essentialGameObjects;
    // Start is called before the first frame update
    void Start()
    {
        oxygen = maxOxygen;
        switchText.text = (cooldown * 10).ToString();
        essentialGameObjects = GameObject.FindWithTag("Dont Destroy").GetComponent<EssentialGameObjects>();

        // Updates Reference To UI Gameobjects
        essentialGameObjects.updateReferencesToUICanvas();
    }

    // Update is called once per frame
    void Update()
    {
        if (essentialGameObjects.GetComponent<PauseMenu>().isPaused == false)
        {
            oxygenBar.GetComponent<Slider>().value = oxygen;

            tempColor = radiation.GetComponent<Image>().color;
            tempColor.a = (((oxygen - 100) * -1)/ 100f);

            radiation.GetComponent<Image>().color = tempColor;

            // Kill player if oxygen gets to zero
            if(oxygen == 0)
            {
                this.gameObject.GetComponent<Respawn>().loseOxygenRespawn();
            }

            if (disableSwap == true)
            {
                canSwitchSignier.transform.GetChild(1).gameObject.SetActive(true);
                switchText.enabled = false;
            }
            else
            {
                canSwitchSignier.transform.GetChild(1).gameObject.SetActive(false);

                if (canSwitch == true)
                {
                    switchText.enabled = false;
                }
                else if (canSwitch == false)
                {
                    switchText.enabled = true;
                }
            }

            if (Input.GetButtonDown("World Swap") && canSwitch == true && disableSwap == false)
            {
                essentialGameObjects.SFXObject.GetComponent<AudioSource>().PlayOneShot(essentialGameObjects.worldSwap);
                playerSprite.GetComponent<Animator>().ResetTrigger("Walk");
                playerSprite.GetComponent<Animator>().ResetTrigger("Sprint");
                playerSprite.GetComponent<Animator>().ResetTrigger("Fall");
                playerSprite.GetComponent<Animator>().ResetTrigger("Jump");
                playerSprite.GetComponent<Animator>().ResetTrigger("Crouch");
                playerSprite.GetComponent<Animator>().ResetTrigger("CrouchIdle");
                playerSprite.GetComponent<Animator>().ResetTrigger("Uncrouch");
                playerSprite.GetComponent<Animator>().ResetTrigger("CrouchWalk");

                if (!this.gameObject.GetComponent<PlayerAnimations>().isCrouch)
                {
                    playerSprite.GetComponent<Animator>().SetTrigger("Shift");
                }
                else
                {
                    playerSprite.GetComponent<Animator>().SetTrigger("CrouchShift");
                }

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
        canSwitchSignier.transform.GetChild(0).gameObject.transform.GetChild(0).GetComponent<Slider>().value = (cooldown * 10) * -1;

        switchText.text = (cooldown * 10).ToString();
        for (float i = cooldown; i >= 0; i = i - 0.1f)
        {
            yield return new WaitForSeconds(0.1f);
            int roundedCounter = (int)(i * 10);
            canSwitchSignier.transform.GetChild(0).gameObject.transform.GetChild(0).GetComponent<Slider>().value = roundedCounter * -1;
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
        for (int i = oxygen; i < maxOxygen; i++)
        {
            if (stopOxygenChange == true)
            {
                stopOxygenChange = false;
                break;
            }

            oxygen++;
            yield return new WaitForSeconds(speed / oxygenRegainSpeed);
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
        oxygen = 100;
        swappedWorlds = true;
        darkWorld = false;
        lightWorld = true;
        canSwitch = true;
        UICanvas.GetComponent<Animator>().SetTrigger("Respawn");
    }

}
