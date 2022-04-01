using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryItemCollectable : Collectable
{
    public string itemName;
    public Texture2D itemIcon;

    public new void Collect()
    {
        base.Collect();
        Inventory.InventoryItem i;
        i.name = itemName;
        i.icon = itemIcon;

        if (collector == PlayerController.PlayerType.DAUGHTER)
        {
            DaughterController.Instance.GetComponent<Inventory>().Add(i);
        } else
        {
            FatherController.Instance.GetComponent<Inventory>().Add(i);
        }
    }

}
