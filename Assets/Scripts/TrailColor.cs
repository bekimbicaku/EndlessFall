using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailColor : MonoBehaviour
{
    public TrailRenderer trailRenderer;

    void Start()
    {
        // Get the material assigned to the Trail Renderer
        Material trailMaterial = trailRenderer.material;

        // Set the material's color to match the trail's start color
        trailMaterial.SetColor("_BaseColor", trailRenderer.startColor);
    }

    public void Update()
    {
        // Interpolate the color between start and end based on time or trail settings
        Color dynamicColor = Color.Lerp(trailRenderer.startColor, trailRenderer.endColor, 0.5f);

        // Set the material's base color to this dynamic color
        trailRenderer.material.SetColor("_BaseColor", dynamicColor);
    }


}
