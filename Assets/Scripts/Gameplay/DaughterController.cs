using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.AI;

public class DaughterController : PlayerController
{
    public static DaughterController Instance { get; private set; }

    //public bool hidden { get; private set; } = false;
    public bool hidden;

    private void Awake()
    {
        if (!Instance) Instance = this as DaughterController;
    }

    public void Hide()
    {
        hidden = true;
    }

    private void OnTriggerEnter(Collider collision) {
        // TO-DO: press some button to trigger
        // detect only hide spot
        if (collision.gameObject.CompareTag("HiddenSpot")) {
            //NavMeshAgent monster = GameObject.FindGameObjectWithTag("Monsters").GetComponent<NavMeshAgent>();
            //monster.ResetPath();
            Hide();
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.CompareTag("HiddenSpot"))
        {
            hidden = false;
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

        var gamepad = Gamepad.current;
        var keyboard = Keyboard.current;
        float hor = 0;
        float ver = 0;
        Vector2 movement1 = new Vector2(0,0);
        if(gamepad != null)
        {
            movement1 = Gamepad.current.leftStick.ReadValue();
            hor = Gamepad.current.leftStick.x.ReadValue();  //getting right/left
            ver = Gamepad.current.leftStick.y.ReadValue(); //getting up/down
        }
       
        else
        {
           hor = 0;
           ver = 0;
           bool wkey = keyboard.wKey.isPressed;
           bool dkey = keyboard.dKey.isPressed;
           bool skey = keyboard.sKey.isPressed;
           bool akey = keyboard.aKey.isPressed;

           if(wkey)
           {
               ver++;
           }
           if(dkey)
           {
               hor++;
           }
           if(skey)
           {
               ver--;
           }
           if(akey)
           {
               ver--;
           }
           movement1 = new Vector2(hor,ver);
        }

        if(hor == 0 && ver == 0)
        {
            //animator.SetFloat("Speed", 0);
            animator.SetBool("TurnEast", true);
        }

        else 
        {
            //Vector2 movement2 = movement1.normalized; //making it into a unit vector
            Vector2 movement2 = new Vector2(movement.x, movement.z).normalized;
            float newhor = movement2[0]; //horizontal portion of unit vector
            float newver = movement2[1]; //vertical portion of unit vector
            float angle = Mathf.Rad2Deg*Mathf.Atan(newver/newhor); //create angle using unit vector and make it into degrees

            if(angle <= 10 || angle >= 350) // go east
            {
                animator.SetFloat("Speed", 1);
                ResetAnimatorDirections();
                animator.SetBool("RunW", true);
            }
            else if (angle > 10 && angle < 80) //go northeast
            {
                animator.SetFloat("Speed", 1);
                ResetAnimatorDirections();
                animator.SetBool("RunNW", true);
            }
            else if(angle >= 80 && angle <= 100) //go north
            {
                animator.SetFloat("Speed", 1);
                ResetAnimatorDirections();
                animator.SetBool("RunN", true);
            }
            else if(angle > 100 && angle < 170) //go northwest
            {
                animator.SetFloat("Speed", 1);
                ResetAnimatorDirections();
                animator.SetBool("RunNW", true);
            }
            else if(angle >= 170 && angle <= 190) //go west
            {
                animator.SetFloat("Speed", 1);
                ResetAnimatorDirections();
                animator.SetBool("RunW", true);
            }
            else if(angle > 190 && angle < 260) //go southwest
            {
                animator.SetFloat("Speed", 1);
                ResetAnimatorDirections();
                animator.SetBool("RunSW", true);
            }
            else if(angle >= 260 && angle <= 280) //go south
            {
                animator.SetFloat("Speed", 1);
                ResetAnimatorDirections();
                animator.SetBool("RunS", true);
            }
            else //go southeast
            {
                animator.SetFloat("Speed", 1);
                ResetAnimatorDirections();
                animator.SetBool("RunSW", true);
            }
        }
    }

}
