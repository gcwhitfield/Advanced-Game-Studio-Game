using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ToonPostProcessBlit : MonoBehaviour
{
    [SerializeField] private Material toonPostProcessMaterial;

    [Range(1, 10)] public int lightLevels;
    [Range(1, 255)] public int colorLevels;
    [Range(0, 1)] public float brightnessOffset;
    [Range(0, 2)] public float lightSpreadFactor;
    [Range(0, 5)] public float exposure;
    

    public void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        toonPostProcessMaterial.SetInt("_LightLevels", lightLevels);
        toonPostProcessMaterial.SetFloat("_BrightnessOffset", brightnessOffset);
        toonPostProcessMaterial.SetFloat("_LightSpreadFactor", lightSpreadFactor);
        toonPostProcessMaterial.SetFloat("_Exposure", exposure);
        toonPostProcessMaterial.SetFloat("_ColorLevels", colorLevels);
        Graphics.Blit(source, destination, toonPostProcessMaterial);
    }
}
