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

    [System.Serializable]
    public enum TextType
    {
        FATHER, // text only on father's side
        DAUGHTER, // text only on daughter's side
        CENTER // text in the center of screen [not yet implemented]
    };

    public void ShowTestText()
    {
        ShowText(TextType.FATHER, "Bagels are my favorite food! :^)");
    }

    public void ShowText(TextType type, string text)
    {
        ScrollingTextParams textParams;
        textParams.type = type;
        textParams.text = text;
        StartCoroutine("DisplayScrollingText", textParams);
    }

    struct ScrollingTextParams
    {
        public TextType type;
        public string text;
    }

    IEnumerator DisplayScrollingText(ScrollingTextParams textParams)
    {
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

        // play the open animation    
        animator.SetTrigger("Open");
        
        // display scrolling text
        float timeBetweenChars = 0.05f; // in seconds
        int i = 0;
        text.text = "";
        while (i < textParams.text.Length)
        {
            text.text = text.text + textParams.text[i];
            yield return new WaitForSeconds(timeBetweenChars);
        }
        yield return null;

        // play close animation, disable the textbox
        animator.SetTrigger("Close");
        float timer = 0;
        while (timer < animator.GetCurrentAnimatorClipInfo(0)[0].clip.length + 0.1f) // wait additional 0.1 sec after finish
        {
            timer += Time.deltaTime;
            yield return null;
        }
        animator.gameObject.SetActive(false);
    }
}
