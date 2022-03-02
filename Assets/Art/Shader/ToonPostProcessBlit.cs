using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ToonPostProcessBlit : MonoBehaviour
{
    [SerializeField] private Material toonPostProcessMaterial;

    public int lightLevels;
    [Range(0, 1)] public float brightnessOffset;
    [Range(0, 2)] public float lightSpreadFactor;
    [Range(0, 5)] public float exposure;

    public void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        toonPostProcessMaterial.SetInt("_LightLevels", lightLevels);
        toonPostProcessMaterial.SetFloat("_BrightnessOffset", brightnessOffset);
        toonPostProcessMaterial.SetFloat("_LightSpreadFactor", lightSpreadFactor);
        toonPostProcessMaterial.SetFloat("_Exposure", exposure);
        Graphics.Blit(source, destination, toonPostProcessMaterial);
    }
}
