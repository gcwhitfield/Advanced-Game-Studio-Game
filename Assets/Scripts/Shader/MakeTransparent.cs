using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    public Transform playerPos;
    public List<TransparentObject> inTheWay;
    public List<TransparentObject> transparent;
    private Transform cameraPos;


    private void Awake()
    {
        cameraPos = this.gameObject.transform;
    }

    // Update is called once per frame
    void Update()
    {
        GetAllObjectsInTheWay();
        MakeTransparent();
        MakeSolid();
    }
    private void GetAllObjectsInTheWay()
    {
        inTheWay.Clear();

        float distance = Vector3.Magnitude(cameraPos.position - cameraPos.position);
        Ray rayForward = new Ray(cameraPos.position, playerPos.position - cameraPos.position);
        Ray rayBackward = new Ray(playerPos.position - cameraPos.position, cameraPos.position);
        var hitForward = Physics.RaycastAll(rayForward, distance);
        var hitBackward = Physics.RaycastAll(rayBackward, distance);
        foreach (var hit in hitForward)
        {
            if (hit.collider.gameObject.TryGetComponent(out TransparentObject obj))
            {
                if (!transparent.Contains(obj))
                {
                    inTheWay.Add(obj);
                }
            }
        }
        foreach (var hit in hitBackward)
        {
            if (hit.collider.gameObject.TryGetComponent(out TransparentObject obj))
            {
                if (!transparent.Contains(obj))
                {
                    inTheWay.Add(obj);
                }
            }
        }
    }
    private void MakeTransparent()
    {
        for (int i = 0; i < inTheWay.Count; i++)
        {
            TransparentObject obj = inTheWay[i];
            if (!transparent.Contains(obj))
            {
                obj.ShowTransparent();
                transparent.Add(obj);
            }
        }
    }
    private void MakeSolid()
    {
        for (int i = transparent.Count - 1; i >= 0; i--)
        {
            TransparentObject obj = transparent[i];
            if (!inTheWay.Contains(obj))
            {
                obj.ShowSolid();
                inTheWay.Add(obj);
            }
        }
    }
}
