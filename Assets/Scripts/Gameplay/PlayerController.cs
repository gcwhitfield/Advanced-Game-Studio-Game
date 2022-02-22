using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    public Animator animator;

    [SerializeField]
    protected float moveSpeed;

    [SerializeField]
    protected float rotateSpeed;

    private CharacterController cc;

    // the value of playerIndex will be set inside of DaughterController.cs or FatherController.cs
    public int playerIndex { get; set; }

    protected Vector3 movement; // the direction that the player is currently moving
    protected PlayerInput playerInput;

    protected void Start()
    {
        cc = GetComponent<CharacterController>();
    }

    public void OnMove(CallbackContext context)
    {
        //Debug.Log("OnMove");
        Vector2 movement2D = context.ReadValue<Vector2>();
        movement = new Vector3(movement2D.x, 0, movement2D.y);
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
            } else
            {
                animator.SetFloat("Speed", 0);
            }    
        }

        // move the character based on the movement input vector
        if (movement != Vector3.zero) {
            cc.Move(movement * moveSpeed * Time.deltaTime);
            //character rotation
            //Quaternion toRoration = Quaternion.LookRotation(movement, Vector3.up);
            //transform.rotation = Quaternion.RotateTowards(transform.rotation, toRoration, rotateSpeed * Time.deltaTime);
        }
    }
}
