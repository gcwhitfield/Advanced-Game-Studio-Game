using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DaughterController : PlayerController
{
    public static DaughterController Instance { get; private set; }

    private void Awake()
    {
        if (!Instance) Instance = this as DaughterController;
    }
}
