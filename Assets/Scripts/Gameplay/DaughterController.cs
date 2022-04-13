using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.AI;
using UnityEngine.UI;
using static UnityEngine.InputSystem.InputAction;

public class DaughterController : PlayerController
{
    public static DaughterController Instance { get; private set; }

    [HideInInspector]
    public bool hidden;

    private bool isNearHiddenSpot = false;

    public GameObject flashlight;
    public GameObject lightCone;

    public List<GameObject> keyObjects = new List<GameObject>();
    public List<GameObject> keyHints = new List<GameObject>();
    public List<GameObject> chainObjects = new List<GameObject>();
    public List<GameObject> lockObjects = new List<GameObject>();
    public List<Transform> locksPos = new List<Transform>();
    public GameObject keyLockUI;
    public GameObject keyBG;

    [HideInInspector]
    public bool keyLockFlag = false;

    private Vector3 prevLookDirection;
    private Transform keyTransform;
    private Transform lockPos;
    private Vector3 keyMovement;
    private float keySpeed = 10.0f;
    private Vector2 borderLB;
    private Vector2 borderRT;
    private Vector2 borderBias = new Vector2(2f, 5f);
    private float forceStrength = 0.6f;
    private bool keySelected = false;
    private float unlockThreshold = 2f;
    private int keyIndex = 0;
    private int lockIndex = 0;
    private Vector3 keyInitialPos;
    private int inputCounter = 0;

