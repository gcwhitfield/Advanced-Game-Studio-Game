using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Level2Controller : Singleton<Level2Controller>
{
    public DialogueEvent endingDialogue;
    bool endDialogueActivated = false;

    public string nextScene;

    public PlayerInputManager playerInputManager;

    private void Awake()
    {
        // automatically join players when scene begins
        bool fatherJoined = false;
        foreach (InputDevice device in InputSystem.devices)
        {
            if (device.layout.Contains("Gamepad"))
            {
                if (!fatherJoined)
                {
                    playerInputManager.JoinPlayer(FatherController.playerIndex, 0, null, device);
                    fatherJoined = true;
                }
                else
                {
                    playerInputManager.JoinPlayer(DaughterController.playerIndex, 0, null, device);
                    break;
                }
            }
        }
    }

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
