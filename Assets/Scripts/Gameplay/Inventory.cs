using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// attach this component to Daughter and Father to give them an inventory
public class Inventory : MonoBehaviour
{
    public struct InventoryItem
    {
        public string name;
        public Texture2D icon;
    };
    public List<InventoryItem> inventory;

    public void Add(InventoryItem i)
    {
        inventory.Add(i);
    }

    public void Remove(InventoryItem i)
    {
        inventory.Remove(i);
    }

    public void Remove(string itemName)
    {
        foreach (InventoryItem i in inventory)
        {
            if (i.name == itemName)
            {
                inventory.Remove(i);
                break;
            }
        }
    }
}
