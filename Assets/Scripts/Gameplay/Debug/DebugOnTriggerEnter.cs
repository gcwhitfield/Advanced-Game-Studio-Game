using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugOnTriggerEnter : MonoBehaviour
{
    public void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger Entered: " + other.name + " has entered the trigger of " + gameObject.name);
    }
}
