using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public GameObject hitEffect;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Bullet has hit " + other.gameObject.name.ToString());
        if (other.gameObject.tag != "Player")
        {
            GameObject exposion = Instantiate(hitEffect, transform.position, Quaternion.identity);
            Destroy(exposion,1f);
            Destroy(gameObject);
        }
    }
}
