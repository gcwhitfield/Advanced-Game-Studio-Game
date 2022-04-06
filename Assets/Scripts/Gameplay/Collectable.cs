using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : Interactable
{
    public PlayerController.PlayerType collector;

    public string itemName;
    public Sprite itemIcon;

    public void Collect()
    {
        // TODO: play the collection sound
        InventoryItem i = new InventoryItem();
        i.itemName = itemName;
        i.icon = itemIcon;

        Debug.Log("COLLECT CALLAED AUYA");
        if (collector == PlayerController.PlayerType.DAUGHTER)
        {
            DaughterController.Instance.gameObject.GetComponent<Inventory>().Add(i);
        }
        else
        {
            FatherController.Instance.gameObject.GetComponent<Inventory>().Add(i);
        }

        Destroy(gameObject);
    }

    private new void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);

        if (other.tag == "Player")
        {
            player = other.gameObject.GetComponent<PlayerController>();
            player.ExecuteUponSubmit(Collect);
        }
    }
}
