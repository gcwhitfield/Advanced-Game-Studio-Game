using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Input Settings", menuName = "Scrtipable Objects/Input Settings", order = 0)]
public class InputSettings : ScriptableSingleton<InputSettings>
{
    [Header("Player 1 Input")]
    public KeyCode P1Up;
    public KeyCode P1Down;
    public KeyCode P1Left;
    public KeyCode P1Right;

    [Header("Player 2 Input")]
    public KeyCode P2Up;
    public KeyCode P2Down;
    public KeyCode P2Left;
    public KeyCode P2Right;
}
