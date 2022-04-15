using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyableBranches : MonoBehaviour
{
    public BoxCollider collider;

    public void DestroyBranches()
    {
        // all of the children of this gameobject should have rigidbodies
        // when the father shoots the branches, the "kinematic" property of the children
        // will be set to false, which makes them fall to the ground
        foreach (Transform child in transform)
        {
            // loop over each of the transform of children of children
            foreach (Transform c in child)
            {
                Rigidbody cRb = c.GetComponent<Rigidbody>();
                if (cRb)
                {
                    cRb.isKinematic = false;
                }
            }
            Rigidbody rb = child.GetComponent<Rigidbody>();
            if (rb)
            {
                rb.isKinematic = false;
            }
        }
        collider.isTrigger = true;
    }
}
