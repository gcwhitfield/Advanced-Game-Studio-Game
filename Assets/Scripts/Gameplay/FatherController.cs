using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FatherController : PlayerController
{
    public Transform fireSpawn;
    public GameObject bulletPrefab;
    public float bulletForce = 20.0f;

    private void Update()
    {
        if (Input.GetButtonDown("Fire1")) {
            shoot();
        }
    }

    void shoot() {
        GameObject bullet = Instantiate(bulletPrefab,fireSpawn.position,fireSpawn.rotation);
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        rb.AddForce(fireSpawn.up * bulletForce, ForceMode.Impulse);
    }
}
