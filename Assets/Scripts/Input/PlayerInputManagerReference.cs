using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInputManager))]
public class PlayerInputManagerReference : Singleton<PlayerInputManagerReference>
{
    public PlayerInputManager playerInputManager { get; private set; }

    public void Awake()
    {
        playerInputManager = GetComponent<PlayerInputManager>();
    }

}
