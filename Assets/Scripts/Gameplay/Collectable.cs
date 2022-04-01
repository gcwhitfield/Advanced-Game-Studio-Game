using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : Interactable
{
    public PlayerController.PlayerType collector;

    public void Collect()
    {
        // TODO: play the collection sound
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
