using System;
using UnityEngine;

[ExecuteInEditMode]
public class Transformer : MonoBehaviour
{
    //Transform Object position to different points in editorMode
    [Header("~Settings~")]
    public Transform Object;

    [Tooltip("Positions for Object")]
    public Transform[] positions;
    
    [Space(5)]

    [Tooltip("Position number to transform to")]
    public int positionNumber;
    public bool increase, decrease;
    public bool execute;
   
   void Update() 
   {
       if(execute) UpdatePosition();

       if(increase) ChangePositionNumber(increase);
       if(decrease) ChangePositionNumber(!decrease);
    }

    private void ChangePositionNumber(bool increment)
    {
        if(increment) positionNumber ++;
        else if(!increment) positionNumber --;
        increase = false;
        decrease = false;
    }

    public void UpdatePosition()
    {
        Object.localPosition = positions[positionNumber].localPosition;
        Object.localRotation = positions[positionNumber].localRotation;
        execute = false;
    }

    public void UpdateRuntimePosition(int position)
    {
        Object.localPosition = positions[position].localPosition;
        Object.localRotation = positions[position].localRotation;
        execute = false;
    }
}
