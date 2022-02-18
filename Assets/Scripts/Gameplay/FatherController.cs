using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FatherController : PlayerController
{
    public static FatherController Instance { get; private set; }

    [SerializeField]
    private GameObject fatherInputHandlerPrefab;

    public Transform fireSpawn;
    public GameObject bulletPrefab;
    public float bulletForce = 20.0f;

    private void Awake()
    {
        if (!Instance) Instance = this as FatherController;
    }

    //private void Update()
    //{
    //    if (Input.GetButtonDown("Fire1")) {
    //        shoot();
    //    }
    //}

    public void Shoot() {
        GameObject bullet = Instantiate(bulletPrefab,fireSpawn.position,fireSpawn.rotation);
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        rb.AddForce(fireSpawn.up * bulletForce, ForceMode.Impulse);
    }
}
