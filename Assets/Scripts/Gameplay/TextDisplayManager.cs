using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextDisplayManager : Singleton<TextDisplayManager>
{
    [SerializeField] private Animator fatherTextAnimator;
    [SerializeField] private TMP_Text fatherText;
    [SerializeField] private Animator daughterTextAnimator;
    [SerializeField] private TMP_Text daughterText;

    bool daughterContinue = false;
    bool fatherContinue = false;

    [System.Serializable]
    public enum TextType
    {
        FATHER, // text only on father's side
        DAUGHTER, // text only on daughter's side
        CENTER // text in the center of screen [not yet implemented]
    };

    public void ShowTestText()
    {
        ShowText(TextType.FATHER, "Bagels are my favorite food! :^)", 5.0f);
    }

    public void ShowText(TextType type, string text, float textHoldTime)
    {
        ScrollingTextParams textParams;
        textParams.type = type;
        textParams.text = text;
        textParams.textHoldTime = textHoldTime;
        StartCoroutine("DisplayScrollingText", textParams);
    }

    struct ScrollingTextParams
    {
        public TextType type; // the location to display the text on the screen
        public string text; // the text to display
        public float textHoldTime; // the duration to hold the text on the screen
    }

    // this function will be called from DaughterController.cs. when the player presses their
    // controller to advance to the next dialogue option
    public void DaughterContinueToNextLine()
    {
        daughterContinue = true;
    }

    // this function will be called from FatherController.cs. when the player presses their
    // controller to advance to the next dialogue option
    public void FatherContinueToNextLine()
    {
        fatherContinue = true;
    }

    IEnumerator DisplayScrollingText(ScrollingTextParams textParams)
    {
        Debug.Log("Display scrolling text called");

        Animator animator = null;
        TMP_Text text = null;
        switch(textParams.type)
        {
            case TextType.FATHER:
                animator = fatherTextAnimator;
                text = fatherText;
                break;
            case TextType.DAUGHTER:
                animator = daughterTextAnimator;
                text = daughterText;
                break;
            case TextType.CENTER:
                // not implemented
                yield break;
        }

        if (!animator) // log warning if animator not found
        {
            Debug.LogWarning("Animator for scrolling text not found. Not playing text animation...");
            yield break;
        }
        if (!text) // log error if text not found
        {
            Debug.LogError("'TMP_Text' object for text scrolling not found. Text will not be displayed");
            yield break;
        }

        // create a list of lines based on the return character from input string
        char []separator = { '\n' };
        string []lines = textParams.text.Split(separator);
        
        // play the open animation    
        animator.gameObject.SetActive(true);
        animator.SetTrigger("Open");

        for (int l = 0; l < lines.Length; l++)
        {
            // display scrolling text
            float timeBetweenChars = 0.05f; // in seconds
            text.text = "";
            for (int i = 0; i < lines[l].Length; i++)
            {
                text.text = text.text + lines[l][i];
                yield return new WaitForSeconds(timeBetweenChars);
            }
            yield return null;

            // wait for the user to continue
            // 'continueToNextLine' will be set to true when the user presses their controller
            bool cont = false;
            while (!cont) 
            {
                switch(textParams.type)
                {
                    case TextType.FATHER:
                        cont = fatherContinue;
                        break;
                    case TextType.DAUGHTER:
                        cont = daughterContinue;
                        break;
                    default:
                        cont = daughterContinue;
                        break;
                }
                yield return null;
            }
        }

        // play close animation, disable the textbox
        animator.SetTrigger("Close");

        float timer = 0;
        float waitTime = 2; // time to wait before removing the text
        while (timer < waitTime) {
            timer += Time.deltaTime;
            yield return null;
        }
        animator.Rebind();
        animator.gameObject.SetActive(false);
    }
}
