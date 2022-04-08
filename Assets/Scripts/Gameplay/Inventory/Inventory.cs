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

    // this is the inventory item prefab that gets instantiated into the inventoryLayoutGroup
    public GameObject InventoryItemUIPrefab;

    protected int currSelected = 0;

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
        GameObject g = GameObject.Instantiate(InventoryItemUIPrefab, inventoryLayoutGroup.transform);
        Image img = g.transform.Find("Img").GetComponent<Image>();
        img.sprite = i.icon;
        g.name = i.itemName;

        ShowSelectedItem();
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

    // displays the selection box around the item that is currently selected in the inventory
    void ShowSelectedItem()
    {
        if (inventory.Count > 0)
        {
            Debug.Log("Inventory size: " + inventory.Count.ToString() + " : currSeleted " + currSelected.ToString());
            foreach (Transform t in inventoryLayoutGroup.transform)
            {
                if (t.name != inventory[currSelected].itemName)
                {
                    t.Find("SelectionBox").gameObject.GetComponent<Image>().enabled = false;
                } else
                {
                    t.Find("SelectionBox").gameObject.GetComponent<Image>().enabled = true;
                }
            }
        }
    }

    // called from PlayerController when the player presses directional pad 
    public void IncrementSelection()
    {
        if (inventory.Count > 0)
        {
            currSelected++;
            if (currSelected >= inventory.Count)
            {
                currSelected = 0;
            }
            ShowSelectedItem();
        }
    }

    // called from PlayerController when the player presses the directional pad
    public void DecrementSelection()
    {
        if (inventory.Count > 0)
        {
            currSelected--;
            if (currSelected < 0)
            {
                currSelected = inventory.Count - 1;
            }
            ShowSelectedItem();
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
