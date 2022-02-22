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
        if (!Instance) Instance = this as FatherController;
    }

    public new void Update()
    {
        base.Update();

        Debug.DrawRay(gameObject.transform.position, lookDirection * 10.0f, Color.white, 1.0f);
    }

    public void Shoot() {
        // OLD shooting code (instantiates bullet prefab)
        //GameObject bullet = Instantiate(bulletPrefab,fireSpawn.position,fireSpawn.rotation);
        //Rigidbody rb = bullet.GetComponent<Rigidbody>();
        //rb.AddForce(fireSpawn.up * bulletForce, ForceMode.Impulse);
        Debug.Log("Shoot");
        if (Physics.Raycast(gameObject.transform.posi))

    }
}
