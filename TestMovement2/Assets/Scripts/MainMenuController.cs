using System;
using System.IO;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MainMenuController : MonoBehaviour
{
    [Header("Main Menu Scroll Info")]
    [SerializeField] bool inSettingsMenu = false;
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

    bool loadFile = false;
    EssentialGameObjects essentialGameObjects;

    // Start is called before the first frame update
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
        if(loadFile == false)
        {
            readTextFileOnStartUp();

            if (Screen.fullScreen)
            {
                if (essentialGameObjects.isFullscreen == false)
                {
                    Screen.fullScreen = !Screen.fullScreen;
                }
            }
            else if (!Screen.fullScreen)
            {
                if (essentialGameObjects.isFullscreen == true)
                {
                    Screen.fullScreen = !Screen.fullScreen;
                }
            }
            loadFile = true;
        }


        controlMainMenu();
    }

    private void controlMainMenu()
    {
        if(inSettingsMenu == false)
        {
            if (Input.GetButtonDown("Return"))
            {
                // Play
                if (position == 0)
                {
                    Debug.Log("Play");
                    SceneManager.LoadScene(2);
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
                // Quit
                if (position == 2)
                {
                    Application.Quit();
                }
            }


            if ((Input.GetAxis("Vertical") == 0))
            {
                canScrollY = true;
            }
            else if (canScrollY == true)
            {
                // Controller Controls
                if ((Input.GetAxis("Vertical") > 0) && position > 0)
                {
                    canScrollY = false;
                    position--;
                    selector.transform.position = positions[position].transform.position;
                }
                else if ((Input.GetAxis("Vertical") > 0) && position == 0)
                {
                    canScrollY = false;
                    position = positions.Length - 1;
                    selector.transform.position = positions[position].transform.position;
                }
                else if ((Input.GetAxis("Vertical") < 0) && position < positions.Length - 1)
                {
                    canScrollY = false;
                    position++;
                    selector.transform.position = positions[position].transform.position;
                }
                else if ((Input.GetAxis("Vertical") < 0) && position == positions.Length - 1)
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
                if(settingsPosition == 0)
                {
                    if (Input.GetAxis("Horizontal") > 0 && essentialGameObjects.bgmVolume < essentialGameObjects.bgmMax)
                    {
                        canScrollX = false;
                        essentialGameObjects.bgmVolume++;
                        bgmText.text = essentialGameObjects.bgmVolume.ToString();
                    }
                    else if (Input.GetAxis("Horizontal") < 0 && essentialGameObjects.bgmVolume > 0)
                    {
                        canScrollX = false;
                        essentialGameObjects.bgmVolume--;
                        bgmText.text = essentialGameObjects.bgmVolume.ToString();
                    }
                }
                // Change SFX Volume
                if (settingsPosition == 1)
                {
                    if (Input.GetAxis("Horizontal") > 0 && essentialGameObjects.sfxVolume < essentialGameObjects.sfxMax)
                    {
                        canScrollX = false;
                        essentialGameObjects.sfxVolume++;
                        sfxText.text = essentialGameObjects.sfxVolume.ToString();
                    }
                    else if (Input.GetAxis("Horizontal") < 0 && essentialGameObjects.sfxVolume > 0)
                    {
                        canScrollX = false;
                        essentialGameObjects.sfxVolume--;
                        sfxText.text = essentialGameObjects.sfxVolume.ToString();
                    }
                }
                // Change Fullscreen
                if(settingsPosition == 2)
                {
                    if (Input.GetAxis("Horizontal") > 0)
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
                    else if (Input.GetAxis("Horizontal") < 0)
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

            if ((Input.GetAxis("Vertical") == 0))
            {
                canScrollY = true;
            }
            else if (canScrollY == true)
            {
                // Controller Controls
                if ((Input.GetAxis("Vertical") > 0) && settingsPosition > 0)
                {
                    canScrollY = false;
                    settingsPosition--;
                    settingsSelector.transform.position = settingsPositions[settingsPosition].transform.position;
                }
                else if ((Input.GetAxis("Vertical") > 0) && settingsPosition == 0)
                {
                    canScrollY = false;
                    settingsPosition = settingsPositions.Length - 1;
                    settingsSelector.transform.position = settingsPositions[settingsPosition].transform.position;
                }
                else if ((Input.GetAxis("Vertical") < 0) && settingsPosition < settingsPositions.Length - 1)
                {
                    canScrollY = false;
                    settingsPosition++;
                    settingsSelector.transform.position = settingsPositions[settingsPosition].transform.position;
                }
                else if ((Input.GetAxis("Vertical") < 0) && settingsPosition == settingsPositions.Length - 1)
                {
                    canScrollY = false;
                    settingsPosition = 0;
                    settingsSelector.transform.position = settingsPositions[settingsPosition].transform.position;
                }
            }
        }

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

    private void writeToSettingsFile()
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
