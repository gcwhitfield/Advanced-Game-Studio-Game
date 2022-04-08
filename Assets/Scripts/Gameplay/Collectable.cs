using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : Interactable
{
    public PlayerController.PlayerType collector;

    public string itemName;
    public Sprite itemIcon;
    public InventoryItemUseEvents.UseEvent useEvent;

    public void Collect()
    {
        // TODO: play the collection sound
        InventoryItem i = new InventoryItem();
        i.itemName = itemName;
        i.icon = itemIcon;
        i.ExecuteUponUse(InventoryItemUseEvents.GetEvent(useEvent));
        Debug.Log("COLLECT CALLAED AUYA");
        if (collector == PlayerController.PlayerType.DAUGHTER)
        {
            DaughterController.Instance.gameObject.GetComponent<Inventory>().Add(i);
            AudioManager.Instance.PickUpAudio(DaughterController.Instance.gameObject);
        }
        else
        {
            FatherController.Instance.gameObject.GetComponent<Inventory>().Add(i);
            AudioManager.Instance.PickUpAudio(FatherController.Instance.gameObject);
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

    private new void OnTriggerExit(Collider other)
    {
        base.OnTriggerExit(other);

        if (other.tag == "Player")
        {
            player = other.gameObject.GetComponent<PlayerController>();
            player.CancelExecuteUponSubmit(Collect);
        }
    }
}
