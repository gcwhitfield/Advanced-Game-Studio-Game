using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadingObject : MonoBehaviour, IEquatable<FadingObject>
{
    public List<Renderer> Renderers = new List<Renderer>();
    public Vector3 Position;
    public List<Material> Materials = new List<Material>();

    [HideInInspector]
    public float InitialAlpha;

    [HideInInspector]
    public float targetAlpha;

    private void Awake()
    {
        Position = transform.position;
        if (Renderers.Count == 0)
        {
            Renderers.AddRange(GetComponentsInChildren<Renderer>());
        }
        for (int i = 0; i < Renderers.Count; i++)
        {
            Materials.AddRange(Renderers[i].materials);
        }

        InitialAlpha = Materials[0].color.a;
        targetAlpha = InitialAlpha;
    }

    public bool Equals(FadingObject other)
    {
        return Position.Equals(other.Position);
    }

    public override int GetHashCode()
    {
        return Position.GetHashCode();
    }
}