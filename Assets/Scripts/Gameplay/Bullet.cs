using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public GameObject hitEffect;

    private void OnCollisionEnter(Collision collision)
    {
        GameObject exposion = Instantiate(hitEffect, transform.position, Quaternion.identity);
        Destroy(exposion,1f);
        Destroy(gameObject);
    }
}
