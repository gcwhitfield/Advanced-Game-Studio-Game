using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level2Controller : Singleton<Level2Controller>
{
    public DialogueEvent endingDialogue;
    bool endDialogueActivated = false;

    public string nextScene;

    void FixedUpdate()
    {
        // when the daughter and father get close to each other, activate the
        // ending dialogue cutscne
        if (!endDialogueActivated)
        {
            float dist = 5;
            if (Vector3.Distance(DaughterController.Instance.gameObject.transform.position,
                                   FatherController.Instance.gameObject.transform.position) < dist)
            {
                Debug.Log("End dialogue activated");
                endDialogueActivated = true;
                //TextDisplayManager.Instance.ShowText(endingDialogue.dialogueLines);
                SceneTransitionManager.Instance.TransitionToScene(nextScene);
            }
            //SceneTransitionManager.Instance.TransitionToScene(nextScene);
        }
    }
}
