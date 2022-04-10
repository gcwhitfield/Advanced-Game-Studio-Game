using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleDisplayCameraFollow : MonoBehaviour
{
    [Range(0,1)]
    public float movementDampening;

    Vector3 offset;

    private void Start()
    {
        offset = transform.position - new Vector3(
                (FatherController.Instance.transform.position.x +
                    DaughterController.Instance.transform.position.x) / 2.0f,
                0,
                (FatherController.Instance.transform.position.z +
                    DaughterController.Instance.transform.position.z) / 2.0f
            ); ;
    }
    void Update()
    {
        // position the x and z coordinate of the camera in between the daughter and father
        Vector3 oldPosition = transform.position;
        Vector3 newPosition = new Vector3(
                (FatherController.Instance.transform.position.x +
                    DaughterController.Instance.transform.position.x) / 2.0f,
                0,
                (FatherController.Instance.transform.position.z +
                    DaughterController.Instance.transform.position.z) / 2.0f
            );
        transform.position = Vector3.Lerp(oldPosition, newPosition + offset, 1.0f-movementDampening);
    }
}
