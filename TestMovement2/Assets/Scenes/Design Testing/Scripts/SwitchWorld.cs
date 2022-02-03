using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class SwitchWorld : MonoBehaviour
{
    public GameObject world1;
    public GameObject world2;

    public bool toggle;

    void Update()
    {
        if(toggle){
            world1.SetActive(!world1.activeSelf);
            world2.SetActive(!world2.activeSelf);
            toggle = false;
        }
    }
}
