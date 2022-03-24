using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// The Wolf encounter occurs in Level 2
// 1) the father collects the bone
// 2) the daughter encounters the wolf and goes to hide
// 3) the father needs to throw the bone to distract the wolf
public class WolfEncounter : MonoBehaviour
{
    bool fatherHasCollectedBone = false;
    bool daugherHasHidden = false;
    bool fatherHasThrownBone = false;
    bool daughterHasReachedInvisibleTrigger = false;

    public List<Interactable> daughterHideSpots;
    public Collectable fatherBone;
    public Interactable daughterWolfInvisibleTrigger; // this is an invisible trigger that the daughter must
    // reach before hiding 

    public GameObject wolf;

    // called when the father collects bone
    public void OnFatherCollectBone()
    {
        fatherHasCollectedBone = true;
        Debug.Log("Father has collected bone");
    }

    // called when the daughter hides
    public void OnDaughterHide()
    {
        Debug.Log("Daughter has hidden");
        daugherHasHidden = true;
    }

    public void OnFatherThrowBone()
    {
        Debug.Log("Father has thrown bone");
        fatherHasThrownBone = true;
        TextDisplayManager.Instance.FatherContinueToNextLine();
        // TODO: Add bone throwing to father
    }

    public void OnDaughterInvisibleTriggerReached()
    {
        Debug.Log("Daughter has reached invisible trigger");
        daughterHasReachedInvisibleTrigger = true;
    }

    private void Start()
    {
        if (daughterHideSpots.Count < 1 || !fatherBone || !wolf || !daughterWolfInvisibleTrigger)
        {
            Debug.LogError("All public variables must be set in the Inspector before executing the wolf encounter!");
        } else
        {
            StartCoroutine("DoWolfEncounter");
        }
    }

    IEnumerator DoWolfEncounter()
    {
        // step 1) the father must collect the bone and the daughter must hide
        Debug.Log("Begin step 1");
        fatherBone.ExecuteOnInteract(OnFatherCollectBone);
        daughterWolfInvisibleTrigger.ExecuteOnTriggerEnter(OnDaughterInvisibleTriggerReached);
        foreach (Interactable i in daughterHideSpots)
        {
            i.ExecuteOnInteract(OnDaughterHide);
        }
        while (!fatherHasCollectedBone || !daugherHasHidden || !daughterHasReachedInvisibleTrigger)
        {
            yield return new WaitForSeconds(0.2f);
        }

        Debug.Log("Begin step 2");
        // step 2) display the "throw bone" text to father. Wait for the father
        // to throw the bone
        TextDisplayManager.Instance.ShowText(TextDisplayManager.TextType.FATHER, "Press Q to throw bone");
        FatherController.Instance.ExecuteUponSubmit(OnFatherThrowBone);
        while (!fatherHasThrownBone)
        {
            yield return new WaitForSeconds(0.2f);
        }

        TextDisplayManager.Instance.ShowText(TextDisplayManager.TextType.DAUGHTER, "The wolf leaves");
        DaughterController.Instance.ExecuteUponSubmit(TextDisplayManager.Instance.DaughterContinueToNextLine);
        // step 3) the wolf runs away from the daughter. The wolf goes to the bone
        wolf.gameObject.SetActive(false);
    }
}
