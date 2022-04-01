using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

// when the player collides with this object, text will display to the screen
public class TextDisplayOnTriggerEnter : Interactable
{
    [System.Serializable]
    public enum ActivationTrigger
    {
        ON_FATHER_ENTER,
        ON_DAUGHTER_ENTER,
        ON_EITHER_ENTER
    };

    [Tooltip("This determines which side of the screen the text gets displayed on")]
    public TextDisplayManager.TextType textDestination;

    [Tooltip("This determines when the text display is triggered")]
    public ActivationTrigger activationTrigger;
    public DialogueEvent textToDisplay;
    public TMP_FontAsset fontDisplay;
    public int fontSize;

    public bool onlyShowTextOnce = true;
    public bool eraseTextIfPlayerLeaves = false;

    bool triggered = false;

    private new void OnTriggerEnter(Collider other)
    {
        if (!triggered || !onlyShowTextOnce)
        {
            if (other.gameObject.tag == "Player")
            {
                PlayerController p = other.gameObject.GetComponent<PlayerController>();
                switch(activationTrigger)
                {
                    case ActivationTrigger.ON_DAUGHTER_ENTER:
                        if (other.gameObject.GetComponent<DaughterController>())
                        {
                            ShowText();
                            ExecuteTriggerEvents();
                        }
                        break;
                    case ActivationTrigger.ON_FATHER_ENTER:
                        if (other.gameObject.GetComponent<FatherController>())
                        {
                            ShowText();
                            ExecuteTriggerEvents();
                        }
                        break;
                    case ActivationTrigger.ON_EITHER_ENTER:
                        ShowText();
                        ExecuteTriggerEvents();
                        break;
                }
            }
        }
    }

    private new void OnTriggerExit(Collider other)
    {
        base.OnTriggerExit(other);
        if (other.tag == "Player")
        {
            if (eraseTextIfPlayerLeaves)
            {
                if (other.GetComponent<DaughterController>() && activationTrigger == ActivationTrigger.ON_DAUGHTER_ENTER)
                {
                    TextDisplayManager.Instance.DaughterContinueToNextLine();
                }
                else if (other.GetComponent<FatherController>() && activationTrigger == ActivationTrigger.ON_FATHER_ENTER)
                {
                    TextDisplayManager.Instance.FatherContinueToNextLine();
                }
            }
        }
    }
    void ShowText()
    {
        triggered = true;
        TextDisplayManager.Instance.ShowText(textToDisplay.dialogueLines, textDestination, fontDisplay, fontSize);
    }
}
