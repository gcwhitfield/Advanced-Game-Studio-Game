using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private Animator animator; // the animator for the character's art

    [SerializeField]
    private float moveSpeed;

    private CharacterController cc;

    private void Start()
    {
        cc = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 movement = new Vector3(0, 0, 0);
        if (Input.GetKey(KeyCode.W))
        {
            movement += new Vector3(1, 0, 0);
        }
        if (Input.GetKey(KeyCode.A))
        {
            movement += new Vector3(0, -1, 0);
        }
        if (Input.GetKey(KeyCode.S))
        {
            movement += new Vector3(-1, 0, 0);
        }
        if (Input.GetKey(KeyCode.D))
        {
            movement += new Vector3(0, 1, 0);
        }
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
        cc.Move(movement * moveSpeed);
    }
}
