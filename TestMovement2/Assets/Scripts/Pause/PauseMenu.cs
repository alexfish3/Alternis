using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public bool canPause = false;
    public bool isPaused = false;
    EssentialGameObjects essentialGameObjects;
    public GameObject pauseMenu;
    [SerializeField] PauseMenuController pauseMenuController;
    [SerializeField] GameObject player;
    [SerializeField] GameObject lightWorldActive;

    // Start is called before the first frame update
    void Start()
    {
        essentialGameObjects = GameObject.FindWithTag("Dont Destroy").GetComponent<EssentialGameObjects>();
    }

    // Update is called once per frame
    void Update()
    {
        if(canPause == true)
        {
            Pause();
        }
    }

    public void Pause()
    {
        // Keyboard controls
        if (Input.GetKeyDown(KeyCode.Escape) && pauseMenuController.inSettingsMenu == false)
        {
            PauseChecker();
        }

        // ps4 controls
        if (essentialGameObjects.GetComponent<ControllerType>().PS4 == true)
        {
            if (Input.GetButtonDown("Pause"))
            {
                PauseChecker();
            }
        }
        // xb1 controls
        else if (essentialGameObjects.GetComponent<ControllerType>().XB1 == true)
        {
            if (Input.GetButtonDown("Pause"))
            {
                PauseChecker();
            }
        }
    }

    // This is the pause. sets the time scale to 0 or 1 depending on the bool isPaused
    private void PauseChecker()
    {

        Debug.Log("Pause Checler");
        // Resumes Game
        if (isPaused == true)
        {
            pauseMenuController.inSettingsMenu = false;
            lightWorldActive.SetActive(true);

            // Sets timescale back to normal time
            Time.timeScale = 1f;
            pauseMenuController.writeToSettingsFile();
            isPaused = false;
            pauseMenu.SetActive(false);
        }

        // Pauses Game
        else if (isPaused == false)
        {
            player = GameObject.FindWithTag("Player").gameObject;
            lightWorldActive = GameObject.FindWithTag("LightWorldActive").gameObject;
            if (player.GetComponent<WorldSwap>().darkWorld == true)
            {
                lightWorldActive.SetActive(false);
            }

            // Pauses timescale, makes it freeze
            Time.timeScale = 0f;
            isPaused = true;
            pauseMenu.SetActive(true);
            pauseMenuController.resetOnStart();
        }
    }

    public void resume()
    {
        pauseMenuController.inSettingsMenu = false;
        lightWorldActive.SetActive(true);

        // Sets timescale back to normal time
        Time.timeScale = 1f;
        pauseMenuController.writeToSettingsFile();
        isPaused = false;
        pauseMenu.SetActive(false);
    }

    public void mainMenu()
    {
        Time.timeScale = 1f;
        isPaused = false;
        canPause = false;
        pauseMenu.SetActive(false);
    }
}
