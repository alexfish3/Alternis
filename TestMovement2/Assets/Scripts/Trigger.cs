using UnityEngine;
using UnityEngine.Events;

public class Trigger : MonoBehaviour
{
    [SerializeField] bool allowInteraction = true;
    [SerializeField] bool disableOnTriggered = true;
    [SerializeField] UnityEvent triggerEvent;
    [SerializeField] UnityEvent failEvent;

    void OnTriggerEnter(Collider other) {
        if(other.CompareTag("Player")){
            triggerEvent.Invoke();
        }   
    }

    public void TriggerEvent()
    {
        if(allowInteraction) triggerEvent.Invoke();
        else failEvent.Invoke();
        
        if(disableOnTriggered) this.gameObject.SetActive(false);
    }

    public void InteractionState(bool allow)
    {
        allowInteraction = allow;
    }
}
