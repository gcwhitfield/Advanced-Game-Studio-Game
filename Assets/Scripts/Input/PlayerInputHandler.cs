using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

[RequireComponent(typeof(PlayerInput))]
public class PlayerInputHandler : MonoBehaviour
{
    PlayerInput playerInput;
    [HideInInspector]
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

    public void OnShoot(CallbackContext context)
    {
        // if player is "father"
        if (player == FatherController.Instance)
        {
            FatherController f = player as FatherController;
            f.Shoot();
        }

    }

    public void OnHide(CallbackContext context)
    {
        // if player is "daughter"
        if (player == DaughterController.Instance)
        {
            DaughterController d = player as DaughterController;
            d.Hide();
        }
    }
}
