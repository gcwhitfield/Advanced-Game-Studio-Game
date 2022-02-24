using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.AI;
using FMODUnity;

public class DaughterController : PlayerController
{
    public static DaughterController Instance { get; private set; }

    // 'collectableObject' is a refernece to the item that the daughter can currently collect
    // if it is null, then the daughter is not standing nearby any collectbale objects
    // if it is not null, then 'collectableObject' will refer to the Collectable component that
    // she can collect.
    private Collectable collectableObject = null;

    [HideInInspector]
    public bool hidden;

    public EventReference footStepAudio;
    private FMOD.Studio.EventInstance footStepInstance;
    private float timer = 0.0f;

    private void Awake()
    {
        if (!Instance) Instance = this as DaughterController;

        footStepInstance = RuntimeManager.CreateInstance(footStepAudio);
        RuntimeManager.AttachInstanceToGameObject(footStepInstance, GetComponent<Transform>());
        //InvokeRepeating("PlayFootstepAudio", 0, 0.75f);
    }

    public void PlayFootstepAudio()
    {
        footStepInstance.set3DAttributes(RuntimeUtils.To3DAttributes(transform.position));
        if (movement != Vector3.zero)
        {
            if (timer > moveSpeed / 9)
            {
                footStepInstance.start();
                //footStepInstance.release();
                timer = 0.0f;
            }
            timer += Time.deltaTime;
        }
    }

    // called when the daughter presses the "Hide" key
    public void Hide()
    {
        hidden = true;
    }

    // called when the daughter presses the "Collect" key
    public void Collect()
    {
        Debug.Log("Collect");
        if (collectableObject)
        {
            collectableObject.Collect();
            Destroy(collectableObject.gameObject);
            collectableObject = null;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // TO-DO: press some button to trigger
        // detect only hide spot
        if (other.gameObject.CompareTag("HiddenSpot"))
        {
            Hide();
            return;
        }

        Collectable c = other.GetComponent<Collectable>();
        if (c)
        {
            Debug.Log("Collectable has been set");
            collectableObject = c;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("HiddenSpot"))
        {
            hidden = false;
        }

        Collectable c = other.GetComponent<Collectable>();
        if (c)
        {
            collectableObject = null;
        }
    }

    private void ResetAnimatorDirections()
    {
        animator.SetBool("RunNW", false);
        animator.SetBool("RunW", false);
        animator.SetBool("RunSW", false);
        animator.SetBool("RunS", false);
        animator.SetBool("RunN", false);
    }

    private new void Update()
    {
        base.Update(); // calls PlayerController.Update()
        PlayFootstepAudio();

        Vector2 movement2 = new Vector2(movement.x, movement.z).normalized; //getting movement vector from playercontroller.cs
        float newhor = movement2[0]; //horizontal portion of unit vector
        float newver = movement2[1];

        if (newhor == 0 && newver == 0) //character is idle
        {
            //animator.SetFloat("Speed", 0);
            animator.SetBool("TurnEast", true);
        }
        else
        {
            //Vector2 movement2 = movement1.normalized; //making it into a unit vector
            /*Vector2 movement2 = new Vector2(movement.x, movement.z).normalized; //getting movement vector from playercontroller.cs
            float newhor = movement2[0]; //horizontal portion of unit vector
            float newver = movement2[1]; //vertical portion of unit vector */
            float angle = Mathf.Rad2Deg * Mathf.Atan2(newver, newhor); //create angle using unit vector and make it into degrees
            if (angle < 0)
            {
                angle = 360 + angle;
            }
            Debug.Log(angle);
            if (angle <= 10 || angle >= 350) // go east, 20 degree angle
            {
                animator.SetFloat("Speed", 1);
                ResetAnimatorDirections();
                animator.SetBool("RunW", true);
            }
            else if (angle > 10 && angle < 80) //go northeast, 70 degree angle
            {
                animator.SetFloat("Speed", 1);
                ResetAnimatorDirections();
                animator.SetBool("RunNW", true);
            }
            else if (angle >= 80 && angle <= 100) //go north, 20 degree angle
            {
                animator.SetFloat("Speed", 1);
                ResetAnimatorDirections();
                animator.SetBool("RunN", true);
            }
            else if (angle > 100 && angle < 170) //go northwest, 70 degree angle
            {
                animator.SetFloat("Speed", 1);
                ResetAnimatorDirections();
                animator.SetBool("RunNW", true);
            }
            else if (angle >= 170 && angle <= 190) //go west, 20 degree angle
            {
                animator.SetFloat("Speed", 1);
                ResetAnimatorDirections();
                animator.SetBool("RunW", true);
            }
            else if (angle > 190 && angle < 260) //go southwest, 70 degree angle
            {
                animator.SetFloat("Speed", 1);
                ResetAnimatorDirections();
                animator.SetBool("RunSW", true);
            }
            else if (angle >= 260 && angle <= 280) //go south, 20 degree angle
            {
                animator.SetFloat("Speed", 1);
                ResetAnimatorDirections();
                animator.SetBool("RunS", true);
            }
            else //go southeast, 70 degree angle
            {
                animator.SetFloat("Speed", 1);
                ResetAnimatorDirections();
                animator.SetBool("RunSW", true);
            }
        }
    }
}