using Oculus.Interaction;
using Oculus.Interaction.HandGrab;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LightingTool : MonoBehaviour
{
    [SerializeField] private GameObject visuals;

    [SerializeField] private GameObject directionalLight;
    [SerializeField] private GameObject directionalLightPrefab;

    [SerializeField] private Slider xAxis;
    [SerializeField] private Slider yAxis;
    [SerializeField] private Slider zAxis;

    [SerializeField] private Scrollbar contrast;
    [SerializeField] private Scrollbar brightness;
    [SerializeField] private Scrollbar saturation;

    [SerializeField] private float delayBetweenPresses = 1f;

    private TextMeshProUGUI contrastText;
    private TextMeshProUGUI brightnessText;
    private TextMeshProUGUI saturationText;

    private bool pressedFirstTime = false;
    private float lastPressedTime;

    private OVRPassthroughLayer passthroughLayer;

    private void OnEnable()
    {
        passthroughLayer = GameObject.FindAnyObjectByType<OVRPassthroughLayer>();

        if (passthroughLayer.colorMapEditorType != OVRPassthroughLayer.ColorMapEditorType.ColorAdjustment)
            passthroughLayer.colorMapEditorType = OVRPassthroughLayer.ColorMapEditorType.ColorAdjustment;
    }

    private void Start()
    {
        xAxis.value = directionalLight.transform.rotation.eulerAngles.x;
        yAxis.value = directionalLight.transform.rotation.eulerAngles.y;
        zAxis.value = directionalLight.transform.rotation.eulerAngles.z;
        UpdateRotation();

        contrast.value = (passthroughLayer.colorMapEditorContrast + 1f) / 2f;
        brightness.value = (passthroughLayer.colorMapEditorBrightness + 1f) / 2f;
        saturation.value = (passthroughLayer.colorMapEditorSaturation + 1f) / 2f;

        contrastText = contrast.GetComponentInChildren<TextMeshProUGUI>();
        brightnessText = brightness.GetComponentInChildren<TextMeshProUGUI>();
        saturationText = saturation.GetComponentInChildren<TextMeshProUGUI>();

        contrastText.text = passthroughLayer.colorMapEditorContrast.ToString();
        brightnessText.text = passthroughLayer.colorMapEditorBrightness.ToString();
        saturationText.text = passthroughLayer.colorMapEditorSaturation.ToString();

        xAxis.onValueChanged.AddListener(OnSliderValueChanged);
        yAxis.onValueChanged.AddListener(OnSliderValueChanged);
        zAxis.onValueChanged.AddListener(OnSliderValueChanged);

        contrast.onValueChanged.AddListener(SetContrast);
        brightness.onValueChanged.AddListener(SetBrightness);
        saturation.onValueChanged.AddListener(SetSaturation);
    }

    private void OnSliderValueChanged(float value)
    {
        UpdateRotation();
    }

    // Method to update the rotation of the object based on the slider values
    private void UpdateRotation()
    {
        float xRotation = xAxis.value;
        float yRotation = yAxis.value;
        float zRotation = zAxis.value;

        // Apply the rotation to the object
        directionalLight.transform.rotation = Quaternion.Euler(xRotation, yRotation, zRotation);
        directionalLightPrefab.transform.rotation = Quaternion.Euler(xRotation, yRotation, zRotation);
    }

    public void SetBrightness(float value)
    {
        float tempVal = (value * 2) - 1f;
        passthroughLayer.colorMapEditorBrightness = tempVal;
        brightnessText.text = MathF.Round(tempVal, 2).ToString();
    }

    public void SetContrast(float value)
    {
        float tempVal = (value * 2) - 1f;

        passthroughLayer.colorMapEditorContrast = tempVal;
        contrastText.text = MathF.Round(tempVal, 2).ToString();
    }

    public void SetSaturation(float value)
    {
        float tempVal = (value * 2) - 1f;

        passthroughLayer.colorMapEditorSaturation = tempVal;
        saturationText.text = MathF.Round(tempVal, 2).ToString();
    }

    public void ToggleVisuals()
    {
        Debug.Log("<<< TOGGLE VISUAL");
        if (pressedFirstTime) // we've already pressed the button a first time, we check if the 2nd time is fast enough to be considered a double-press
        {
            bool isDoublePress = Time.time - lastPressedTime <= delayBetweenPresses;

            if (isDoublePress)
            {
                Debug.Log("<<< double press");

                if (visuals.activeSelf)
                {
                    visuals.SetActive(false);
                }
                else
                {
                    visuals.SetActive(true);
                }

                pressedFirstTime = false;
            }
        }
        else // we've not already pressed the button a first time
        {
            pressedFirstTime = true; // we tell this is the first time
        }

        lastPressedTime = Time.time;
        Debug.Log("<< last time" + lastPressedTime);

        if (pressedFirstTime && Time.time - lastPressedTime > delayBetweenPresses) // we're waiting for a 2nd key press but we've reached the delay, we can't consider it a double press anymore
        {
            // note that by checking first for pressedFirstTime in the condition above, we make the program skip the next part of the condition if it's not true,
            // thus we're avoiding the "heavy computation" (the substraction and comparison) most of the time.
            // we're also making sure we've pressed the key a first time before doing the computation, which avoids doing the computation while lastPressedTime is still uninitialized
            pressedFirstTime = false;
        }
    }
}
