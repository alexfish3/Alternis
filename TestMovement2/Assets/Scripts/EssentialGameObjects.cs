using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EssentialGameObjects : MonoBehaviour
{
    [Header("Settings Information")]
    public GameObject sceneTransition;
    public GameObject PostProcessingBlur;
    public GameObject BGMObject;
    public int bgmVolume;
    public int bgmMax;
    public GameObject SFXObject;
    public int sfxVolume;
    public int sfxMax;
    public bool isFullscreen;
    public bool showSpotlights;

    [Header("UI Gameobjects")]
    public GameObject uiCanvas;
    public GameObject canSwitch;
    public GameObject GasMaskHolder;
    public GameObject MeterUI;
    public GameObject Oxygen;

    [Header("SFX")]
    public AudioClip scroll;
    public AudioClip enter;
    public AudioClip exit;
    public AudioClip worldSwap;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void Awake()
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag("Dont Destroy");

        if (objects.Length > 1)
        {
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(this.gameObject);
    }

    public void updateReferencesToUICanvas()
    {
        uiCanvas = GameObject.FindGameObjectWithTag("UI Gameobject");
        canSwitch = uiCanvas.transform.Find("Can Switch").gameObject;
        GasMaskHolder = uiCanvas.transform.Find("Gas Mask Holder").gameObject.transform.GetChild(0).gameObject;
        MeterUI = uiCanvas.transform.Find("MeterUI").gameObject;
        Oxygen = uiCanvas.transform.Find("Oxygen").gameObject;
    }
}