    private new void Start()
    {
        base.Start();
        prevLookDirection = lookDirection;

        keyTransform = keyObjects[0].transform;
        keyInitialPos = keyTransform.position;
        keyHints[0].SetActive(true);
        lockPos = locksPos[0];
        GetEdges();
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

        if (other.gameObject.CompareTag("KeyLock"))
        {
            keyLockFlag = true;
            keyLockUI.SetActive(true);
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

        Quaternion desiredConeRotation = Quaternion.Euler(new Vector3(0, Mathf.Rad2Deg * Mathf.Atan2(lookDirection.x, lookDirection.z) - 70, 0));
        Quaternion currConeRotation = lightCone.transform.rotation;
        lightCone.transform.rotation = Quaternion.Lerp(currConeRotation, desiredConeRotation, 0.5f);

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

        if (keyMovement != Vector3.zero && keySelected)
        {
            Transform t = keyTransform;
            t.Translate(keyMovement * keySpeed * Time.deltaTime);
            t.position = new Vector3(Mathf.Max(Mathf.Min(t.position.x, borderRT.x), borderLB.x), Mathf.Max(Mathf.Min(t.position.y, borderRT.y), borderLB.y), t.position.z);

            CheckKeyLock();
        }
    }

    public void KeyLock(CallbackContext context)
    {
        string button = context.control.ToString();
        if (keyLockFlag)
        {
            if (button.Contains("/Keyboard/k") || button.Contains("buttonEast"))
            {
                if (context.ReadValue<float>() > 0)
                {
                    if (keyObjects.Count > 0)
                    {
                        keySelected = true;
                        keyTransform = keyObjects[keyIndex].transform;
                        keyInitialPos = keyTransform.position;
                        keyHints[keyIndex].SetActive(false);
                        AudioManager.Instance.KeyPickAudio(gameObject);
                    }
                }
            }
            else
            {
                Vector2 keyMovement2D = context.ReadValue<Vector2>();
                if (keySelected)
                {
                    keyMovement = new Vector3(keyMovement2D.x, keyMovement2D.y, 0);
                    if (keyMovement2D.magnitude > 0)
                        DisturbKeyMovement();
                }
                else
                {
                    if (context.phase.ToString().Contains("Performed"))
                    {
                        if (keyMovement2D.x > 0)
                        {
                            moveKeyHint(1);
                        }
                        else if (keyMovement2D.x < 0)
                        {
                            moveKeyHint(-1);
                        }
                    }
                }
            }
        }
    }

    private void GetEdges()
    {
        Transform t = keyBG.transform;
        RectTransform rt = keyBG.GetComponent<RectTransform>();
        borderRT.x = t.position.x + rt.rect.width * t.lossyScale.x / 2f - borderBias.x;
        borderRT.y = t.position.y + rt.rect.height * t.lossyScale.y / 2f - borderBias.y;
        borderLB.x = t.position.x - rt.rect.width * t.lossyScale.x / 2f + borderBias.x;
        borderLB.y = t.position.y - rt.rect.height * t.lossyScale.y / 2f + borderBias.y;
    }

    private void DisturbKeyMovement()
    {
        Vector3 force = new Vector3(Random.Range(-1, 1), Random.Range(-1, 1), 0) * forceStrength;
        keyMovement += force;
    }

    private void moveKeyHint(int i)
    {
        keyHints[keyIndex].SetActive(false);
        int index = keyIndex + i;
        if (index < 0)
        {
            index = 0;
        }
        else if (index > keyHints.Count - 1)
        {
            index = keyHints.Count - 1;
        }
        keyIndex = index;

        keyHints[keyIndex].SetActive(true);

        AudioManager.Instance.KeyRotateAudio(gameObject);
    }

    private void CheckKeyLock()
    {
        if (Vector3.Distance(lockPos.position, keyTransform.position) < unlockThreshold)
        {
            keySelected = false;
            keyMovement = new Vector3(0, 0, 0);
            inputCounter = 0;
            //if key match lock
            if (lockObjects[lockIndex].tag == keyObjects[keyIndex].tag)
            {
                //play audio
                AudioManager.Instance.KeyUnlockAudio(gameObject);
                //fade out animation
                StartCoroutine(FadeOutLockChain(lockObjects[lockIndex], keyObjects[keyIndex], chainObjects[lockIndex]));

                keyObjects.RemoveAt(keyIndex);
                keyHints.RemoveAt(keyIndex);
                keyIndex = 0;
                lockIndex++;
                if (keyObjects.Count > 0)
                {
                    keyHints[keyIndex].SetActive(true);
                    lockPos = locksPos[lockIndex];
                }
            }

            //else Reset key
            else
            {
                //play wrong audio
                AudioManager.Instance.KeyWrongAudio(gameObject);

                keyTransform.position = keyInitialPos;
                keyHints[keyIndex].SetActive(true);
            }
        }
        if (keyObjects.Count == 0)
        {
            AudioManager.Instance.KeyCorrectAudio(gameObject);
            StartCoroutine(FadeOutUI());
        }
    }

    private IEnumerator FadeOutLockChain(GameObject locker, GameObject key, GameObject chain)
    {
        float fadeSpeed = 3f;
        float waitTime = 1f / 30f;
        float ticks = 1;
        float ticksChain = 1;
        WaitForSeconds wait = new WaitForSeconds(waitTime);
        // fade lock and key

        while (chain.GetComponent<Image>().color.a > 0)
        {
            Color cL = locker.GetComponent<Image>().color;
            Color cK = key.GetComponent<Image>().color;
            key.GetComponent<Image>().color = new Color(cL.r, cL.g, cL.b, Mathf.Lerp(cL.a, 0, waitTime * ticks * fadeSpeed));
            locker.GetComponent<Image>().color = new Color(cK.r, cK.g, cK.b, Mathf.Lerp(cK.a, 0, waitTime * ticks * fadeSpeed));
            ticks++;

            //fade chain after lock and key
            if (locker.GetComponent<Image>().color.a <= 0 && key.GetComponent<Image>().color.a <= 0)
            {
                Color cC = chain.GetComponent<Image>().color;
                chain.GetComponent<Image>().color = new Color(cC.r, cC.g, cC.b, Mathf.Lerp(cC.a, 0, waitTime * ticksChain * fadeSpeed));
                ticksChain++;
            }
            yield return wait;
        }
    }

    private IEnumerator FadeOutUI()
    {
        WaitForSeconds wait = new WaitForSeconds(0.9f);
        yield return wait;

        //play unlock all chain audio

        keyLockFlag = false;
        keyLockUI.SetActive(false);
    }
}