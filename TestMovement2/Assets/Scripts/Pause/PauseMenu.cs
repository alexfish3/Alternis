using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public bool canPause = false;
    public bool isPaused = false;
    EssentialGameObjects essentialGameObjects;
    public GameObject pauseMenu;
    [SerializeField] PauseMenuController pauseMenuController;
    [SerializeField] GameObject pauseMenuCanvas;
    [SerializeField] GameObject player;
    [SerializeField] GameObject lightWorldActive;
    [SerializeField] AudioClip mainMenuMusic;

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
        // Resumes Game
        if (isPaused == true)
        {
            StartCoroutine(resumeGameAfterAnim());
        }

        // Pauses Game
        else if (isPaused == false)
        {
            essentialGameObjects.SFXObject.GetComponent<AudioSource>().PlayOneShot(essentialGameObjects.enter);
            essentialGameObjects.canSwitch.SetActive(false);
            essentialGameObjects.GasMaskHolder.GetComponent<Image>().enabled = false;
            essentialGameObjects.MeterUI.SetActive(false);
            essentialGameObjects.Oxygen.SetActive(false);

            this.GetComponent<EssentialGameObjects>().PostProcessingBlur.SetActive(true);
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
        StartCoroutine(resumeGameResume());
    }

    public void mainMenu()
    {
        StartCoroutine(resumeGameMainMenu());
        essentialGameObjects.BGMObject.GetComponent<AudioSource>().Stop();
        essentialGameObjects.BGMObject.GetComponent<AudioSource>().clip = mainMenuMusic;
        essentialGameObjects.BGMObject.GetComponent<AudioSource>().Play();
        essentialGameObjects.BGMObject.GetComponent<AdjustAudio>().pitchToOne();
    }

    private IEnumerator resumeGameAfterAnim()
    {
        essentialGameObjects.SFXObject.GetComponent<AudioSource>().PlayOneShot(essentialGameObjects.exit);
        pauseMenuCanvas.GetComponent<Animator>().SetTrigger("SlideOut");
        yield return new WaitForSecondsRealtime(0.4f);
        this.GetComponent<EssentialGameObjects>().PostProcessingBlur.SetActive(false);

        essentialGameObjects.canSwitch.SetActive(true);
        essentialGameObjects.GasMaskHolder.GetComponent<Image>().enabled = true;
        essentialGameObjects.MeterUI.SetActive(true);
        essentialGameObjects.Oxygen.SetActive(true);

        pauseMenuController.inSettingsMenu = false;
        lightWorldActive.SetActive(true);

        // Sets timescale back to normal time
        Time.timeScale = 1f;
        pauseMenuController.writeToSettingsFile();
        isPaused = false;
        pauseMenu.SetActive(false);
    }

    private IEnumerator resumeGameResume()
    {
        pauseMenuCanvas.GetComponent<Animator>().SetTrigger("SlideOut");
        yield return new WaitForSecondsRealtime(0.4f);
        this.GetComponent<EssentialGameObjects>().PostProcessingBlur.SetActive(false);

        essentialGameObjects.canSwitch.SetActive(true);
        essentialGameObjects.GasMaskHolder.GetComponent<Image>().enabled = true;
        essentialGameObjects.MeterUI.SetActive(true);
        essentialGameObjects.Oxygen.SetActive(true);

        pauseMenuController.inSettingsMenu = false;
        lightWorldActive.SetActive(true);

        // Sets timescale back to normal time
        Time.timeScale = 1f;
        pauseMenuController.writeToSettingsFile();
        isPaused = false;
        pauseMenu.SetActive(false);
    }

    private IEnumerator resumeGameMainMenu()
    {
        pauseMenuCanvas.GetComponent<Animator>().SetTrigger("SlideOut");
        yield return new WaitForSecondsRealtime(0.4f);
        this.GetComponent<EssentialGameObjects>().PostProcessingBlur.SetActive(false);

        Time.timeScale = 1f;
        isPaused = false;
        canPause = false;
        pauseMenu.SetActive(false);
    }
}
