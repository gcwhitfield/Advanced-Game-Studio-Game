using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneController : Singleton<CutsceneController>
{
    public string nextScene; // the scene to transition to once the custscene has ended

    int currPanel = 0;
    public List<GameObject> panels;

    void ShowCurrentPanel()
    {
        foreach (GameObject g in panels)
        {
            g.SetActive(false);
        }
        panels[currPanel].SetActive(true);
    }

    private void Start()
    {
        ShowCurrentPanel(); 
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
        while (t > 0)
        {
            t -= Time.deltaTime;
            yield return null;
        }
        
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
