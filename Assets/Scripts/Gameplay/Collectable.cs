using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : Interactable
{
    public PlayerController.PlayerType collector;

    public string itemName;
    public Sprite itemIcon;
    public InventoryItemUseEvents.UseEvent useEvent = InventoryItemUseEvents.UseEvent.NONE;
    public bool deleteFromInventoryOnUse = true;

    public void Collect()
    {
        InventoryItem i = new InventoryItem();
        i.itemName = itemName;
        i.icon = itemIcon;
        i.ExecuteUponUse(InventoryItemUseEvents.GetEvent(useEvent));
        i.deleteFromInventoryOnUse = deleteFromInventoryOnUse;
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
            if (collector == PlayerController.PlayerType.FATHER)
            {
                FatherController f = other.gameObject.GetComponent<FatherController>();
                if (f != null)
                {
                    f.ExecuteUponSubmit(Collect);
                }
            } else if (collector == PlayerController.PlayerType.DAUGHTER)
            {
                DaughterController d = other.gameObject.GetComponent<DaughterController>();
                if (d != null)
                {
                    d.ExecuteUponSubmit(Collect);
                }
            }
        }
    }

    private new void OnTriggerExit(Collider other)
    {
        base.OnTriggerExit(other);

        if (other.tag == "Player")
        {
            if (collector == PlayerController.PlayerType.FATHER)
            {
                FatherController f = other.gameObject.GetComponent<FatherController>();
                if (f != null)
                {
                    f.CancelExecuteUponSubmit(Collect);
                }
            }
            else if (collector == PlayerController.PlayerType.DAUGHTER)
            {
                DaughterController d = other.gameObject.GetComponent<DaughterController>();
                if (d != null)
                {
                    d.CancelExecuteUponSubmit(Collect);
                }
            }
        }
    }
}
