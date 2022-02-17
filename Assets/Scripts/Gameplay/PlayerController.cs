using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private Animator animator; // the animator for the character's art

    [SerializeField]
    private float moveSpeed;

    [SerializeField]
    private float rotateSpeed;

    private CharacterController cc;

    // the value of playerIndex will be set inside of DaughterController.cs or FatherController.cs
    public int playerIndex { get; set; }

    private Vector3 movement;

    protected PlayerInput playerInput;

    protected void Start()
    {
        cc = GetComponent<CharacterController>();
    }

    public void OnMove(CallbackContext context)
    {
        Debug.Log("OnMove");
        Vector2 movement2D = context.ReadValue<Vector2>();
        movement = new Vector3(movement2D.x, 0, movement2D.y);
    }

    // Update is called once per frame
    void Update()
    {
        //character translation
        if (animator)
        {
            if (movement != Vector3.zero)
            {
                animator.SetInteger("Speed", 1);
            } else
            {
                animator.SetInteger("Speed", 0);
            }
        }

        if (movement != Vector3.zero) {
            cc.Move(movement * moveSpeed * Time.deltaTime);
            //character rotation
            //Quaternion toRoration = Quaternion.LookRotation(movement, Vector3.up);
            //transform.rotation = Quaternion.RotateTowards(transform.rotation, toRoration, rotateSpeed * Time.deltaTime);
        }
    }
}
