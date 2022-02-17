using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DaughterController : PlayerController
{
    public static DaughterController Instance { get; private set; }

    [SerializeField]
    private GameObject daughterInputHandlerPrefab;

    private void Awake()
    {
        if (!Instance) Instance = this as DaughterController;
    }

    private new void Start()
    {
        base.Start();
        Debug.Log("Daughter start called!");
        
        //PlayerInputManagerReference.Instance.playerInputManager.playerPrefab = daughterInputHandlerPrefab;
        //playerIndex = 1;
        //PlayerInputManagerReference.Instance.playerInputManager.JoinPlayer(playerIndex);
    }
}
