using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EssentialGameObjects : MonoBehaviour
{
    [Header("Settings Information")]
    public int bgmVolume;
    public int bgmMax;
    public int sfxVolume;
    public int sfxMax;
    public bool isFullscreen;

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
}
