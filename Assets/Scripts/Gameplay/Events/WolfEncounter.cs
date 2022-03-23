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

    public Interactable daughterHideSpot;
    public Collectable fatherBone;

    MonsterController wolfController;

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
        // TODO: Add bone throwing to father
    }

    private void Start()
    {
        if (!daughterHideSpot || !fatherBone || !wolfController)
        {
            Debug.LogError("All public variables must be set in the Inspector before executing the wolf encounter!");
        } else
        {
            StartCoroutine("WolfEncounter");
        }
    }

    IEnumerator DoWolfEncounter()
    {
        // step 1) the father must collect the bone and the daughter must hide
        Debug.Log("Begin step 1");
        fatherBone.ExecuteOnInteract(OnFatherCollectBone);
        daughterHideSpot.ExecuteOnInteract(OnDaughterHide);
        while (!fatherHasCollectedBone && !daugherHasHidden)
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

        // step 3) the wolf runs away from the daughter. The wolf goes to the bone
        wolfController.gameObject.SetActive(false);
    }
}
