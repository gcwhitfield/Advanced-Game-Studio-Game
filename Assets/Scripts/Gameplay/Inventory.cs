using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// attach this component to Daughter and Father to give them an inventory
public class Inventory : MonoBehaviour
{
    [System.Serializable]
    public struct InventoryItem
    {
        public string name;
        public Sprite icon;
    };
    public List<InventoryItem> inventory;

    public HorizontalLayoutGroup inventoryLayoutGroup;

    public void Add(InventoryItem i)
    {
        inventory.Add(i);

        // create a new gameobject, insert into layout group
        GameObject g = new GameObject();
        g.AddComponent<RectTransform>();
        Image img = g.AddComponent<Image>();
        img.sprite = i.icon;
        g.name = i.name;
        LayoutElement l = g.AddComponent<LayoutElement>();
        GameObject.Instantiate(g, inventoryLayoutGroup.transform);
    }

    public void Remove(InventoryItem i)
    {
        inventory.Remove(i);

        // remove object from the inventory layout group
        foreach (Transform t in inventoryLayoutGroup.transform)
        {
            if (t.name == i.name)
            {
                Destroy(t.gameObject);
                break;
            }
        }
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
