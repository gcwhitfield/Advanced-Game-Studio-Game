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
    private void LateUpdate()
    {
        if (transformToLookAt != null)
        {
            //Debug.Log("Looking at transform");
            transform.LookAt(transform);
            //transform.LookAt(transformToLookAt);
            //transform.rotation = transformToLookAt.transform.rotation;
        }
        else
        {
            //Debug.LogError("Transform in TransformLookAt is null. " + name + " will not look at anything");
        }
        //transform.rotation = Quaternion.Euler(0f, transform.rotation.eulerAngles.y, 0f);
    }
}