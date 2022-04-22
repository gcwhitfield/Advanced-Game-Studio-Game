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
    public GameObject codeInputUI;
    public List<GameObject> asterisks = new List<GameObject>();
    public List<GameObject> recBox = new List<GameObject>();
    public Animator FenceArt;

    [HideInInspector]
    public bool inputCodeFlag = false;

    private int codeInputCount = 0;
    private string CODE = "347";
    private string code = "";
    private int numberCount = 1;
    private Vector2 numberPos = new Vector2(0, 0);
    private Vector2 moveLength;
    private Vector3 recPosInit;
    private GameObject fenceGb;

    private void Awake()
    {
        if (!Instance) Instance = this;
        else Destroy(gameObject);
        inputCodeFlag = false;
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
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Father Shoot N"))
        {
            return;
        }
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Father Shoot NW"))
        {
            return;
        }
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Father Shoot E"))
        {
            return;
        }
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Father Shoot NE"))
        {
            return;
        }
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Father Shoot SE"))
        {
            return;
        }
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Father Shoot SW"))
        {
            return;
        }
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Father Shoot S"))
        {
            return;
        }
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Father Shoot W"))
        {
            return;
        }
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
        float rayLength = 10.0f;
        RaycastHit hit;
        Vector3 offset = new Vector3(0.0f, 1.0f, 0.0f);
        //Debug.DrawRay(gameObject.transform.position + offset, lookDirection * rayLength, Color.white, 1, true);
        if (Physics.Raycast(gameObject.transform.position + offset, lookDirection * rayLength, out hit))
        {
            DestroyableBranches branches = hit.transform.GetComponent<DestroyableBranches>();
            if (branches)
            {
                branches.DestroyBranches();
            }
            MonsterControllerLevel3 monster = hit.transform.GetComponent<MonsterControllerLevel3>();
            if (monster)
            {
                monster.OnMonsterAttacked();
            }
        }

        // play the shooting sound
        AudioManager.Instance.ShootAudio(gameObject);

        // if we hit monster (in level 3) then tell the monster to go away
    }

    private new void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);

        // detect only lock
        if (other.gameObject.CompareTag("Lock"))
        {
            fenceGb = other.gameObject;
            inputCodeFlag = true;
            codeInputUI.SetActive(true);
        }
    }

    public IEnumerator InputCode(CallbackContext context)
    {
        recPosInit = recBox[1].transform.position;
        moveLength = recBox[2].transform.position - recPosInit;
        if (context.ReadValue<float>() > 0 && inputCodeFlag)
        {
            string button = context.control.ToString();
            Debug.Log(button);

            Vector3 movement = new Vector2(0, 0);

            if (button.Contains("/Keyboard/a") || button.Contains("dpad/left"))
            {
                movement = new Vector2(-1, 0);
            }
            if (button.Contains("/Keyboard/s") || button.Contains("dpad/down"))
            {
                movement = new Vector2(0, 1);
            }
            if (button.Contains("/Keyboard/d") || button.Contains("dpad/right"))
            {
                movement = new Vector2(1, 0);
            }
            if (button.Contains("/Keyboard/w") || button.Contains("dpad/up"))
            {
                movement = new Vector2(0, -1);
            }

            NumPad(movement);
            if (button.Contains("/Keyboard/k") || button.Contains("buttonEast"))
            {
                code += (numberCount + 1);
                GameObject asterisk = asterisks[codeInputCount];
                asterisk.GetComponent<UnityEngine.UI.Image>().enabled = true;

                // play some input sound
                AudioManager.Instance.InputCodeAudio(gameObject);

                WaitForSeconds Wait = new WaitForSeconds(0.3f);
                yield return Wait;

                codeInputCount++;

                if (codeInputCount > 2)
                {
                    codeInputCount = 0;
                    for (int i = 0; i < 3; i++)
                    {
                        GameObject a = asterisks[i];
                        a.gameObject.GetComponent<UnityEngine.UI.Image>().enabled = false;
                    }

                    if (code.Contains(CODE))
                    {
                        fenceGb.tag = "Untagged";
                        AudioManager.Instance.LockCorrectAudio(gameObject);
                        FenceArt.SetTrigger("Open");
                    }
                    else
                    {
                        AudioManager.Instance.LockWrongAudio(gameObject);
                    }
                    code = "";
                    inputCodeFlag = false;
                    codeInputUI.SetActive(false);
                }
            }
            else
            {
                AudioManager.Instance.LockHoverAudio(gameObject);
            }
        }
    }

    private void NumPad(Vector2 movement)
    {
        numberPos += movement;
        numberPos.x = numberPos.x > 2 ? 2 : numberPos.x;
        numberPos.x = numberPos.x < 0 ? 0 : numberPos.x;
        numberPos.y = numberPos.y > 2 ? 2 : numberPos.y;
        numberPos.y = numberPos.y < 0 ? 0 : numberPos.y;
        numberCount = (int)(numberPos.x + numberPos.y * 3);
        recBox[0].transform.position = recPosInit + new Vector3(moveLength.x * numberPos.x, moveLength.y * numberPos.y);
    }
}