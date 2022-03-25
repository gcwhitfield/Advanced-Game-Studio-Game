using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

public class FatherController : PlayerController
{
    public static FatherController Instance { get; private set; }

    public Transform fireSpawn;
    public GameObject bulletPrefab;
    public float bulletForce = 20.0f;

    private int codeInputCount = 0;
    private bool inputCodeFlag = true;

    private void Awake()
    {
        if (!Instance) Instance = this;
        else Destroy(gameObject);
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

    public new void Update()
    {
        base.Update();

        // play the footstep audio
        AudioManager.Instance.FootstepAudio(gameObject, movement, moveSpeed);
        // play the lantern sound
        AudioManager.Instance.LanternWalkingAudio(gameObject, movement);

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
    }

    public void Shoot()
    {
        animator.SetTrigger("Shoot");
        // OLD shooting code (instantiates bullet prefab)
        float rayLength = 3.0f;
        RaycastHit hit;
        if (Physics.Raycast(gameObject.transform.position, lookDirection * rayLength, out hit))
        {
            DestroyableBranches branches = hit.transform.GetComponent<DestroyableBranches>();
            if (branches)
            {
                branches.DestroyBranches();
            }
        }

        // play the shooting sound
        AudioManager.Instance.ShootAudio(gameObject);
    }

    public void InputCode(CallbackContext context)
    {
        if (context.ReadValue<float>() > 0 && inputCodeFlag)
        {
            string button = context.control.ToString();
            //Debug.Log(button);
            if (button.Contains("/Keyboard/h"))
            {
                Debug.Log("h pressed");
                codeInputCount++;
            }
            if (button.Contains("/Keyboard/j"))
            {
                Debug.Log("j pressed");
                codeInputCount++;
            }
            if (button.Contains("/Keyboard/k"))
            {
                Debug.Log("k pressed");
                codeInputCount++;
            }
            if (button.Contains("/Keyboard/l"))
            {
                Debug.Log("l pressed");
                codeInputCount++;
            }

            if (codeInputCount > 3)
            {
                codeInputCount = 0;
                for (int i = 1; i < 4; i++)
                {
                    GameObject asterisk = GameObject.Find("Asterisk" + i);
                    if (asterisk != null)
                    {
                        asterisk.gameObject.GetComponent<UnityEngine.UI.Image>().enabled = false;
                    }
                }
            }
            else
            {
                GameObject asterisk = GameObject.Find("Asterisk" + codeInputCount);
                if (asterisk != null)
                {
                    asterisk.GetComponent<UnityEngine.UI.Image>().enabled = true;
                }
            }

            // play some input sound
            AudioManager.Instance.InputCodeAudio(gameObject);
        }
    }
}