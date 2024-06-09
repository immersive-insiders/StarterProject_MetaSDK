using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SliderColorController : MonoBehaviour
{
    [SerializeField] private GrabSliderInteraction redSlider;
    [SerializeField] private GrabSliderInteraction greenSlider;
    [SerializeField] private GrabSliderInteraction blueSlider;

    [SerializeField] private ColorController colorController;

    private void OnEnable()
    {
        Color currentColor = colorController.CurrentColor;
    }

    private void Start()
    {
        redSlider.OnSliderValueChanged.AddListener(OnRedColorChange);
        greenSlider.OnSliderValueChanged.AddListener(OnGreenColorChange);
        blueSlider.OnSliderValueChanged.AddListener(OnBlueColorChange);
    }

    private void OnRedColorChange(float val)
    {
        colorController.UpdateRedValue(val);
    }

    private void OnGreenColorChange(float val)
    {
        colorController.UpdateGreenValue(val);
    }

    private void OnBlueColorChange(float val)
    {
        colorController.UpdateBlueValue(val);
    }
}
