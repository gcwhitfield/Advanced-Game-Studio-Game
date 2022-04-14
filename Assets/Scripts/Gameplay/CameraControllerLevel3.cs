using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControllerLevel3: MonoBehaviour
{
    [Range(0,1)]
    public float movementDampening;
    public Interactable clearingCheckpoint;
    public Transform clearingCameraWaypoint;
    bool checkpointReached = false;

    Vector3 offset;

    private void Start()
    {
        offset = transform.position - new Vector3(
                (FatherController.Instance.transform.position.x +
                    DaughterController.Instance.transform.position.x) / 2.0f,
                0,
                (FatherController.Instance.transform.position.z +
                    DaughterController.Instance.transform.position.z) / 2.0f
            );
        clearingCheckpoint.ExecuteOnTriggerEnter(OnClearingCheckpointReached);

    }

    // this function is called when the clearing checkpoint has been hit
    public void OnClearingCheckpointReached()
    {
        //checkpointReached = true;
        //Debug.Log("Clearing chekpoint has been reached!");
    }

    void Update()
    {
        Vector3 oldPosition = transform.position;
        Vector3 newPosition;
        Quaternion oldRotation = transform.rotation;
        Quaternion newRotation;
        if (!checkpointReached)
        {
            // position the x and z coordinate of the camera in between the daughter and father
            newPosition = new Vector3(
                    (FatherController.Instance.transform.position.x +
                        DaughterController.Instance.transform.position.x) / 2.0f,
                    0,
                    (FatherController.Instance.transform.position.z +
                        DaughterController.Instance.transform.position.z) / 2.0f
                );
            newRotation = transform.rotation;
        } else
        {
            newPosition = clearingCheckpoint.transform.position;
            newRotation = clearingCheckpoint.transform.rotation;
        }
        transform.position = Vector3.Lerp(oldPosition, newPosition + offset, 1.0f-movementDampening);
        transform.rotation = Quaternion.Lerp(oldRotation, newRotation, 1.0f-movementDampening);
    }
}
