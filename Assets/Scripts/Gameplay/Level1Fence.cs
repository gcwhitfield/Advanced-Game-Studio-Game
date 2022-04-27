using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Interactable))]
public class Level1Fence : MonoBehaviour
{
    Inventory i;
    Interactable interactable; // this needs to be attached to the same GameObject
    // that this component is attached to 

    // Start is called before the first frame update
    void Start()
    {
        i = FatherController.Instance.GetComponent<Inventory>();
        interactable = GetComponent<Interactable>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (other.gameObject.GetComponent<FatherController>())
            {
                Debug.Log("Father has entered level 1 fence trigger");
                if (i.Contains("Key"))
                {
                    Debug.Log("Father will execute level complete on trigger enter");
                    interactable.ExecuteOnInteract(Level1Controller.Instance.OnLevelCompleted);
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (other.gameObject.GetComponent<FatherController>())
            {
                if (i.Contains("Key"))
                {
                    interactable.CancelExecuteOnInteract(Level1Controller.Instance.OnLevelCompleted);
                }
            }
        }
    }
}