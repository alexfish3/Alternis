using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlurManager : MonoBehaviour
{
    public Camera blurCamera;
    public Material blurMaterial;
    [SerializeField] GameObject player;

    // Start is called before the first frame update
    private void Update()
    {
        if(blurCamera == null)
        {
            player = GameObject.FindWithTag("Player").gameObject;
            blurCamera = player.GetComponent<WorldSwap>().blurCamera.GetComponent<Camera>();

            if (blurCamera.targetTexture != null)
            {
                blurCamera.targetTexture.Release();
            }
            blurCamera.targetTexture = new RenderTexture(Screen.width, Screen.height, 24, RenderTextureFormat.ARGB32, 1);
            blurMaterial.SetTexture("_RenTex", blurCamera.targetTexture);
        }
    }


}
