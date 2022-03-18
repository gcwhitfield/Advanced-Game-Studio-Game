using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class CameraLookAtPlayer : MonoBehaviour
{
    public Transform player;
    float minHeightAboveTerrain;
    Quaternion originalRotation; // the default rotation of the camera

    private void Start()
    {
        originalRotation = transform.rotation;
    }

    private void Update()
    {
        // if we are close to the terrain, move the camera upwards
        Quaternion desiredRotation;
        //if (Physics.)
        if (transform.position.y < minHeightAboveTerrain)
        {
            transform.position = new Vector3(transform.position.x, minHeightAboveTerrain, transform.position.z);
            desiredRotation = transform.rotation;
            desiredRotation.SetLookRotation(player.position);
        } else
        {
            desiredRotation = originalRotation;
        }
        // smoothly interpolate the camera look rotation
        gameObject.transform.rotation = Quaternion.Slerp(gameObject.transform.rotation, desiredRotation, 0.5f);
    }
}
