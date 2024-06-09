using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ColorController : MonoBehaviour
{
    [SerializeField] public Material targetMaterial;

    private Color originalColor;

    private Color currentColor;

    public Color CurrentColor { get => currentColor; }

    private void OnEnable()
    {
        originalColor = targetMaterial.color;
        Debug.Log(" << color = " + originalColor);
        Debug.Log(" << color = " + currentColor);

        currentColor = targetMaterial.color;
        Debug.Log(" << color = " + currentColor);
    }

    public void UpdateRedValue(float value)
    {
        // Update the red component of the material color

        currentColor.r = value / 255f;
        targetMaterial.color = currentColor;
    }

    public void UpdateGreenValue(float value)
    {
        // Update the green component of the material color
        currentColor.g = value / 255f;
        targetMaterial.color = currentColor;
    }

    public void UpdateBlueValue(float value)
    {
        // Update the blue component of the material color
        currentColor.b = value / 255f;
        targetMaterial.color = currentColor;
    }

    private void OnDisable()
    {
        targetMaterial.color = originalColor;
    }
}
