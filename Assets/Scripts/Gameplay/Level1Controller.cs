using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level1Controller : Singleton<Level1Controller>
{
    [Header("Scene transition")]
    public string nextScene;

    [Header("Daughter")]
    public TextDisplayOnTriggerEnter daughterHideDisplayCheckpoint;
    public GameObject daughterHideRing;

    private void Start()
    {
        // activate the daughter's hide ring when the daughter reaches the text display
        // checkpoint
        daughterHideDisplayCheckpoint.ExecuteOnTriggerEnter(ShowHideRing);
    }

    public void ShowHideRing()
    {
        daughterHideRing.SetActive(true);
    }

    // this function is called when the level is completed
    public void OnLevelCompleted()
    {
        SceneTransitionManager.Instance.TransitionToScene(nextScene);
    }

}
