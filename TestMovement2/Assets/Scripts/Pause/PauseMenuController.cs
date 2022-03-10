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
    public bool inSettingsMenu = false;
    [SerializeField] GameObject selector;
    [SerializeField] GameObject[] positions;
    [SerializeField] int position;
    [SerializeField] bool canScrollY = true;
    [SerializeField] bool canScrollX = true;

    [Header("Settings Menu")]
    [SerializeField] int amountOfSettings;
    [SerializeField] GameObject settingsMenu;
    [SerializeField] GameObject settingsSelector;
    [SerializeField] GameObject[] settingsPositions;
    [SerializeField] int settingsPosition;
    [SerializeField] TMP_Text bgmText;
    [SerializeField] TMP_Text sfxText;
    [SerializeField] GameObject fullscreenToggle;

    [Header("Default Settings")]
    [SerializeField] int defaultBGMVolume;
    [SerializeField] int defaultSFXVolume;
    [SerializeField] bool defaultIsFullscreen;

    bool loadFile = false;
    EssentialGameObjects essentialGameObjects;

    void Start()
    {
        essentialGameObjects = GameObject.FindWithTag("Dont Destroy").GetComponent<EssentialGameObjects>();

        position = 0;
        selector.transform.position = positions[position].transform.position;

        settingsPosition = 0;
        settingsSelector.transform.position = settingsPositions[settingsPosition].transform.position;
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
                        settingsMenu.SetActive(true);
                        bgmText.text = essentialGameObjects.bgmVolume.ToString();
                        sfxText.text = essentialGameObjects.sfxVolume.ToString();
                        fullscreenToggle.SetActive(essentialGameObjects.isFullscreen);

                        settingsPosition = 0;
                        settingsSelector.transform.position = settingsPositions[settingsPosition].transform.position;
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
                        position--;
                        selector.transform.position = positions[position].transform.position;
                    }
                    else if ((Input.GetAxis("Vertical") > 0.5) && position == 0)
                    {
                        canScrollY = false;
                        position = positions.Length - 1;
                        selector.transform.position = positions[position].transform.position;
                    }
                    else if ((Input.GetAxis("Vertical") < -0.5) && position < positions.Length - 1)
                    {
                        canScrollY = false;
                        position++;
                        selector.transform.position = positions[position].transform.position;
                    }
                    else if ((Input.GetAxis("Vertical") < -0.5) && position == positions.Length - 1)
                    {
                        canScrollY = false;
                        position = 0;
                        selector.transform.position = positions[position].transform.position;
                    }
                }
            }
            else if (inSettingsMenu == true)
            {
                if (Input.GetButtonDown("Escape"))
                {
                    writeToSettingsFile();
                    inSettingsMenu = false;
                    settingsMenu.SetActive(false);
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
                            bgmText.text = essentialGameObjects.bgmVolume.ToString();
                        }
                        else if (Input.GetAxis("Horizontal") < -0.5 && essentialGameObjects.bgmVolume > 0)
                        {
                            canScrollX = false;
                            essentialGameObjects.bgmVolume--;
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
                            sfxText.text = essentialGameObjects.sfxVolume.ToString();
                        }
                        else if (Input.GetAxis("Horizontal") < -0.5 && essentialGameObjects.sfxVolume > 0)
                        {
                            canScrollX = false;
                            essentialGameObjects.sfxVolume--;
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

                }
                // Restore To Defaults
                if (settingsPosition == 3)
                {
                    if (Input.GetButtonDown("Return"))
                    {
                        Debug.Log("Restore");

                        // Restore BGM Volume
                        essentialGameObjects.bgmVolume = defaultBGMVolume;
                        bgmText.text = essentialGameObjects.bgmVolume.ToString();

                        // Restore SFX Volume
                        essentialGameObjects.sfxVolume = defaultSFXVolume;
                        sfxText.text = essentialGameObjects.sfxVolume.ToString();

                        // Restore Fullscreen
                        essentialGameObjects.isFullscreen = defaultIsFullscreen;
                        fullscreenToggle.SetActive(essentialGameObjects.isFullscreen);
                        Screen.fullScreen = essentialGameObjects.isFullscreen;
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
                        settingsPosition--;
                        settingsSelector.transform.position = settingsPositions[settingsPosition].transform.position;
                    }
                    else if ((Input.GetAxis("Vertical") > 0.5) && settingsPosition == 0)
                    {
                        canScrollY = false;
                        settingsPosition = settingsPositions.Length - 1;
                        settingsSelector.transform.position = settingsPositions[settingsPosition].transform.position;
                    }
                    else if ((Input.GetAxis("Vertical") < -0.5) && settingsPosition < settingsPositions.Length - 1)
                    {
                        canScrollY = false;
                        settingsPosition++;
                        settingsSelector.transform.position = settingsPositions[settingsPosition].transform.position;
                    }
                    else if ((Input.GetAxis("Vertical") < -0.5) && settingsPosition == settingsPositions.Length - 1)
                    {
                        canScrollY = false;
                        settingsPosition = 0;
                        settingsSelector.transform.position = settingsPositions[settingsPosition].transform.position;
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
                        settingsMenu.SetActive(true);
                        bgmText.text = essentialGameObjects.bgmVolume.ToString();
                        sfxText.text = essentialGameObjects.sfxVolume.ToString();
                        fullscreenToggle.SetActive(essentialGameObjects.isFullscreen);

                        settingsPosition = 0;
                        settingsSelector.transform.position = settingsPositions[settingsPosition].transform.position;
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
                        position--;
                        selector.transform.position = positions[position].transform.position;
                    }
                    else if ((Input.GetAxis("Vertical") > 0.5) && position == 0)
                    {
                        canScrollY = false;
                        position = positions.Length - 1;
                        selector.transform.position = positions[position].transform.position;
                    }
                    else if ((Input.GetAxis("Vertical") < -0.5) && position < positions.Length - 1)
                    {
                        canScrollY = false;
                        position++;
                        selector.transform.position = positions[position].transform.position;
                    }
                    else if ((Input.GetAxis("Vertical") < -0.5) && position == positions.Length - 1)
                    {
                        canScrollY = false;
                        position = 0;
                        selector.transform.position = positions[position].transform.position;
                    }
                }
            }
            else if (inSettingsMenu == true)
            {
                if (Input.GetButtonDown("Escape"))
                {
                    writeToSettingsFile();
                    inSettingsMenu = false;
                    settingsMenu.SetActive(false);
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
                            canScrollX = false;
                            essentialGameObjects.bgmVolume++;
                            bgmText.text = essentialGameObjects.bgmVolume.ToString();
                        }
                        else if (Input.GetAxis("Horizontal") < -0.5 && essentialGameObjects.bgmVolume > 0)
                        {
                            canScrollX = false;
                            essentialGameObjects.bgmVolume--;
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
                            sfxText.text = essentialGameObjects.sfxVolume.ToString();
                        }
                        else if (Input.GetAxis("Horizontal") < -0.5 && essentialGameObjects.sfxVolume > 0)
                        {
                            canScrollX = false;
                            essentialGameObjects.sfxVolume--;
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

                }
                // Restore To Defaults
                if (settingsPosition == 3)
                {
                    if (Input.GetButtonDown("Return"))
                    {
                        Debug.Log("Restore");

                        // Restore BGM Volume
                        essentialGameObjects.bgmVolume = defaultBGMVolume;
                        bgmText.text = essentialGameObjects.bgmVolume.ToString();

                        // Restore SFX Volume
                        essentialGameObjects.sfxVolume = defaultSFXVolume;
                        sfxText.text = essentialGameObjects.sfxVolume.ToString();

                        // Restore Fullscreen
                        essentialGameObjects.isFullscreen = defaultIsFullscreen;
                        fullscreenToggle.SetActive(essentialGameObjects.isFullscreen);
                        Screen.fullScreen = essentialGameObjects.isFullscreen;
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
                        settingsPosition--;
                        settingsSelector.transform.position = settingsPositions[settingsPosition].transform.position;
                    }
                    else if ((Input.GetAxis("Vertical") > 0.5) && settingsPosition == 0)
                    {
                        canScrollY = false;
                        settingsPosition = settingsPositions.Length - 1;
                        settingsSelector.transform.position = settingsPositions[settingsPosition].transform.position;
                    }
                    else if ((Input.GetAxis("Vertical") < -0.5) && settingsPosition < settingsPositions.Length - 1)
                    {
                        canScrollY = false;
                        settingsPosition++;
                        settingsSelector.transform.position = settingsPositions[settingsPosition].transform.position;
                    }
                    else if ((Input.GetAxis("Vertical") < -0.5) && settingsPosition == settingsPositions.Length - 1)
                    {
                        canScrollY = false;
                        settingsPosition = 0;
                        settingsSelector.transform.position = settingsPositions[settingsPosition].transform.position;
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
                    settingsMenu.SetActive(true);
                    bgmText.text = essentialGameObjects.bgmVolume.ToString();
                    sfxText.text = essentialGameObjects.sfxVolume.ToString();
                    fullscreenToggle.SetActive(essentialGameObjects.isFullscreen);

                    settingsPosition = 0;
                    settingsSelector.transform.position = settingsPositions[settingsPosition].transform.position;
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
                canScrollY = false;
                position--;
                selector.transform.position = positions[position].transform.position;
            }
            else if ((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) && position == 0)
            {
                canScrollY = false;
                position = positions.Length - 1;
                selector.transform.position = positions[position].transform.position;
            }
            else if ((Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) && position < positions.Length - 1)
            {
                canScrollY = false;
                position++;
                selector.transform.position = positions[position].transform.position;
            }
            else if ((Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) && position == positions.Length - 1)
            {
                canScrollY = false;
                position = 0;
                selector.transform.position = positions[position].transform.position;
            }
        }
        else if (inSettingsMenu == true)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Debug.Log("TEST");
                writeToSettingsFile();
                inSettingsMenu = false;
                settingsMenu.SetActive(false);
            }

            // Change BGM Volume
            if (settingsPosition == 0)
            {
                if ((Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) && essentialGameObjects.bgmVolume < essentialGameObjects.bgmMax)
                {
                    canScrollX = false;
                    essentialGameObjects.bgmVolume++;
                    bgmText.text = essentialGameObjects.bgmVolume.ToString();
                }
                else if ((Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)) && essentialGameObjects.bgmVolume > 0)
                {
                    canScrollX = false;
                    essentialGameObjects.bgmVolume--;
                    bgmText.text = essentialGameObjects.bgmVolume.ToString();
                }
            }
            // Change SFX Volume
            if (settingsPosition == 1)
            {
                if ((Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) && essentialGameObjects.sfxVolume < essentialGameObjects.sfxMax)
                {
                    canScrollX = false;
                    essentialGameObjects.sfxVolume++;
                    sfxText.text = essentialGameObjects.sfxVolume.ToString();
                }
                else if ((Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)) && essentialGameObjects.sfxVolume > 0)
                {
                    canScrollX = false;
                    essentialGameObjects.sfxVolume--;
                    sfxText.text = essentialGameObjects.sfxVolume.ToString();
                }
            }
            // Change Fullscreen
            if (settingsPosition == 2)
            {
                if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
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
                else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
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
            // Restore To Defaults
            if (settingsPosition == 3)
            {
                if (Input.GetKeyDown(KeyCode.Return))
                {
                    Debug.Log("Restore");

                    // Restore BGM Volume
                    essentialGameObjects.bgmVolume = defaultBGMVolume;
                    bgmText.text = essentialGameObjects.bgmVolume.ToString();

                    // Restore SFX Volume
                    essentialGameObjects.sfxVolume = defaultSFXVolume;
                    sfxText.text = essentialGameObjects.sfxVolume.ToString();

                    // Restore Fullscreen
                    essentialGameObjects.isFullscreen = defaultIsFullscreen;
                    fullscreenToggle.SetActive(essentialGameObjects.isFullscreen);
                    Screen.fullScreen = essentialGameObjects.isFullscreen;
                }
            }

            // Controller Controls
            if ((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) && settingsPosition > 0)
            {
                canScrollY = false;
                settingsPosition--;
                settingsSelector.transform.position = settingsPositions[settingsPosition].transform.position;
            }
            else if ((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) && settingsPosition == 0)
            {
                canScrollY = false;
                settingsPosition = settingsPositions.Length - 1;
                settingsSelector.transform.position = settingsPositions[settingsPosition].transform.position;
            }
            else if ((Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) && settingsPosition < settingsPositions.Length - 1)
            {
                canScrollY = false;
                settingsPosition++;
                settingsSelector.transform.position = settingsPositions[settingsPosition].transform.position;
            }
            else if ((Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) && settingsPosition == settingsPositions.Length - 1)
            {
                canScrollY = false;
                settingsPosition = 0;
                settingsSelector.transform.position = settingsPositions[settingsPosition].transform.position;
            }
        }
    }

    public void resetOnStart()
    {
        inSettingsMenu = false;
        position = 0;
        selector.transform.position = positions[position].transform.position;
        settingsMenu.SetActive(false);
    }

    private void readTextFileOnStartUp()
    {
        string[] lines = File.ReadAllLines(Application.dataPath + "/StreamingAssets/Settings.txt");

        string[] splitLine = lines[1].Split(new char[] { '=' });
        essentialGameObjects.bgmVolume = int.Parse(splitLine[1]);

        splitLine = lines[2].Split(new char[] { '=' });
        essentialGameObjects.sfxVolume = int.Parse(splitLine[1]);

        splitLine = lines[3].Split(new char[] { '=' });
        essentialGameObjects.isFullscreen = bool.Parse(splitLine[1]);

        return;
    }

    public void writeToSettingsFile()
    {
        string[] lines = new string[amountOfSettings + 1];

        lines[0] = "[Settings]";
        lines[1] = "BGM Volume=" + essentialGameObjects.bgmVolume;
        lines[2] = "SFX Volume=" + essentialGameObjects.sfxVolume;
        lines[3] = "Is Fullscreen=" + essentialGameObjects.isFullscreen;

        File.WriteAllLines(Application.dataPath + "/StreamingAssets/Settings.txt", lines);
        return;
    }


}