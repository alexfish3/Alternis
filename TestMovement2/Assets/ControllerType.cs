using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ControllerType : MonoBehaviour
{
    [SerializeField] TMP_Text identifier;
    public bool keyboard;
    public bool PS4;
    public bool XB1;

    void Update()
    {
        string[] names = Input.GetJoystickNames();
        for (int x = 0; x < names.Length; x++)
        {

            if (names[x].Length == 0)
            {
                identifier.text = "Keyboard";
                keyboard = true;
                PS4 = false;
                XB1 = false;
            }
            if (names[x].Length == 19)
            {
                identifier.text = "Playstation";
                keyboard = false;
                PS4 = true;
                XB1 = false;
            }
            if (names[x].Length == 33)
            {
                identifier.text = "XBox";
                keyboard = false;
                PS4 = false;
                XB1 = true;
            }
        }
    }

}
