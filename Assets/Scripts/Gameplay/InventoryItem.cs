using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryItem : Object
{
    public string itemName;
    public Sprite icon;
    public PlayerController player; // this variable is set when the InventoryItem is added to the inventory
    public delegate void UseEvent();
    public UseEvent useEvent;
}
