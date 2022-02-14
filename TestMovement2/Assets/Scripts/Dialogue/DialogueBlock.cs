using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Dialgoue", menuName = "Data Objects/Dialogue Object", order = 0)]
public class DialogueBlock : ScriptableObject
{
    public bool isProtag;
    public Sprite imageOfPerson;
    public string nameOfPerson;
    [TextArea(5,5)]
    public string dialogueOfPerson;
}
