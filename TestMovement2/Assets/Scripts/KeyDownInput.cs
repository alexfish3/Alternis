using UnityEngine.Events;
using UnityEngine;

public class KeyDownInput : MonoBehaviour
{

    [SerializeField] KeyCode key;
    [SerializeField] UnityEvent Event;
    bool triggered;
    void Update()
    {
        if(Input.GetKeyDown(key) && !triggered){
            Event.Invoke();
            triggered = true;
        }
        else if(Input.GetKeyUp(key) && triggered) triggered = false;
    }
}
