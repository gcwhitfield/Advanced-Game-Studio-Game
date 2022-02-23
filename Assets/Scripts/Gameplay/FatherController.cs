using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using FMODUnity;

public class FatherController : PlayerController
{
    public static FatherController Instance { get; private set; }

    public Transform fireSpawn;
    public GameObject bulletPrefab;
    public float bulletForce = 20.0f;

    public EventReference shootAudio;
    private FMOD.Studio.EventInstance shootInstance;

    private void Awake()
    {
        if (!Instance) Instance = this as FatherController;
        shootInstance = RuntimeManager.CreateInstance(shootAudio);
        RuntimeManager.AttachInstanceToGameObject(shootInstance, GetComponent<Transform>(), bulletPrefab.GetComponent<Rigidbody>());
    }

    public new void Update()
    {
        base.Update();

        Debug.DrawRay(gameObject.transform.position, lookDirection * 10.0f, Color.white, 1.0f);
    }

    public void Shoot()
    {
        // OLD shooting code (instantiates bullet prefab)
        //GameObject bullet = Instantiate(bulletPrefab,fireSpawn.position,fireSpawn.rotation);
        //Rigidbody rb = bullet.GetComponent<Rigidbody>();
        //rb.AddForce(fireSpawn.up * bulletForce, ForceMode.Impulse);
        Debug.Log("Shoot");
        float rayLength = 3.0f;
        RaycastHit hit;
        if (Physics.Raycast(gameObject.transform.position, lookDirection * rayLength, out hit))
        {
            Debug.Log("The RayCast has hit " + hit.transform.name);
            DestroyableBranches branches = hit.transform.GetComponent<DestroyableBranches>();
            if (branches)
            {
                branches.DestroyBranches();
            }
            else
            {
                Debug.Log("did NOT hit branches");
            }
        }

        // play the shooting sound
        shootInstance.start();
        //shootInstance.release();
    }
}