using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class GestureActivator : MonoBehaviour
{
    public UnityEvent onLeftHandGestureRecognized;
    public UnityEvent onRightHandGestureRecognized;

    public void OnLeftHandGestureRecongnition()
    {
        Debug.Log(" <<<<< Gesture Recongized >>>>> ");
        onLeftHandGestureRecognized.Invoke();
        Button button = GetComponent<Button>();
    }

    public void OnRightHandGestureRecongnition()
    {
        Debug.Log(" <<<<< Gesture Recongized >>>>> ");
        onRightHandGestureRecognized.Invoke();
    }
}
