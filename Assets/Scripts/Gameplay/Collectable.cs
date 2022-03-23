using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : Interactable
{
    PlayerController player;
    public PlayerController.PlayerType collector;

    public void Collect()
    {
        Debug.Log("Collect");
        ExecuteInteractionEvents();
        // TODO: play the collection sound
        Destroy(gameObject);
    }

    private new void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);

        if (other.tag == "Player")
        {
            Debug.Log("triggered");
            player = other.gameObject.GetComponent<PlayerController>();
            player.ExecuteUponSubmit(Collect);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (player)
        {
            player.CancelExecuteUponSubmit(Collect);
            player = null; 
        }
    }
}
