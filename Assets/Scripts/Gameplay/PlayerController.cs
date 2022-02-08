using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private Animator animator; // the animator for the character's art

    [SerializeField]
    private float moveSpeed;

    private CharacterController cc;
    public KeyCode up;
    public KeyCode down;
    public KeyCode left;
    public KeyCode right;

    private void Start()
    {
        cc = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 movement = new Vector3(0, 0, 0);
        if (Input.GetKey(up))
        {
            movement += new Vector3(0, 0, 1);
        }
        if (Input.GetKey(left))
        {
            movement += new Vector3(-1, 0, 0);
        }
        if (Input.GetKey(down))
        {
            movement += new Vector3(0, 0, -1);
        }
        if (Input.GetKey(right))
        {
            movement += new Vector3(1, 0, 0);
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
        cc.Move(movement * moveSpeed * Time.deltaTime);
    }
}
