using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class MonsterIsNearBlit : MonoBehaviour
{
    public Material spookyMonsterIsNearMaterial;
    public Color color;

    [Range(0, 1)]
    public float intensity; // the intensity of the spooky effect

    // this is the max distance that the monster must be from the player before the effect
    // before the effect begins to play
    public float effectBeginDistance = 10;

    public GameObject monster;

    float maxIntensity = 0.22f; // clamps the value of "_Intensity" inside of the shader
    // between 0 and maxIntensity

    private void FixedUpdate()
    {
        float dist = Vector3.Distance(gameObject.transform.parent.position, monster.transform.position);
        if (dist < effectBeginDistance)
        {
            intensity = (effectBeginDistance - dist) / effectBeginDistance;
        }
    }
    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        spookyMonsterIsNearMaterial.SetFloat("_Intensity", intensity * maxIntensity);
        spookyMonsterIsNearMaterial.SetVector("_Color", color);
        Graphics.Blit(source, destination, spookyMonsterIsNearMaterial);
    }
}
