using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

[RequireComponent(typeof(PlayerInput))]
public class PlayerInputHandler : MonoBehaviour
{
    private PlayerInput playerInput;

    [HideInInspector]
    public PlayerController player;

    public bool debugMode = true;
    public bool fatherOn = false;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        Debug.Log(DaughterController.Instance.playerIndex);
        Debug.Log(FatherController.Instance.playerIndex);
        if (debugMode)
        {
            if (fatherOn)
            {
                player = FatherController.Instance;
            }
            else {
                player = DaughterController.Instance;
            }
        }
        else {
            if (playerInput.playerIndex == DaughterController.Instance.playerIndex)
            {
                player = DaughterController.Instance;
                //player = FatherController.Instance;
            }
            else
            {
                player = FatherController.Instance;
            }
        }
    }

    public void OnMove(CallbackContext context)
    {
        if (player)
        {
            player.OnMove(context);
        }
        else
        {
            Debug.LogError("Instance of player not set in PlayerInputHandler");
        }
    }

    public void OnShoot(CallbackContext context)
    {
        // if player is "father"
        if (player == FatherController.Instance)
        {
            if (context.ReadValueAsButton())
            {
                FatherController f = player as FatherController;
                f.Shoot();
            }
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

    public void OnCollect(CallbackContext context)
    {
        // if player is "daughter"
        if (player == DaughterController.Instance)
        {
            DaughterController d = player as DaughterController;
            d.Collect();
        }
    }
}