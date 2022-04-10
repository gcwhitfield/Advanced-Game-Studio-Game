using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneController : MonoBehaviour
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

    // when this function is called, the game will show the next panel in the cutscene. If there
    // are no panels remaining, then the game will transition to the next scene
    public void ContinueToNextScreen()
    {
        // show the next panel in sequence
        if (currPanel < panels.Count - 1)
        {
            currPanel++;
            ShowCurrentPanel();
        } else // transition to the next scene
        {
            SceneTransitionManager.Instance.TransitionToScene(nextScene);
        }
    }
}
