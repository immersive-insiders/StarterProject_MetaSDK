using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GrabKnobInteraction : MonoBehaviour
{
    [SerializeField] private Transform knobTrasform;
    public IntEvent OnKnobValueChanged; // Custom integer event

    private float previousYRotation;

    private void Start()
    {
        if (knobTrasform == null)
        {
            Debug.LogError("Target transform is not assigned!");
            return;
        }

        previousYRotation = knobTrasform.rotation.eulerAngles.y;
    }

    private void LateUpdate()
    {
        if (knobTrasform == null)
            return;

        float currentYRotation = knobTrasform.rotation.eulerAngles.y;

        if (!Mathf.Approximately(currentYRotation, previousYRotation))
        {
            // Y rotation value has changed
            int roundedYRotation = Mathf.RoundToInt(currentYRotation);
            OnKnobValueChanged.Invoke(roundedYRotation);
            previousYRotation = currentYRotation;
        }
    }
}

[System.Serializable]
public class IntEvent : UnityEvent<int>
{ } // Custom integer event type
