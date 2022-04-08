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
        if (debugMode)
        {
            if (fatherOn)
            {
                player = FatherController.Instance;
            }
            else
            {
                player = DaughterController.Instance;
            }
        }
        else
        {
            if (playerInput.playerIndex == DaughterController.Instance.playerIndex)
            {
                player = DaughterController.Instance;
            }
            else
            {
                player = FatherController.Instance;
            }
        }
    }

    public void OnMove(CallbackContext context)
    {
        if (player == FatherController.Instance)
        {
            if (!FatherController.Instance.inputCodeFlag)
            {
                player.Move(context);
            }
            else
            {
                player.Stop();
            }
        }
        else if (player == DaughterController.Instance)
        {
            player.Move(context);
            if (!DaughterController.Instance.keyLockFlag)
            {
                player.Move(context);
            }
            else
            {
                player.Stop();
            }
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
        if (player == DaughterController.Instance)
        {
            DaughterController d = player as DaughterController;
            d.Submit();
        }
        else
        {
            FatherController f = player as FatherController;
            f.Submit();
        }
    }

    public void OnSubmit(CallbackContext context)
    {
        if (player == DaughterController.Instance)
        {
            DaughterController d = player as DaughterController;
            d.Submit();
        }
        else
        {
            FatherController f = player as FatherController;
            f.Submit();
        }
    }

    public void OnInputCode(CallbackContext context)
    {
        if (player == FatherController.Instance)
        {
            FatherController f = player as FatherController;
            StartCoroutine(f.InputCode(context));
        }
    }

    public void OnToggleInventory(CallbackContext context)
    {
        player.ToggleInventory();
    }

    public void OnNavigateInventory(CallbackContext context)
    {
        player.NagivateInventory(context);
    }

    public void OnKeyLock(CallbackContext context)
    {
        if (player == DaughterController.Instance)
        {
            DaughterController f = player as DaughterController;
            f.KeyLock(context);
        }
    }
}