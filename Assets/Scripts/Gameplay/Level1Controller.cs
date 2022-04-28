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

    // this is the font that is used to display the level instructions dialogue at the
    // beginning of level 1
    public TMPro.TMP_FontAsset daughterIntructionsFont;
    public TMPro.TMP_FontAsset fatherIntructionsFont;

    private void Start()
    {
        // activate the daughter's hide ring when the daughter reaches the text display
        // checkpoint
        daughterHideDisplayCheckpoint.ExecuteOnTriggerEnter(ShowHideRing);

        //  show the level intructions text to the players at the start of the level
        
        TextDisplayManager.Instance.ShowText("Collect pinecones and explore the area.", TextDisplayManager.TextType.DAUGHTER, daughterIntructionsFont);
        TextDisplayManager.Instance.ShowText("Find a way to open the fence gate.", TextDisplayManager.TextType.FATHER, fatherIntructionsFont);
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
