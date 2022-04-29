using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// The Wolf encounter occurs in Level 2
// 1) the father collects the bone
// 2) the daughter encounters the wolf and goes to hide
// 3) the father needs to throw the bone to distract the wolf
public class WolfEncounter : Singleton<WolfEncounter>
{
    [HideInInspector]
    public bool fatherHasCollectedBone = false;
    private bool daugherHasHidden = false;
    private bool fatherHasThrownBone = false;
    [HideInInspector]
    public bool daughterHasReachedInvisibleTrigger = false;
    private bool fatherPressTheButton = false;

    public List<Interactable> daughterHideSpots;
    public List<GameObject> hideRings;
    public Collectable fatherBone;
    public Interactable daughterWolfInvisibleTrigger; // this is an invisible trigger that the daughter must
    // reach before hiding

    public GameObject wolf;

    // called when the father collects bone
    public void OnFatherCollectBone()
    {
        fatherHasCollectedBone = true;
        Debug.Log("Father has collected bone");

        // play audio
        AudioManager.Instance.PickUpAudio(FatherController.Instance.gameObject);
    }

    // called when the daughter hides
    public void OnDaughterHide()
    {
        Debug.Log("Daughter has hidden");
        daugherHasHidden = true;
    }

    public void OnFatherThrowBone()
    {
        if (DaughterController.Instance.hidden)
        {
            Debug.Log("Father has thrown bone");
            fatherHasThrownBone = true;
            TextDisplayManager.Instance.FatherContinueToNextLine();
            // TODO: Add bone throwing animation

            // deactivate the hide rings
            foreach (GameObject g in hideRings)
            {
                g.SetActive(false);
            }

            // play audio
            AudioManager.Instance.BoneAudio(FatherController.Instance.gameObject);
        }
        else
        {
            Debug.Log("empty: not throw bone");
            fatherPressTheButton = true;
        }
    }

    public void OnDaughterInvisibleTriggerReached()
    {
        Debug.Log("Daughter has reached invisible trigger");
        daughterHasReachedInvisibleTrigger = true;

        // activate all of the hide rings
        foreach (GameObject g in hideRings)
        {
            g.SetActive(true);
        }

        // play audio
        AudioManager.Instance.WolfShowUpAudio(DaughterController.Instance.gameObject);
    }

    private void Start()
    {
        if (daughterHideSpots.Count < 1 || !fatherBone || !wolf || !daughterWolfInvisibleTrigger)
        {
            Debug.LogError("All public variables must be set in the Inspector before executing the wolf encounter!");
        }
        else
        {
            StartCoroutine("DoWolfEncounter");
        }
    }

    private IEnumerator TellFatherToThrowBone()
    {
        float waitTime = 5.0f; // seconds
        yield return new WaitForSeconds(waitTime);
        if (!fatherHasThrownBone)
        {
            TextDisplayManager.Instance.ShowText("If only there was some way to make the wolf disappear...", TextDisplayManager.TextType.FATHER);
        }
    }

    private IEnumerator DoWolfEncounter()
    {
        // step 1) the father must collect the bone and the daughter must hide
        Debug.Log("Begin step 1");
        fatherBone.ExecuteOnInteract(OnFatherCollectBone);
        daughterWolfInvisibleTrigger.ExecuteOnTriggerEnter(OnDaughterInvisibleTriggerReached);
        foreach (Interactable i in daughterHideSpots)
        {
            i.ExecuteOnInteract(OnDaughterHide);
        }
        while (!fatherHasCollectedBone || !DaughterController.Instance.hidden || !daughterHasReachedInvisibleTrigger)
        {
            yield return new WaitForSeconds(0.2f);
        }

        Debug.Log("Begin step 2");
        // step 2) display the "throw bone" text to father. Wait for the father
        // to throw the bone
        StartCoroutine("TellFatherToThrowBone");
        //FatherController.Instance.ExecuteUponSubmit(OnFatherThrowBone);
        while (!fatherHasThrownBone)
        {
            if (fatherPressTheButton)
            {
                fatherPressTheButton = false;
                FatherController.Instance.ExecuteUponSubmit(OnFatherThrowBone);
            }
            yield return new WaitForSeconds(0.2f);
        }

        TextDisplayManager.Instance.ShowText("The wolf leaves", TextDisplayManager.TextType.DAUGHTER);
        DaughterController.Instance.ExecuteUponSubmit(TextDisplayManager.Instance.DaughterContinueToNextLine);
        // step 3) the wolf runs away from the daughter. The wolf goes to the bone
        wolf.gameObject.SetActive(false);
        daughterWolfInvisibleTrigger.gameObject.SetActive(false);
        AudioManager.Instance.WolfChaseAudioStop();
    }
}