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

    public FadeMode FadingMode;

    public enum FadeMode
    {
        Transparent,
        Fade
    }

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

        for (int i = 0; i < Materials.Count; i++)
        {
            if (FadingMode == FadeMode.Fade)
            {
                Materials[i].DisableKeyword("_ALPHABLEND_ON");
            }
            else
            {
                Materials[i].DisableKeyword("_ALPHAPREMULTIPLY_ON");
            }
            Materials[i].SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
            Materials[i].SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
            Materials[i].SetInt("_ZWrite", 1); // re-enable Z Writing
            Materials[i].renderQueue = (int)UnityEngine.Rendering.RenderQueue.Geometry;
        }
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