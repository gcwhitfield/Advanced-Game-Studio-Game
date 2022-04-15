using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

[RequireComponent(typeof(PlayerInput))]
public class CustscenePlayerInputHandler : MonoBehaviour
{
    // this function is called whenever the player hits the B button on the gamepad
    // or the E key on the keyboard
    public void OnBButtonHit(CallbackContext context)
    {
        if (context.started)
        {
            CutsceneController.Instance.ContinueToNextScreen();
        }
    }
}
