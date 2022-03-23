using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// a gameobject with the "Interactable" component will execute the functions in 'events' when a player
// collides with the object.
public class Interactable : MonoBehaviour
{
    public delegate void Event();
    Event triggerEvents;
    Event interactEvents;

    protected void ExecuteInteractionEvents()
    {
        if (interactEvents != null)
        {
            interactEvents();
            interactEvents = null;
        }
    }

    public void ExecuteOnInteract(Event e)
    {
        interactEvents += e;
    }

    public void CancelExecuteOnInteract(Event e)
    {
        interactEvents -= e;
    }

    protected void ExecuteTriggerEvents()
    {
        if (triggerEvents != null)
        {
            triggerEvents();
        }
        triggerEvents = null;
    }

    public void ExecuteOnTriggerEnter(Event e)
    {
        triggerEvents += e;
    }

    public void CancelExecuteOnTriggerEnter(Event e)
    {
        triggerEvents -= e;
    }

    protected void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            ExecuteTriggerEvents();
        }
    }
}
