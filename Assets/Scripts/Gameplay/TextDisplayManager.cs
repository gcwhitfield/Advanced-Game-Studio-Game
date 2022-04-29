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

    bool isPlayingFatherText = false;
    bool isPlayingDaughterText = false;
    bool closeFatherText = false;
    bool closeDaughterText = false;

    [System.Serializable]
    public enum TextType
    {
        FATHER, // text only on father's side
        DAUGHTER // text only on daughter's side
        // CENTER // text in the center of screen [not yet implemented]
    };

    public delegate void TextCompletedEvent();

    void CloseFatherText()
    {
        closeFatherText = true;
    }

    void CloseDaughterText()
    {
        closeDaughterText = true;
    }

    public void ShowText(string text, TextType type = TextType.DAUGHTER, TMP_FontAsset font = null, int fontSize = 20, TextCompletedEvent completedEvent = null, bool isCutscene = false)
    {
        ScrollingTextParams textParams;
        switch (type)
        {
            case TextType.FATHER:
                if (isPlayingFatherText)
                {
                    CloseFatherText();
                }
                break;
            case TextType.DAUGHTER:
                if (isPlayingDaughterText)
                {
                    CloseDaughterText();
                }
                break;
        }
        if (text.Contains("[FATHER]") || text.Contains("[DAUGHTER]"))
        {
            if (isPlayingFatherText)
            {
                CloseFatherText();
            }
            if (isPlayingDaughterText)
            {
                CloseDaughterText();
            }
        }
        textParams.type = type;
        textParams.text = text;
        textParams.font = font;
        textParams.fontSize = fontSize;
        textParams.textCompletedEvent = completedEvent;
        textParams.isCutscene = isCutscene;
        StartCoroutine("DisplayScrollingText", textParams);
    }

    struct ScrollingTextParams
    {
        public bool isCutscene; // when set to true, the text will advance if either
        // player gives input (used for cutscenes)
        public TextType type; // the location to display the text on the screen
        public string text; // the text to display
        public TMP_FontAsset font;
        public int fontSize;
        public TextCompletedEvent textCompletedEvent; // this function gets executed when the next has
        // finished displaying
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
        // wait for old father text to close before showing this one
        if (textParams.type == TextType.FATHER)
        {
            while (isPlayingFatherText)
            {
                yield return null;
            }
        }
        // wait for old daughter text to close before showing this one
        if (textParams.type == TextType.DAUGHTER)
        {
            while (isPlayingDaughterText)
            {
                yield return null;
            }
        }

        bool isDualDialogue = false;

        if (textParams.text.Contains("[FATHER]") || textParams.text.Contains("[DAUGHTER]"))
        {
            isDualDialogue = true;
        }
        // wait for old dual text to close before showing this one
        if (isDualDialogue)
        {
            while (isPlayingFatherText || isPlayingDaughterText)
            {
                yield return null;
            }
        }

        if (isDualDialogue)
        {
            isPlayingFatherText = true;
            isPlayingDaughterText = true;
        } else
        {
            switch (textParams.type)
            {
                case TextType.DAUGHTER:
                    isPlayingDaughterText = true;
                    break;
                case TextType.FATHER:
                    isPlayingFatherText = true;
                    break;
            }
        }

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

        if (textParams.font != null)
        {
          text.font = textParams.font;
        }
        if (textParams.fontSize != 0){
          text.fontSize = textParams.fontSize;
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
        fatherTextAnimator.Rebind();
        daughterTextAnimator.Rebind();
        animator.gameObject.SetActive(true);
        animator.SetTrigger("Open");

        for (int l = 0; l < lines.Length; l++)
        {
            // display scrolling text
            float timeBetweenChars = 0.03f; // in seconds
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
                // close this current text window if another text window wants to play
                if (closeDaughterText)
                {
                    if (textParams.type == TextType.DAUGHTER || isDualDialogue)
                    {
                        closeDaughterText = false;
                        isPlayingDaughterText = false;
                        if (isDualDialogue) isPlayingFatherText = false;
                        yield break;
                    }
                }
                if (closeFatherText)
                {
                    if (textParams.type == TextType.FATHER || isDualDialogue)
                    {
                        closeFatherText = false;
                        isPlayingFatherText = false;
                        if (isDualDialogue) isPlayingDaughterText = false;
                        yield break;
                    }
                }

                // show scrolling text
                text.text = text.text + lines[l][i];
                yield return new WaitForSeconds(timeBetweenChars);
            }
            yield return null;

            // initialize fatherContinue and daughterContinue
            ResetTextContinueInput(textParams);

            switch (textParams.type)
            {
                case TextType.FATHER:
                    if (FatherController.Instance)
                    {
                        FatherController.Instance.ExecuteUponSubmit(FatherContinueToNextLine);
                    }
                    break;
                case TextType.DAUGHTER:
                    if (DaughterController.Instance)
                    {
                        DaughterController.Instance.ExecuteUponSubmit(DaughterContinueToNextLine);
                    }
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

        // execute the 'text completed' event
        if (textParams.textCompletedEvent != null)
        {
            textParams.textCompletedEvent();
        }

        if (isDualDialogue)
        {
            isPlayingFatherText = false;
            isPlayingDaughterText = false;
        }
        else
        {
            switch (textParams.type)
            {
                case TextType.FATHER:
                    isPlayingFatherText = false;
                    break;
                case TextType.DAUGHTER:
                    isPlayingDaughterText = false;
                    break;
            }
        }
    }
}
