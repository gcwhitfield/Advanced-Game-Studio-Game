using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

[RequireComponent(typeof(PlayerInput))]
public class PlayerInputHandler : MonoBehaviour
{
    PlayerInput playerInput;
    public PlayerController player;

    private void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        if (playerInput.playerIndex == DaughterController.Instance.playerIndex)
        {
            player = DaughterController.Instance;
        } else
        {
            player = FatherController.Instance;
        }
    }

    public void OnMove(CallbackContext context)
    {
        player.OnMove(context);
    }
}
