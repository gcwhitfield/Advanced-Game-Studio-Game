using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Dialogue Item", menuName = "Dialogue/Dialogue Item")]
public class DialogueEvent : ScriptableObject
{
    [Tooltip("The order in which the text entries are defined in dialogueLines will determine " +
        "the order in which they appear in the game. New elements of dialogueLines will always " +
        "show up as a new line in the game, NOT a continuation of the previous line.")]
    [TextArea(1, 50)]
    public string dialogueLines;
}
