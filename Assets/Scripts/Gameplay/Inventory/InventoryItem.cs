using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InventoryItem
{
    public string itemName;
    public Sprite icon;
    public PlayerController player; // this variable is set when the InventoryItem is added to the inventory
    public delegate void UseEvent();
    public UseEvent useEvents;

    public InventoryItem() { }
    ~InventoryItem() { }

    public void ExecuteUponUse(UseEvent e)
    {
        useEvents += e;
    }

    public void CancelExecuteUponUse(UseEvent e)
    {
        useEvents -= e;
    }

    // this function will get overwritten by a subclass of InventoryItem
    public void Use()
    {
        if (useEvents != null)
        {
            useEvents();
            useEvents = null;
        }
    }
}
