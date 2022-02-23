using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyableBranches : MonoBehaviour
{
    public void DestroyBranches()
    {
        // all of the children of this gameobject should have rigidbodies
        // when the father shoots the branches, the "kinematic" property of the children
        // will be set to false, which makes them fall to the ground
        foreach (Transform child in transform)
        {
            Rigidbody rb = child.GetComponent<Rigidbody>();
            rb.isKinematic = false;
        }
    }
}
