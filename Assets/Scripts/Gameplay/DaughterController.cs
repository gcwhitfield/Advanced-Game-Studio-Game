using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.AI;
using static UnityEngine.InputSystem.InputAction;

public class DaughterController : PlayerController
{
    public static DaughterController Instance { get; private set; }

    [HideInInspector]
    public bool hidden;
    bool isNearHiddenSpot = false;

    public GameObject flashlight;

    public Transform keyMoving;
    private Vector3 prevLookDirection;


    private new void Start()
    {
        base.Start();
        prevLookDirection = lookDirection;
    }

    private void Awake()
    {
        if (!Instance) Instance = this;
        else Destroy(gameObject);
    }

    // called when the daughter presses the "Hide" key
    public void Hide()
    {
        if (isNearHiddenSpot && animator.GetFloat("Speed") < 0.01)
        {
            Debug.Log("Hidden Function works");
            hidden = true;
            animator.SetBool("Hide", true);
            TextDisplayManager.Instance.DaughterContinueToNextLine();
        }
    }

    // called when the player presses the submit button
    public new void Submit()
    {
        base.Submit();
    }

    private new void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);

        // detect only hide spot
        if (other.gameObject.CompareTag("HiddenSpot"))
        {
            isNearHiddenSpot = true;
            TextDisplayManager.Instance.ShowText("Press Q to HIDE", TextDisplayManager.TextType.DAUGHTER);
            return;
        }
    }

    private new void OnTriggerExit(Collider other)
    {
        base.OnTriggerExit(other);

        if (other.gameObject.CompareTag("HiddenSpot"))
        {
            hidden = false;
            isNearHiddenSpot = false;
            animator.SetBool("Hide", false);
        }
    }

    private void ResetAnimatorDirections()
    {
        animator.SetBool("RunNW", false);
        animator.SetBool("RunW", false);
        animator.SetBool("RunSW", false);
        animator.SetBool("RunS", false);
        animator.SetBool("RunN", false);
        animator.SetBool("RunE", false);
        animator.SetBool("RunNE", false);
        animator.SetBool("RunSE", false);
    }

    private new void Update()
    {
        base.Update(); // calls PlayerController.Update()

        // set flashlight look direction. Use Lerp to smoothly transition rotation
        Quaternion desiredRotation = Quaternion.Euler(new Vector3(0, Mathf.Rad2Deg * Mathf.Atan2(lookDirection.x, lookDirection.z), 0));
        Quaternion currRotation = flashlight.transform.rotation;
        flashlight.transform.rotation = Quaternion.Lerp(currRotation, desiredRotation, 0.5f);

        // footstep audio
        AudioManager.Instance.FootstepAudio(gameObject, movement, moveSpeed);

        Vector2 movement2 = new Vector2(movement.x, movement.z).normalized; //getting movement vector from playercontroller.cs
        float newhor = movement2[0]; //horizontal portion of unit vector
        float newver = movement2[1];

        if (newhor == 0 && newver == 0) //character is idle
        {
            animator.SetFloat("Speed", 0);
            //animator.SetBool("TurnEast", true);
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
            //Debug.Log(angle);
            if (angle <= 10 || angle >= 350) // go east, 20 degree angle
            {
                animator.SetFloat("Speed", 1);
                ResetAnimatorDirections();
                animator.SetBool("RunE", true);
            }
            else if (angle > 10 && angle < 80) //go northeast, 70 degree angle
            {
                animator.SetFloat("Speed", 1);
                ResetAnimatorDirections();
                animator.SetBool("RunNE", true);
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
                animator.SetBool("RunSE", true);
            }
        }
        //if daughter moves, unhides
        if (hidden == true && animator.GetFloat("Speed") > 0.01)
        {
            hidden = false;
            animator.SetBool("Hide", false);
        }
    }

    public IEnumerator KeyLock(CallbackContext context)
    {



        WaitForSeconds Wait = new WaitForSeconds(0.3f);
        yield return Wait;
    }
}
