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
        DAUGHTER // text only on daughter's side
        // CENTER // text in the center of screen [not yet implemented]
    };

    public void ShowText(string text, TextType type = TextType.DAUGHTER)
    {
        ScrollingTextParams textParams;
        textParams.type = type;
        textParams.text = text;
        StartCoroutine("DisplayScrollingText", textParams);
    }

    struct ScrollingTextParams
    {
        public TextType type; // the location to display the text on the screen
        public string text; // the text to display
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

    private void ResetTextContinueInput(ScrollingTextParams textParams)
    {
        switch (textParams.type)
        {
            case TextType.FATHER:
                fatherContinue = false;
                break;
            case TextType.DAUGHTER:
                daughterContinue = false;
                break;
        }
    }

    IEnumerator DisplayScrollingText(ScrollingTextParams textParams)
    {
        bool isDualDialogue = false;
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

            // switch between father and daugher text if switch code given
            // [DAUGHTER] - switches to daughter lines
            // [FATHER] - switches to father lines
            if (lines[l] == "[DAUGHTER]")
            {
                isDualDialogue = true;
                textParams.type = TextType.DAUGHTER;
                animator = daughterTextAnimator;
                fatherText.text = "...";
                text = daughterText;
                continue;
            }
            if (lines[l] == "[FATHER]")
            {
                isDualDialogue = true;
                textParams.type = TextType.FATHER;
                text = fatherText;
                daughterText.text = "...";
                animator = fatherTextAnimator;
                continue;
            }
            if (!animator.gameObject.activeSelf)
            {
                animator.gameObject.SetActive(true);
                animator.SetTrigger("Open");
            }

            for (int i = 0; i < lines[l].Length; i++)
            {
                text.text = text.text + lines[l][i];
                yield return new WaitForSeconds(timeBetweenChars);
            }
            yield return null;

            // initialize fatherContinue and daughterContinue
            ResetTextContinueInput(textParams);

            switch (textParams.type)
            {
                case TextType.FATHER:
                    FatherController.Instance.ExecuteUponSubmit(FatherContinueToNextLine);
                    break;
                case TextType.DAUGHTER:
                    DaughterController.Instance.ExecuteUponSubmit(DaughterContinueToNextLine);
                    break;
            }

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

            ResetTextContinueInput(textParams);
        }

        // play close animation, disable the textbox
        if (isDualDialogue)
        {
            fatherTextAnimator.SetTrigger("Close");
            daughterTextAnimator.SetTrigger("Close");
        } else
        {
            animator.SetTrigger("Close");
        }
        float timer = 0;
        float waitTime = 2; // time to wait before removing the text
        while (timer < waitTime) {
            timer += Time.deltaTime;
            yield return null;
        }
        if (isDualDialogue)
        {
            fatherTextAnimator.gameObject.SetActive(false);
            fatherTextAnimator.Rebind();
            daughterTextAnimator.gameObject.SetActive(false);
            daughterTextAnimator.Rebind();
        } else
        {
            animator.Rebind();
            animator.gameObject.SetActive(false);
        }
    }
}
