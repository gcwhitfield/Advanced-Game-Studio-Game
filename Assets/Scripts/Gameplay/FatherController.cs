using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FatherController : PlayerController
{
    public static FatherController Instance { get; private set; }

    public Transform fireSpawn;
    public GameObject bulletPrefab;
    public float bulletForce = 20.0f;

    private void Awake()
    {
        if (!Instance) Instance = this;
        else Destroy(gameObject);
    }

    public new void Update()
    {
        base.Update();

        // play the footstep audio
        AudioManager.Instance.FootstepAudio(gameObject, movement, moveSpeed);
        // play the lantern sound
        AudioManager.Instance.LanternWalkingAudio(gameObject, movement);

        Debug.DrawRay(gameObject.transform.position, lookDirection * 10.0f, Color.white, 1.0f);
    }

    public void Shoot()
    {
        // OLD shooting code (instantiates bullet prefab)
        float rayLength = 3.0f;
        RaycastHit hit;
        if (Physics.Raycast(gameObject.transform.position, lookDirection * rayLength, out hit))
        {
            DestroyableBranches branches = hit.transform.GetComponent<DestroyableBranches>();
            if (branches)
            {
                branches.DestroyBranches();
            }
        }

        // play the shooting sound
        AudioManager.Instance.ShootAudio(gameObject);
    }
}