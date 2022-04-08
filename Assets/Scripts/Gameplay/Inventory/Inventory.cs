using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// attach this component to Daughter and Father to give them an inventory
public class Inventory : MonoBehaviour
{
    //[System.Serializable]
    //public struct InventoryItem
    //{
    //    public string name;
    //    public Sprite icon;
    //};
    public List<InventoryItem> inventory;

    public VerticalLayoutGroup inventoryLayoutGroup;
    PlayerController player;

    protected int currSelected;

    private void Start()
    {
        // get a reference to the player component that this Inventory is attached to
        if (gameObject.GetComponent<DaughterController>())
        {
            player = DaughterController.Instance;
        } else
        {
            player = FatherController.Instance;
        }
    }


    public void Add(InventoryItem i)
    {
        if (gameObject.GetComponent<FatherController>())
        {
            i.player = FatherController.Instance;
        } else
        {
            i.player = DaughterController.Instance;
        }
        inventory.Add(i);

        // create a new gameobject, insert into layout group
        GameObject g = new GameObject();
        g.AddComponent<RectTransform>();
        Image img = g.AddComponent<Image>();
        img.sprite = i.icon;
        g.name = i.itemName;
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

    // called from PlayerController when the player presses directional pad 
    public void IncrementSelection()
    {
        if (inventory.Count > 0)
        {
            currSelected++;
            currSelected %= inventory.Count;
        }
    }

    // called from PlayerController when the player presses the directional pad
    public void DecrementSelection()
    {
        if (inventory.Count > 0)
        {
            currSelected--;
            currSelected %= inventory.Count;
        }
    }

    // called from PlayerController when the inventory is opened
    public void OnShowInventory()
    {
        player.ExecuteUponSubmit(UseItem);
    }

    // called from PlayerController when the inventory is hidden
    public void OnHideInventory()
    {
        player.CancelExecuteUponSubmit(UseItem);
    }

    // called wen the item is used
    public void UseItem()
    {
        if (inventory[currSelected])
        {
                inventory[currSelected].Use();
                Remove(inventory[currSelected]);
        } else
        {
            Debug.LogError("Inventory current seletion is null!");
        }
    }
}
