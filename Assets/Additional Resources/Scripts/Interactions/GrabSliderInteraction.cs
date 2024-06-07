using Oculus.Interaction;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GrabSliderInteraction : MonoBehaviour
{
    [SerializeField] private Vector3 sliderStartPos;
    [SerializeField] private Vector3 sliderEndPos;
    [SerializeField] private Transform sliderTransform;
    [SerializeField] private bool wholeNumber;

    public float minValue = 0f;
    public float maxValue = 1f;

    [System.Serializable]
    public class SliderValueChangedEvent : UnityEvent<float>
    { }

    public SliderValueChangedEvent OnSliderValueChanged;

    private Vector3 previousPosition;

    private void Start()
    {
        // Initialize previous position
        previousPosition = sliderTransform.localPosition;
    }

    private void Update()
    {
        // Check if the position has changed
        if (sliderTransform.localPosition != previousPosition)
        {
            // Calculate the slider value based on the new position
            float sliderValue = GetSliderValue(sliderTransform.localPosition);

            OnSliderValueChanged.Invoke(sliderValue);

            // Update the previous position
            previousPosition = sliderTransform.localPosition;
        }
    }

    private float GetSliderValue(Vector3 currentPosition)
    {
        // Calculate the total distance between start and end points
        float totalDistance = Vector3.Distance(sliderStartPos, sliderEndPos);

        // Calculate the distance from the start point to the current position
        float distanceToCurrent = Vector3.Distance(sliderStartPos, currentPosition);

        // Clamp the distance to the range [0, totalDistance]
        distanceToCurrent = Mathf.Clamp(distanceToCurrent, 0, totalDistance);

        // Calculate the normalized value (0 to 1) based on the distance
        float normalizedValue = distanceToCurrent / totalDistance;

        // Interpolate between minValue and maxValue based on the normalized value
        float interpolatedValue = Mathf.Lerp(minValue, maxValue, normalizedValue);

        // Round the value to 2 decimal places
        float roundedValue = Mathf.Round(interpolatedValue * 100f) / 100f;

        // Ensure the value is exactly 0 at the start and 1 at the end
        if (currentPosition == sliderStartPos)
        {
            roundedValue = minValue;
        }
        else if (currentPosition == sliderEndPos)
        {
            roundedValue = maxValue;
        }

        // Return the value based on wholeNumber flag
        if (wholeNumber)
        {
            return Mathf.Round(roundedValue);
        }
        else
        {
            return roundedValue;
        }
    }
}
