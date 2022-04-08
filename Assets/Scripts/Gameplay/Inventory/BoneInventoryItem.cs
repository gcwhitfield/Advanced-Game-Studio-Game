using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoneInventoryItem : InventoryItem
{
    public new void Use()
    {
        base.Use();
        Debug.Log("The bone has been thrown! :)");
    }
}
