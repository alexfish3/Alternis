using System;
using System.IO;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PauseMenuController : MonoBehaviour
{
    [Header("Main Menu Scroll Info")]
    [SerializeField] GameObject pauseCanvas;
    public bool inSettingsMenu = false;
    [SerializeField] GameObject[] positions;
    [SerializeField] int position;
    [SerializeField] bool canScrollY = true;
    [SerializeField] bool canScrollX = true;

    [Header("Settings Menu")]
    [SerializeField] int amountOfSettings;
    [SerializeField] GameObject settingsMenu;
    [SerializeField] GameObject[] settingsPositions;
    [SerializeField] int settingsPosition;
    [SerializeField] TMP_Text bgmText;
    [SerializeField] TMP_Text sfxText;
    [SerializeField] GameObject fullscreenToggle;
    [SerializeField] GameObject spotlightToggle;

    [Header("Default Settings")]
    [SerializeField] int defaultBGMVolume;
    [SerializeField] int defaultSFXVolume;
    [SerializeField] bool defaultIsFullscreen;
    [SerializeField] bool defaultShowSpotlights;

    bool loadFile = false;
    EssentialGameObjects essentialGameObjects;

    void Start()
    {
        essentialGameObjects = GameObject.FindWithTag("Dont Destroy").GetComponent<EssentialGameObjects>();

        position = 0;
        positions[position].GetComponent<Animator>().SetTrigger("Selected");

        settingsPosition = 0;
        settingsPositions[settingsPosition].GetComponent<Animator>().SetTrigger("Selected");
    }

    // Update is called once per frame
    void Update()
    {
        controlMainMenu();
    }

    private void controlMainMenu()
    {
        Keyboard();

        if (essentialGameObjects.GetComponent<ControllerType>().PS4 == true)
        {
            if (inSettingsMenu == false)
            {
                if (Input.GetButtonDown("Return"))
                {
                    // Resume
                    if (position == 0)
                    {
                        Debug.Log("Resume");
                        essentialGameObjects.GetComponent<PauseMenu>().resume();
                    }
                    // Settings
                    if (position == 1)
                    {
                        inSettingsMenu = true;
                        pauseCanvas.GetComponent<Animator>().SetTrigger("Enter Settings");
                        bgmText.text = essentialGameObjects.bgmVolume.ToString();
                        sfxText.text = essentialGameObjects.sfxVolume.ToString();
                        fullscreenToggle.SetActive(essentialGameObjects.isFullscreen);
                        spotlightToggle.SetActive(essentialGameObjects.showSpotlights);

                        settingsPosition = 0;
                        settingsPositions[settingsPosition].GetComponent<Animator>().SetTrigger("Selected");
                    }
                    // Main Menu
                    if (position == 2)
                    {
                        Debug.Log("Main Menu");
                        essentialGameObjects.GetComponent<PauseMenu>().mainMenu();
                        SceneManager.LoadScene(1);
                    }
                }


                if ((Input.GetAxis("Vertical") == 0))
                {
                    canScrollY = true;
                }
                else if (canScrollY == true)
                {
                    // Controller Controls
                    if ((Input.GetAxis("Vertical") > 0.5) && position > 0)
                    {
                        canScrollY = false;
                        positions[position].GetComponent<Animator>().SetTrigger("Deselected");
                        position--;
                        positions[position].GetComponent<Animator>().SetTrigger("Selected");
                    }
                    else if ((Input.GetAxis("Vertical") > 0.5) && position == 0)
                    {
                        canScrollY = false;
                        positions[position].GetComponent<Animator>().SetTrigger("Deselected");
                        position = positions.Length - 1;
                        positions[position].GetComponent<Animator>().SetTrigger("Selected");
                    }
                    else if ((Input.GetAxis("Vertical") < -0.5) && position < positions.Length - 1)
                    {
                        canScrollY = false;
                        positions[position].GetComponent<Animator>().SetTrigger("Deselected");
                        position++;
                        positions[position].GetComponent<Animator>().SetTrigger("Selected");
                    }
                    else if ((Input.GetAxis("Vertical") < -0.5) && position == positions.Length - 1)
                    {
                        canScrollY = false;
                        positions[position].GetComponent<Animator>().SetTrigger("Deselected");
                        position = 0;
                        positions[position].GetComponent<Animator>().SetTrigger("Selected");
                    }
                }
            }
            else if (inSettingsMenu == true)
            {
                if (Input.GetButtonDown("Escape"))
                {
                    writeToSettingsFile();
                    inSettingsMenu = false;
                    settingsPositions[settingsPosition].GetComponent<Animator>().SetTrigger("Deselected");
                    pauseCanvas.GetComponent<Animator>().SetTrigger("Exit Settings");
                }

                if ((Input.GetAxis("Horizontal") == 0))
                {
                    canScrollX = true;
                }
                else if (canScrollX == true)
                {
                    // Change BGM Volume
                    if (settingsPosition == 0)
                    {
                        if (Input.GetAxis("Horizontal") > 0.5 && essentialGameObjects.bgmVolume < essentialGameObjects.bgmMax)
                        {
                            Debug.Log("TESTER");
                            canScrollX = false;
                            essentialGameObjects.bgmVolume++;
                            essentialGameObjects.BGMObject.GetComponent<AdjustAudio>().updateAudio();
                            bgmText.text = essentialGameObjects.bgmVolume.ToString();
                        }
                        else if (Input.GetAxis("Horizontal") < -0.5 && essentialGameObjects.bgmVolume > 0)
                        {
                            canScrollX = false;
                            essentialGameObjects.bgmVolume--;
                            essentialGameObjects.BGMObject.GetComponent<AdjustAudio>().updateAudio();
                            bgmText.text = essentialGameObjects.bgmVolume.ToString();
                        }
                    }
                    // Change SFX Volume
                    if (settingsPosition == 1)
                    {
                        if (Input.GetAxis("Horizontal") > 0.5 && essentialGameObjects.sfxVolume < essentialGameObjects.sfxMax)
                        {
                            canScrollX = false;
                            essentialGameObjects.sfxVolume++;
                            essentialGameObjects.SFXObject.GetComponent<AdjustAudio>().updateAudio();
                            sfxText.text = essentialGameObjects.sfxVolume.ToString();
                        }
                        else if (Input.GetAxis("Horizontal") < -0.5 && essentialGameObjects.sfxVolume > 0)
                        {
                            canScrollX = false;
                            essentialGameObjects.sfxVolume--;
                            essentialGameObjects.SFXObject.GetComponent<AdjustAudio>().updateAudio();
                            sfxText.text = essentialGameObjects.sfxVolume.ToString();
                        }
                    }
                    // Change Fullscreen
                    if (settingsPosition == 2)
                    {
                        if (Input.GetAxis("Horizontal") > 0.5)
                        {
                            canScrollX = false;
                            if (essentialGameObjects.isFullscreen == true)
                            {
                                essentialGameObjects.isFullscreen = false;
                            }
                            else if (essentialGameObjects.isFullscreen == false)
                            {
                                essentialGameObjects.isFullscreen = true;
                            }
                            fullscreenToggle.SetActive(essentialGameObjects.isFullscreen);
                            Screen.fullScreen = essentialGameObjects.isFullscreen;

                        }
                        else if (Input.GetAxis("Horizontal") < -0.5)
                        {
                            canScrollX = false;
                            if (essentialGameObjects.isFullscreen == true)
                            {
                                essentialGameObjects.isFullscreen = false;
                            }
                            else if (essentialGameObjects.isFullscreen == false)
                            {
                                essentialGameObjects.isFullscreen = true;
                            }
                            fullscreenToggle.SetActive(essentialGameObjects.isFullscreen);
                            Screen.fullScreen = essentialGameObjects.isFullscreen;

                        }
                    }
                    // Change DX-2 Spotlights
                    if (settingsPosition == 3)
                    {
                        if (Input.GetAxis("Horizontal") > 0.5)
                        {
                            canScrollX = false;
                            if (essentialGameObjects.showSpotlights == true)
                            {
                                essentialGameObjects.showSpotlights = false;
                            }
                            else if (essentialGameObjects.showSpotlights == false)
                            {
                                essentialGameObjects.showSpotlights = true;
                            }
                            spotlightToggle.SetActive(essentialGameObjects.showSpotlights);

                        }
                        else if (Input.GetAxis("Horizontal") < -0.5)
                        {
                            canScrollX = false;
                            if (essentialGameObjects.showSpotlights == true)
                            {
                                essentialGameObjects.showSpotlights = false;
                            }
                            else if (essentialGameObjects.showSpotlights == false)
                            {
                                essentialGameObjects.showSpotlights = true;
                            }
                            spotlightToggle.SetActive(essentialGameObjects.showSpotlights);

                        }
                    }
                }
                // Restore To Defaults
                if (settingsPosition == 4)
                {
                    if (Input.GetButtonDown("Return"))
                    {
                        Debug.Log("Restore");

                        // Restore BGM Volume
                        essentialGameObjects.bgmVolume = defaultBGMVolume;
                        essentialGameObjects.BGMObject.GetComponent<AdjustAudio>().updateAudio();
                        bgmText.text = essentialGameObjects.bgmVolume.ToString();

                        // Restore SFX Volume
                        essentialGameObjects.sfxVolume = defaultSFXVolume;
                        essentialGameObjects.SFXObject.GetComponent<AdjustAudio>().updateAudio();
                        sfxText.text = essentialGameObjects.sfxVolume.ToString();

                        // Restore Fullscreen
                        essentialGameObjects.isFullscreen = defaultIsFullscreen;
                        fullscreenToggle.SetActive(essentialGameObjects.isFullscreen);
                        Screen.fullScreen = essentialGameObjects.isFullscreen;

                        // Restore DX-2 Spotlights
                        essentialGameObjects.showSpotlights = defaultShowSpotlights;
                        spotlightToggle.SetActive(essentialGameObjects.showSpotlights);
                    }
                }

                if ((Input.GetAxis("Vertical") == 0))
                {
                    canScrollY = true;
                }
                else if (canScrollY == true)
                {
                    // Controller Controls
                    if ((Input.GetAxis("Vertical") > 0.5) && settingsPosition > 0)
                    {
                        canScrollY = false;
                        settingsPositions[settingsPosition].GetComponent<Animator>().SetTrigger("Deselected");
                        settingsPosition--;
                        settingsPositions[settingsPosition].GetComponent<Animator>().SetTrigger("Selected");

                    }
                    else if ((Input.GetAxis("Vertical") > 0.5) && settingsPosition == 0)
                    {
                        canScrollY = false;
                        settingsPositions[settingsPosition].GetComponent<Animator>().SetTrigger("Deselected");
                        settingsPosition = settingsPositions.Length - 1;
                        settingsPositions[settingsPosition].GetComponent<Animator>().SetTrigger("Selected");
                    }
                    else if ((Input.GetAxis("Vertical") < -0.5) && settingsPosition < settingsPositions.Length - 1)
                    {
                        canScrollY = false;
                        settingsPositions[settingsPosition].GetComponent<Animator>().SetTrigger("Deselected");
                        settingsPosition++;
                        settingsPositions[settingsPosition].GetComponent<Animator>().SetTrigger("Selected");
                    }
                    else if ((Input.GetAxis("Vertical") < -0.5) && settingsPosition == settingsPositions.Length - 1)
                    {
                        canScrollY = false;
                        settingsPositions[settingsPosition].GetComponent<Animator>().SetTrigger("Deselected");
                        settingsPosition = 0;
                        settingsPositions[settingsPosition].GetComponent<Animator>().SetTrigger("Selected");
                    }
                }
            }
        }
        else if (essentialGameObjects.GetComponent<ControllerType>().XB1 == true)
        {
            if (inSettingsMenu == false)
            {
                if (Input.GetButtonDown("Return"))
                {
                    // Resume
                    if (position == 0)
                    {
                        Debug.Log("Resume");
                        essentialGameObjects.GetComponent<PauseMenu>().resume();
                    }
                    // Settings
                    if (position == 1)
                    {
                        inSettingsMenu = true;
                        pauseCanvas.GetComponent<Animator>().SetTrigger("Enter Settings");
                        bgmText.text = essentialGameObjects.bgmVolume.ToString();
                        sfxText.text = essentialGameObjects.sfxVolume.ToString();
                        fullscreenToggle.SetActive(essentialGameObjects.isFullscreen);
                        spotlightToggle.SetActive(essentialGameObjects.showSpotlights);

                        settingsPosition = 0;
                        settingsPositions[settingsPosition].GetComponent<Animator>().SetTrigger("Selected");
                    }
                    // Main Menu
                    if (position == 2)
                    {
                        Debug.Log("Main Menu");
                        essentialGameObjects.GetComponent<PauseMenu>().mainMenu();
                        SceneManager.LoadScene(1);
                    }
                }


                if ((Input.GetAxis("Vertical") == 0))
                {
                    canScrollY = true;
                }
                else if (canScrollY == true)
                {
                    // Controller Controls
                    if ((Input.GetAxis("Vertical") > 0.5) && position > 0)
                    {
                        canScrollY = false;
                        positions[position].GetComponent<Animator>().SetTrigger("Deselected");
                        position--;
                        positions[position].GetComponent<Animator>().SetTrigger("Selected");
                    }
                    else if ((Input.GetAxis("Vertical") > 0.5) && position == 0)
                    {
                        canScrollY = false;
                        positions[position].GetComponent<Animator>().SetTrigger("Deselected");
                        position = positions.Length - 1;
                        positions[position].GetComponent<Animator>().SetTrigger("Selected");
                    }
                    else if ((Input.GetAxis("Vertical") < -0.5) && position < positions.Length - 1)
                    {
                        canScrollY = false;
                        positions[position].GetComponent<Animator>().SetTrigger("Deselected");
                        position++;
                        positions[position].GetComponent<Animator>().SetTrigger("Selected");
                    }
                    else if ((Input.GetAxis("Vertical") < -0.5) && position == positions.Length - 1)
                    {
                        canScrollY = false;
                        positions[position].GetComponent<Animator>().SetTrigger("Deselected");
                        position = 0;
                        positions[position].GetComponent<Animator>().SetTrigger("Selected");
                    }
                }
            }
            else if (inSettingsMenu == true)
            {
                if (Input.GetButtonDown("Escape"))
                {
                    writeToSettingsFile();
                    inSettingsMenu = false;
                    settingsPositions[settingsPosition].GetComponent<Animator>().SetTrigger("Deselected");
                    pauseCanvas.GetComponent<Animator>().SetTrigger("Exit Settings");
                }

                if ((Input.GetAxis("Horizontal") == 0))
                {
                    canScrollX = true;
                }
                else if (canScrollX == true)
                {
                    // Change BGM Volume
                    if (settingsPosition == 0)
                    {
                        if (Input.GetAxis("Horizontal") > 0.5 && essentialGameObjects.bgmVolume < essentialGameObjects.bgmMax)
                        {
                            Debug.Log("TESTER");
                            canScrollX = false;
                            essentialGameObjects.bgmVolume++;
                            essentialGameObjects.BGMObject.GetComponent<AdjustAudio>().updateAudio();
                            bgmText.text = essentialGameObjects.bgmVolume.ToString();
                        }
                        else if (Input.GetAxis("Horizontal") < -0.5 && essentialGameObjects.bgmVolume > 0)
                        {
                            canScrollX = false;
                            essentialGameObjects.bgmVolume--;
                            essentialGameObjects.BGMObject.GetComponent<AdjustAudio>().updateAudio();
                            bgmText.text = essentialGameObjects.bgmVolume.ToString();
                        }
                    }
                    // Change SFX Volume
                    if (settingsPosition == 1)
                    {
                        if (Input.GetAxis("Horizontal") > 0.5 && essentialGameObjects.sfxVolume < essentialGameObjects.sfxMax)
                        {
                            canScrollX = false;
                            essentialGameObjects.sfxVolume++;
                            essentialGameObjects.SFXObject.GetComponent<AdjustAudio>().updateAudio();
                            sfxText.text = essentialGameObjects.sfxVolume.ToString();
                        }
                        else if (Input.GetAxis("Horizontal") < -0.5 && essentialGameObjects.sfxVolume > 0)
                        {
                            canScrollX = false;
                            essentialGameObjects.sfxVolume--;
                            essentialGameObjects.SFXObject.GetComponent<AdjustAudio>().updateAudio();
                            sfxText.text = essentialGameObjects.sfxVolume.ToString();
                        }
                    }
                    // Change Fullscreen
                    if (settingsPosition == 2)
                    {
                        if (Input.GetAxis("Horizontal") > 0.5)
                        {
                            canScrollX = false;
                            if (essentialGameObjects.isFullscreen == true)
                            {
                                essentialGameObjects.isFullscreen = false;
                            }
                            else if (essentialGameObjects.isFullscreen == false)
                            {
                                essentialGameObjects.isFullscreen = true;
                            }
                            fullscreenToggle.SetActive(essentialGameObjects.isFullscreen);
                            Screen.fullScreen = essentialGameObjects.isFullscreen;

                        }
                        else if (Input.GetAxis("Horizontal") < -0.5)
                        {
                            canScrollX = false;
                            if (essentialGameObjects.isFullscreen == true)
                            {
                                essentialGameObjects.isFullscreen = false;
                            }
                            else if (essentialGameObjects.isFullscreen == false)
                            {
                                essentialGameObjects.isFullscreen = true;
                            }
                            fullscreenToggle.SetActive(essentialGameObjects.isFullscreen);
                            Screen.fullScreen = essentialGameObjects.isFullscreen;

                        }
                    }
                    // Change DX-2 Spotlights
                    if (settingsPosition == 3)
                    {
                        if (Input.GetAxis("Horizontal") > 0.5)
                        {
                            canScrollX = false;
                            if (essentialGameObjects.showSpotlights == true)
                            {
                                essentialGameObjects.showSpotlights = false;
                            }
                            else if (essentialGameObjects.showSpotlights == false)
                            {
                                essentialGameObjects.showSpotlights = true;
                            }
                            spotlightToggle.SetActive(essentialGameObjects.showSpotlights);

                        }
                        else if (Input.GetAxis("Horizontal") < -0.5)
                        {
                            canScrollX = false;
                            if (essentialGameObjects.showSpotlights == true)
                            {
                                essentialGameObjects.showSpotlights = false;
                            }
                            else if (essentialGameObjects.showSpotlights == false)
                            {
                                essentialGameObjects.showSpotlights = true;
                            }
                            spotlightToggle.SetActive(essentialGameObjects.showSpotlights);

                        }
                    }
                }
                // Restore To Defaults
                if (settingsPosition == 4)
                {
                    if (Input.GetButtonDown("Return"))
                    {
                        Debug.Log("Restore");

                        // Restore BGM Volume
                        essentialGameObjects.bgmVolume = defaultBGMVolume;
                        essentialGameObjects.BGMObject.GetComponent<AdjustAudio>().updateAudio();
                        bgmText.text = essentialGameObjects.bgmVolume.ToString();

                        // Restore SFX Volume
                        essentialGameObjects.sfxVolume = defaultSFXVolume;
                        essentialGameObjects.SFXObject.GetComponent<AdjustAudio>().updateAudio();
                        sfxText.text = essentialGameObjects.sfxVolume.ToString();

                        // Restore Fullscreen
                        essentialGameObjects.isFullscreen = defaultIsFullscreen;
                        fullscreenToggle.SetActive(essentialGameObjects.isFullscreen);
                        Screen.fullScreen = essentialGameObjects.isFullscreen;

                        // Restore DX-2 Spotlights
                        essentialGameObjects.showSpotlights = defaultShowSpotlights;
                        spotlightToggle.SetActive(essentialGameObjects.showSpotlights);
                    }
                }

                if ((Input.GetAxis("Vertical") == 0))
                {
                    canScrollY = true;
                }
                else if (canScrollY == true)
                {
                    // Controller Controls
                    if ((Input.GetAxis("Vertical") > 0.5) && settingsPosition > 0)
                    {
                        canScrollY = false;
                        settingsPositions[settingsPosition].GetComponent<Animator>().SetTrigger("Deselected");
                        settingsPosition--;
                        settingsPositions[settingsPosition].GetComponent<Animator>().SetTrigger("Selected");

                    }
                    else if ((Input.GetAxis("Vertical") > 0.5) && settingsPosition == 0)
                    {
                        canScrollY = false;
                        settingsPositions[settingsPosition].GetComponent<Animator>().SetTrigger("Deselected");
                        settingsPosition = settingsPositions.Length - 1;
                        settingsPositions[settingsPosition].GetComponent<Animator>().SetTrigger("Selected");
                    }
                    else if ((Input.GetAxis("Vertical") < -0.5) && settingsPosition < settingsPositions.Length - 1)
                    {
                        canScrollY = false;
                        settingsPositions[settingsPosition].GetComponent<Animator>().SetTrigger("Deselected");
                        settingsPosition++;
                        settingsPositions[settingsPosition].GetComponent<Animator>().SetTrigger("Selected");
                    }
                    else if ((Input.GetAxis("Vertical") < -0.5) && settingsPosition == settingsPositions.Length - 1)
                    {
                        canScrollY = false;
                        settingsPositions[settingsPosition].GetComponent<Animator>().SetTrigger("Deselected");
                        settingsPosition = 0;
                        settingsPositions[settingsPosition].GetComponent<Animator>().SetTrigger("Selected");
                    }
                }
            }
        }

    }

    private void Keyboard()
    {
        if (inSettingsMenu == false)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                essentialGameObjects.SFXObject.GetComponent<AudioSource>().PlayOneShot(essentialGameObjects.enter);
                // Resume
                if (position == 0)
                {
                    Debug.Log("Resume");
                    essentialGameObjects.GetComponent<PauseMenu>().resume();
                }
                // Settings
                if (position == 1)
                {
                    inSettingsMenu = true;
                    bgmText.text = essentialGameObjects.bgmVolume.ToString();
                    sfxText.text = essentialGameObjects.sfxVolume.ToString();
                    fullscreenToggle.SetActive(essentialGameObjects.isFullscreen);
                    spotlightToggle.SetActive(essentialGameObjects.showSpotlights);

                    pauseCanvas.GetComponent<Animator>().SetTrigger("Enter Settings");

                    settingsPosition = 0;
                    settingsPositions[settingsPosition].GetComponent<Animator>().SetTrigger("Selected");
                }
                // Main Menu
                if (position == 2)
                {
                    Debug.Log("Main Menu");
                    essentialGameObjects.GetComponent<PauseMenu>().mainMenu();
                    SceneManager.LoadScene(1);
                }
            }

            // Controller Controls
            if ((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) && position > 0)
            {
                essentialGameObjects.SFXObject.GetComponent<AudioSource>().PlayOneShot(essentialGameObjects.scroll);
                canScrollY = false;
                positions[position].GetComponent<Animator>().SetTrigger("Deselected");
                position--;
                positions[position].GetComponent<Animator>().SetTrigger("Selected");
            }
            else if ((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) && position == 0)
            {
                essentialGameObjects.SFXObject.GetComponent<AudioSource>().PlayOneShot(essentialGameObjects.scroll);
                canScrollY = false;
                positions[position].GetComponent<Animator>().SetTrigger("Deselected");
                position = positions.Length - 1;
                positions[position].GetComponent<Animator>().SetTrigger("Selected");
            }
            else if ((Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) && position < positions.Length - 1)
            {
                essentialGameObjects.SFXObject.GetComponent<AudioSource>().PlayOneShot(essentialGameObjects.scroll);
                canScrollY = false;
                positions[position].GetComponent<Animator>().SetTrigger("Deselected");
                position++;
                positions[position].GetComponent<Animator>().SetTrigger("Selected");
            }
            else if ((Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) && position == positions.Length - 1)
            {
                essentialGameObjects.SFXObject.GetComponent<AudioSource>().PlayOneShot(essentialGameObjects.scroll);
                canScrollY = false;
                positions[position].GetComponent<Animator>().SetTrigger("Deselected");
                position = 0;
                positions[position].GetComponent<Animator>().SetTrigger("Selected");
            }
        }
        else if (inSettingsMenu == true)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                essentialGameObjects.SFXObject.GetComponent<AudioSource>().PlayOneShot(essentialGameObjects.exit);
                Debug.Log("TEST");
                writeToSettingsFile();
                inSettingsMenu = false;
                settingsPositions[settingsPosition].GetComponent<Animator>().SetTrigger("Deselected");
                pauseCanvas.GetComponent<Animator>().SetTrigger("Exit Settings");
            }

            // Change BGM Volume
            if (settingsPosition == 0)
            {
                if ((Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) && essentialGameObjects.bgmVolume < essentialGameObjects.bgmMax)
                {
                    essentialGameObjects.SFXObject.GetComponent<AudioSource>().PlayOneShot(essentialGameObjects.scroll);
                    canScrollX = false;
                    essentialGameObjects.bgmVolume++;
                    essentialGameObjects.BGMObject.GetComponent<AdjustAudio>().updateAudio();
                    bgmText.text = essentialGameObjects.bgmVolume.ToString();
                }
                else if ((Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)) && essentialGameObjects.bgmVolume > 0)
                {
                    essentialGameObjects.SFXObject.GetComponent<AudioSource>().PlayOneShot(essentialGameObjects.scroll);
                    canScrollX = false;
                    essentialGameObjects.bgmVolume--;
                    essentialGameObjects.BGMObject.GetComponent<AdjustAudio>().updateAudio();
                    bgmText.text = essentialGameObjects.bgmVolume.ToString();
                }
            }
            // Change SFX Volume
            if (settingsPosition == 1)
            {
                if ((Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) && essentialGameObjects.sfxVolume < essentialGameObjects.sfxMax)
                {
                    essentialGameObjects.SFXObject.GetComponent<AudioSource>().PlayOneShot(essentialGameObjects.scroll);
                    canScrollX = false;
                    essentialGameObjects.sfxVolume++;
                    essentialGameObjects.SFXObject.GetComponent<AdjustAudio>().updateAudio();
                    sfxText.text = essentialGameObjects.sfxVolume.ToString();
                }
                else if ((Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)) && essentialGameObjects.sfxVolume > 0)
                {
                    essentialGameObjects.SFXObject.GetComponent<AudioSource>().PlayOneShot(essentialGameObjects.scroll);
                    canScrollX = false;
                    essentialGameObjects.sfxVolume--;
                    essentialGameObjects.SFXObject.GetComponent<AdjustAudio>().updateAudio();
                    sfxText.text = essentialGameObjects.sfxVolume.ToString();
                }
            }
            // Change Fullscreen
            if (settingsPosition == 2)
            {
                if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
                {
                    essentialGameObjects.SFXObject.GetComponent<AudioSource>().PlayOneShot(essentialGameObjects.scroll);
                    canScrollX = false;
                    if (essentialGameObjects.isFullscreen == true)
                    {
                        essentialGameObjects.isFullscreen = false;
                    }
                    else if (essentialGameObjects.isFullscreen == false)
                    {
                        essentialGameObjects.isFullscreen = true;
                    }
                    fullscreenToggle.SetActive(essentialGameObjects.isFullscreen);
                    Screen.fullScreen = essentialGameObjects.isFullscreen;

                }
                else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    essentialGameObjects.SFXObject.GetComponent<AudioSource>().PlayOneShot(essentialGameObjects.scroll);
                    canScrollX = false;
                    if (essentialGameObjects.isFullscreen == true)
                    {
                        essentialGameObjects.isFullscreen = false;
                    }
                    else if (essentialGameObjects.isFullscreen == false)
                    {
                        essentialGameObjects.isFullscreen = true;
                    }
                    fullscreenToggle.SetActive(essentialGameObjects.isFullscreen);
                    Screen.fullScreen = essentialGameObjects.isFullscreen;

                }
            }
            // Change DX-2 Spotlights
            if (settingsPosition == 3)
            {
                if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
                {
                    essentialGameObjects.SFXObject.GetComponent<AudioSource>().PlayOneShot(essentialGameObjects.scroll);
                    canScrollX = false;
                    if (essentialGameObjects.showSpotlights == true)
                    {
                        essentialGameObjects.showSpotlights = false;
                    }
                    else if (essentialGameObjects.showSpotlights == false)
                    {
                        essentialGameObjects.showSpotlights = true;
                    }
                    spotlightToggle.SetActive(essentialGameObjects.showSpotlights);
                }
                else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    essentialGameObjects.SFXObject.GetComponent<AudioSource>().PlayOneShot(essentialGameObjects.scroll);
                    canScrollX = false;
                    if (essentialGameObjects.showSpotlights == true)
                    {
                        essentialGameObjects.showSpotlights = false;
                    }
                    else if (essentialGameObjects.showSpotlights == false)
                    {
                        essentialGameObjects.showSpotlights = true;
                    }
                    spotlightToggle.SetActive(essentialGameObjects.showSpotlights);
                }
            }
            // Restore To Defaults
            if (settingsPosition == 4)
            {
                if (Input.GetKeyDown(KeyCode.Return))
                {
                    essentialGameObjects.SFXObject.GetComponent<AudioSource>().PlayOneShot(essentialGameObjects.enter);
                    Debug.Log("Restore");

                    // Restore BGM Volume
                    essentialGameObjects.bgmVolume = defaultBGMVolume;
                    essentialGameObjects.BGMObject.GetComponent<AdjustAudio>().updateAudio();
                    bgmText.text = essentialGameObjects.bgmVolume.ToString();

                    // Restore SFX Volume
                    essentialGameObjects.sfxVolume = defaultSFXVolume;
                    essentialGameObjects.SFXObject.GetComponent<AdjustAudio>().updateAudio();
                    sfxText.text = essentialGameObjects.sfxVolume.ToString();

                    // Restore Fullscreen
                    essentialGameObjects.isFullscreen = defaultIsFullscreen;
                    fullscreenToggle.SetActive(essentialGameObjects.isFullscreen);
                    Screen.fullScreen = essentialGameObjects.isFullscreen;

                    // Restore DX-2 Spotlights
                    essentialGameObjects.showSpotlights = defaultShowSpotlights;
                    spotlightToggle.SetActive(essentialGameObjects.showSpotlights);
                }
            }

            // Controller Controls
            if ((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) && settingsPosition > 0)
            {
                essentialGameObjects.SFXObject.GetComponent<AudioSource>().PlayOneShot(essentialGameObjects.scroll);
                canScrollY = false;
                settingsPositions[settingsPosition].GetComponent<Animator>().SetTrigger("Deselected");
                settingsPosition--;
                settingsPositions[settingsPosition].GetComponent<Animator>().SetTrigger("Selected");
            }
            else if ((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) && settingsPosition == 0)
            {
                essentialGameObjects.SFXObject.GetComponent<AudioSource>().PlayOneShot(essentialGameObjects.scroll);
                canScrollY = false;
                settingsPositions[settingsPosition].GetComponent<Animator>().SetTrigger("Deselected");
                settingsPosition = settingsPositions.Length - 1;
                settingsPositions[settingsPosition].GetComponent<Animator>().SetTrigger("Selected");
            }
            else if ((Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) && settingsPosition < settingsPositions.Length - 1)
            {
                essentialGameObjects.SFXObject.GetComponent<AudioSource>().PlayOneShot(essentialGameObjects.scroll);
                canScrollY = false;
                settingsPositions[settingsPosition].GetComponent<Animator>().SetTrigger("Deselected");
                settingsPosition++;
                settingsPositions[settingsPosition].GetComponent<Animator>().SetTrigger("Selected");
            }
            else if ((Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) && settingsPosition == settingsPositions.Length - 1)
            {
                essentialGameObjects.SFXObject.GetComponent<AudioSource>().PlayOneShot(essentialGameObjects.scroll);
                canScrollY = false;
                settingsPositions[settingsPosition].GetComponent<Animator>().SetTrigger("Deselected");
                settingsPosition = 0;
                settingsPositions[settingsPosition].GetComponent<Animator>().SetTrigger("Selected");
            }
        }
    }

    public void resetOnStart()
    {
        inSettingsMenu = false;
        position = 0;
        positions[position].GetComponent<Animator>().SetTrigger("Selected");
        settingsMenu.SetActive(false);
    }

    public void readTextFileOnStartUp()
    {
        string[] lines = File.ReadAllLines(Application.dataPath + "/StreamingAssets/Settings.txt");

        string[] splitLine = lines[1].Split(new char[] { '=' });
        essentialGameObjects.bgmVolume = int.Parse(splitLine[1]);

        splitLine = lines[2].Split(new char[] { '=' });
        essentialGameObjects.sfxVolume = int.Parse(splitLine[1]);

        splitLine = lines[3].Split(new char[] { '=' });
        essentialGameObjects.isFullscreen = bool.Parse(splitLine[1]);

        splitLine = lines[4].Split(new char[] { '=' });
        essentialGameObjects.showSpotlights = bool.Parse(splitLine[1]);

        return;
    }

    public void writeToSettingsFile()
    {
        string[] lines = new string[amountOfSettings + 1];

        lines[0] = "[Settings]";
        lines[1] = "BGM Volume=" + essentialGameObjects.bgmVolume;
        lines[2] = "SFX Volume=" + essentialGameObjects.sfxVolume;
        lines[3] = "Is Fullscreen=" + essentialGameObjects.isFullscreen;
        lines[4] = "DX-2 Spotlight Toggle=" + essentialGameObjects.showSpotlights;

        File.WriteAllLines(Application.dataPath + "/StreamingAssets/Settings.txt", lines);
        return;
    }


}
