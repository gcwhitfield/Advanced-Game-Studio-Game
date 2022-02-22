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
    public Vector3 lookDirection; // used to determnie where to shoot the father's gun

    private void Awake()
    {
        if (!Instance) Instance = this as FatherController;
    }

    public new void Update()
    {
        base.Update();

        Debug.DrawRay(gameObject.transform.position, gameObject.transform.forward, Color.white, 5.0f);
    }

    public void Shoot() {
        // OLD shooting code (instantiates bullet prefab)
        //GameObject bullet = Instantiate(bulletPrefab,fireSpawn.position,fireSpawn.rotation);
        //Rigidbody rb = bullet.GetComponent<Rigidbody>();
        //rb.AddForce(fireSpawn.up * bulletForce, ForceMode.Impulse);
        Debug.Log("Shoot");

    }
}
