using Oculus.Interaction;
using static Meta.XR.MRUtilityKit.Data;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using Unity.VisualScripting;
using Oculus.Interaction.HandGrab;

public class ExplodedViewGrabHandler : MonoBehaviour
{
    private Grabbable grabbable; // Reference to the Grabbable component
    private Rigidbody body;
    private HandGrabInteractable interactable;
    private Vector3 lastPosition; // Last recorded position of the object
    private Quaternion lastRotation; // Last recorded rotation of the object

    // List to store the tracked positions and rotations
    private List<TransformData> transformHistory = new List<TransformData>();

    // Public property to access the transform history
    public List<TransformData> TransformHistory { get => transformHistory; }

    public bool isGrabbed = false; // Flag to indicate if the object is currently grabbed
    public bool IsResetting { get; private set; } = false; // Flag to indicate if the object is resetting

    private void Awake()
    {
        // Add necessary components if they are not already present
        if (transform.GetComponent<Grabbable>() == null)
        {
            grabbable = gameObject.AddComponent<Grabbable>();
        }
        if (transform.GetComponent<Rigidbody>() == null)
        {
            body = gameObject.AddComponent<Rigidbody>();
            body.isKinematic = true;
            grabbable.InjectOptionalRigidbody(body);
        }
        if (transform.GetComponent<Collider>() == null)
        {
            gameObject.AddComponent<BoxCollider>().isTrigger = true;
        }
        if (transform.GetComponent<HandGrabInteractable>() == null)
        {
            interactable = gameObject.AddComponent<HandGrabInteractable>();
            interactable.InjectOptionalPointableElement(grabbable);
            interactable.InjectRigidbody(body);
        }
    }

    private void Start()
    {
        lastPosition = transform.position; // Initialize last position
        lastRotation = transform.rotation; // Initialize last rotation
        grabbable.WhenPointerEventRaised += HandlePointerEventRaised; // Subscribe to pointer events
    }

    private void HandlePointerEventRaised(PointerEvent raisedEvent)
    {
        // Handle pointer events to update the isGrabbed flag
        if (raisedEvent.Type == PointerEventType.Select)
            isGrabbed = true;
        if (raisedEvent.Type == PointerEventType.Unselect)
            isGrabbed = false;
    }

    private void Update()
    {
        if (isGrabbed)
        {
            // Check if the position or rotation has changed
            if (transform.position != lastPosition || transform.rotation != lastRotation)
            {
                // Store the new position and rotation
                StoreTransformData();

                // Update the last position and rotation to the current ones
                lastPosition = transform.position;
                lastRotation = transform.rotation;
            }
        }
        if (IsResetting)
        {
            StartCoroutine(ResetToInitial()); // Start the reset coroutine
            IsResetting = false; // Reset the IsResetting flag
        }
    }

    private void StoreTransformData()
    {
        // Create a new TransformData instance and add it to the list
        TransformData data = new TransformData(transform.position, transform.rotation);
        transformHistory.Add(data);
    }

    // A method to retrieve the transform history
    public List<TransformData> GetTransformHistory()
    {
        return new List<TransformData>(transformHistory);
    }

    // Method to clear the transform history
    public void RestDataHistory()
    {
        transformHistory.Clear();
    }

    // Method to start resetting the object to its initial state
    public void StartResetToInitial()
    {
        if (!IsResetting)
        {
            IsResetting = true;
        }
    }

    // Coroutine to reset the object to its initial state
    private IEnumerator ResetToInitial()
    {
        // Iterate backwards through the transform history
        for (int i = transformHistory.Count - 1; i >= 0; i--)
        {
            if (i >= transformHistory.Count)
            {
                Debug.LogWarning("Transform history index out of range, breaking out of loop.");
                break;
            }
            Vector3 startPosition = transform.position;
            Quaternion startRotation = transform.rotation;
            Vector3 endPosition = transformHistory[i].Position;
            Quaternion endRotation = transformHistory[i].Rotation;

            float duration = 0.01f; // Adjust this value to control the smoothness
            float elapsed = 0.0f;

            while (elapsed < duration)
            {
                transform.position = Vector3.Lerp(startPosition, endPosition, elapsed / duration);
                transform.rotation = Quaternion.Slerp(startRotation, endRotation, elapsed / duration);
                elapsed += Time.deltaTime;
                yield return null;
            }

            // Ensure the final position and rotation are set
            transform.position = endPosition;
            transform.rotation = endRotation;
        }

        // Optionally clear the history after resetting
        RestDataHistory();
    }

    private void OnDisable()
    {
        grabbable.WhenPointerEventRaised -= HandlePointerEventRaised; // Unsubscribe from pointer events
    }
}

[System.Serializable]
public class TransformData
{
    public Vector3 Position; // Position of the transform
    public Quaternion Rotation; // Rotation of the transform

    public TransformData(Vector3 position, Quaternion rotation)
    {
        Position = position;
        Rotation = rotation;
    }
}
