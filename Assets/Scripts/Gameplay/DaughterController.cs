using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.AI;

public class DaughterController : PlayerController
{
    public static DaughterController Instance { get; private set; }
    public bool Hidden = false;
    private void Awake()
    {
        if (!Instance) Instance = this as DaughterController;
    }

    private void OnTriggerEnter(Collider collision) {
        // TO-DO: press some button to trigger
        // detect only hide spot
        if (collision.gameObject.CompareTag("HiddenSpot")) {
            //NavMeshAgent monster = GameObject.FindGameObjectWithTag("Monsters").GetComponent<NavMeshAgent>();
            //monster.ResetPath();
            Hidden = true;
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.CompareTag("HiddenSpot"))
        {
            Hidden = false;
        }
    }

}
