using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneController : Singleton<CutsceneController>
{
    public string nextScene; // the scene to transition to once the custscene has ended

    int currPanel = 0;

    [System.Serializable]
    public struct panelInfo
    {
        public GameObject panelGameObject;
        public DialogueEvent dialogue;
    };

    public delegate void CutsceneDialogueCompletedEvent();
    CutsceneDialogueCompletedEvent events = null;

    public List<panelInfo> panels;

    public void ExecuteOnDialogueCompleted(CutsceneDialogueCompletedEvent e)
    {
        events += e;
    }

    public void CancelExecuteOnDialogueCompleted(CutsceneDialogueCompletedEvent e)
    {
        events -= e;
    }

    void ShowCurrentPanel()
    {
        foreach (panelInfo p in panels)
        {
            p.panelGameObject.SetActive(false);
        }
        panels[currPanel].panelGameObject.SetActive(true);
    }

    private void Start()
    {
        ShowCurrentPanel();
        TextDisplayManager.Instance.ShowText(panels[currPanel].dialogue.dialogueLines, completedEvent: ContinueToNextScreen);
    }

    // this function is called whe the B button is pressed
    public void OnBButtonHit()
    {
        if (panels[currPanel].dialogue != null)
        {
            TextDisplayManager.Instance.DaughterContinueToNextLine();
            TextDisplayManager.instance.FatherContinueToNextLine();
        } else
        {
            ContinueToNextScreen();
        }
    }

    // shows the fading animation between different cutscene panels
    IEnumerator AnimateIntoNextPanel()
    {
        // there is another panel left in the list of panels. fade the screen to black,
        // change the panel, and then fade the screen back in

        // closing animation
        SceneTransitionManager.Instance.sceneTransitionAnimator.SetTrigger("Close");
        yield return new WaitForSeconds(0.05f);
        float t = SceneTransitionManager.Instance.sceneTransitionAnimator.GetCurrentAnimatorClipInfo(0).Length;
        float extraWaitTime = 1.0f; // wait a little extra to ensure that the animation has finished playing
        t += extraWaitTime;
        while (t > 0)
        {
            t -= Time.deltaTime;
            yield return null;
        }

        ShowCurrentPanel();

        // opening animation
        SceneTransitionManager.Instance.sceneTransitionAnimator.SetTrigger("Open");
        yield return new WaitForSeconds(0.05f);
        t = SceneTransitionManager.Instance.sceneTransitionAnimator.GetCurrentAnimatorClipInfo(0).Length;
        t += extraWaitTime;
        while (t > 0)
        {
            t -= Time.deltaTime;
            yield return null;
        }
        TextDisplayManager.Instance.ShowText(panels[currPanel].dialogue.dialogueLines, completedEvent: ContinueToNextScreen);
    }

    // when this function is called, the game will show the next panel in the cutscene. If there
    // are no panels remaining, then the game will transition to the next scene
    public void ContinueToNextScreen()
    {
        // show the next panel in sequence
        if (currPanel < panels.Count - 1)
        {
            currPanel++;
            StartCoroutine("AnimateIntoNextPanel");
        } else // transition to the next scene
        {
            SceneTransitionManager.Instance.TransitionToScene(nextScene);
        }
    }
}
