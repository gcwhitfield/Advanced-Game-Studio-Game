using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// when the player collides with this object, text will display to the screen
public class TextDisplayOnTriggerEnter : MonoBehaviour
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

    bool triggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!triggered)
        {
            if (other.gameObject.tag == "Player")
            {
                switch(activationTrigger)
                {
                    case ActivationTrigger.ON_DAUGHTER_ENTER:
                        if (other.gameObject.GetComponent<DaughterController>())
                        {
                            ShowText();
                        }
                        break;
                    case ActivationTrigger.ON_FATHER_ENTER:
                        if (other.gameObject.GetComponent<FatherController>())
                        {
                            ShowText();
                        }
                        break;
                    case ActivationTrigger.ON_EITHER_ENTER:
                        ShowText();
                        break;
                }
            }

        }
    }

    void ShowText()
    {
        triggered = true;
        TextDisplayManager.Instance.ShowText(textDestination, textToDisplay.dialogueLines);
    }
}
