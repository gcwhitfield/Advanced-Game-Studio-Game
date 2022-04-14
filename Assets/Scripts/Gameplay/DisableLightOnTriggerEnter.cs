using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableLightOnTriggerEnter : MonoBehaviour
{
    public GameObject lightToDisable;

    // disables the daughter's light when she collides with this gameobject
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            if (other.GetComponent<DaughterController>())
            { // if we collide with daughter
                lightToDisable.SetActive(false);
                // TODO: play the light disable sound?
            }
        }
    }
}
