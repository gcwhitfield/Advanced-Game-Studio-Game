using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// attach this component to a gameobject to make its transform always look at
// another transform
[ExecuteInEditMode]
public class TransformLookAt : MonoBehaviour
{
    public Transform transformToLookAt;

    // Update is called once per frame
    void LateUpdate()
    {
        if (transformToLookAt != null)
        {
            Debug.Log("Looking at transform");
            transform.LookAt(transform);
        } else
        {
            Debug.LogError("Transform in TransformLookAt is null. " + name + " will not look at anything");
        }
    }
}
