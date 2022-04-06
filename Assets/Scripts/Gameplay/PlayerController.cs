using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [System.Serializable]
    public enum PlayerType
    {
        FATHER,
        DAUGHTER
    };

    public Animator animator;

    [SerializeField]
    protected float moveSpeed;

    [SerializeField]
    protected float rotateSpeed;

    private CharacterController cc;

    public delegate void PlayerEvent();

    private PlayerEvent events;

    public GameObject inventory;
    // called when the "ShowInventory" button is pressed
    public void ShowInventory()
    {
        // toggle the inventory being on and off
        if (inventory.activeSelf)
        {
            inventory.SetActive(false);
        }
        else
        {
            inventory.SetActive(true);
        }
    }

    // 'collectableObject' is a refernece to the item that the player can currently collect
    // if it is null, then the daughter is not standing nearby any collectbale objects
    // if it is not null, then 'collectableObject' will refer to the Collectable component that
    // she can collect.
    private Collectable collectableObject = null;

    // the value of playerIndex will be set inside of DaughterController.cs or FatherController.cs
    public int playerIndex { get; set; }

    protected Vector3 movement; // the direction that the player is currently moving
    protected Vector3 lookDirection; // the direction that the player is currently looking
    protected PlayerInput playerInput;

    protected void Start()
    {
        cc = GetComponent<CharacterController>();
        lookDirection = gameObject.transform.forward;
    }

    // called when the player presses the "Collect" key in gameplay
    public void Collect()
    {
        Submit(); // the 'collect' does the same thing as the 'submit' action (for now)
    }

    private IEnumerator DestroyAfterSmallDelay(GameObject ob)
    {
        float smallDelay = 0.1f; // seconds
        yield return new WaitForSeconds(smallDelay);
        Destroy(ob);
    }

    protected void OnTriggerEnter(Collider other)
    {
        Collectable c = other.GetComponent<Collectable>();
        if (c)
        {
            collectableObject = c;
            return;
        }
    }

    protected void OnTriggerExit(Collider other)
    {
        Collectable c = other.GetComponent<Collectable>();
        if (c)
        {
            collectableObject = null;
        }
    }

    public void Move(CallbackContext context)
    {
        Vector2 movement2D = context.ReadValue<Vector2>();
        movement = new Vector3(movement2D.x, 0, movement2D.y);
    }

    public void Stop()
    {
        movement = new Vector3(0, 0, 0);
    }

    // 'e' is a delegate that points to a void function. 'e' will get called
    // the next time that the player hits the 'submit' key
    public void ExecuteUponSubmit(PlayerEvent e)
    {
        events += e;
    }

    public void CancelExecuteUponSubmit(PlayerEvent e)
    {
        events -= e;
    }

    // called when the player needs to confirm an action in the UI or in the environment
    public void Submit(bool collectFlag = false)
    {
        // call all of the functions in 'events'
        if (events != null)
        {
            Debug.Log(events);
            events();
        }
        events = null;
    }

    // Update is called once per frame
    protected void Update()
    {
        //character translation animation
        if (animator)
        {
            if (movement != Vector3.zero)
            {
                animator.SetFloat("Speed", 1);
            }
            else
            {
                animator.SetFloat("Speed", 0);
            }
        }

        // apply gravity
        if (!cc.isGrounded)
        {
            movement.y += Physics.gravity.y / 10;
        }
        else
        {
            movement.y = 0.0f;
        }

        // move the character based on the movement input vector
        if (movement != Vector3.zero)
        {
            cc.Move(movement * moveSpeed * Time.deltaTime);
            lookDirection = movement;
        }
    }
}