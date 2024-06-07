using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodedViewController : MonoBehaviour
{
    [SerializeField] private float explosionScale = 1.0f;  // Scale of the explosion

    [Tooltip("Value between 0 and 1")]
    [SerializeField] private float explosionValue = 0.0f; // Value from 0 to 1 representing the explosion state

    private Vector3 center;              // Center of the object

    private Vector3[] originalPositions; // Store original positions of the parts
    private Transform[] parts;           // Array of parts
    private ExplodedViewGrabHandler[] explodedViewGrabHandlers; // Array of grab handlers for each part

    private void Start()
    {
        // Use the transform position of the parent object as the center
        center = transform.position;

        // Get all child transforms
        Transform[] allChildTransforms = GetComponentsInChildren<Transform>();

        // Filter the child transforms to only include those with a MeshRenderer component
        List<Transform> filteredParts = new List<Transform>();
        foreach (Transform child in allChildTransforms)
        {
            if (child.GetComponent<MeshRenderer>() != null)
            {
                filteredParts.Add(child);
            }
        }

        // Initialize the parts array with the filtered parts
        parts = filteredParts.ToArray();

        // Initialize the array to store original positions of the parts
        originalPositions = new Vector3[parts.Length];

        // Initialize the array to store grab handlers
        explodedViewGrabHandlers = new ExplodedViewGrabHandler[parts.Length];

        // Add a grab handler to each part and store its original position
        for (int i = 0; i < parts.Length; i++)
        {
            parts[i].gameObject.AddComponent<ExplodedViewGrabHandler>();
            originalPositions[i] = parts[i].position;
            explodedViewGrabHandlers[i] = parts[i].GetComponent<ExplodedViewGrabHandler>();
        }
    }

    // Method to start the explosion view action coroutine
    public void ExplosionViewAction(float value)
    {
        StartCoroutine(ExecuteExplosionViewAction(value));
    }

    // Coroutine to handle the explosion view action
    private IEnumerator ExecuteExplosionViewAction(float value)
    {
        // Wait for all grab handlers to complete resetting before updating positions
        for (int i = 1; i < parts.Length; i++) // Skip the parent object
        {
            if (explodedViewGrabHandlers[i].GetTransformHistory().Count > 0)
            {
                explodedViewGrabHandlers[i].StartResetToInitial();

                if (explodedViewGrabHandlers[i].IsResetting)
                {
                    yield return new WaitUntil(() => !explodedViewGrabHandlers[i].IsResetting);
                }
            }
        }

        // Update the position of each part based on the explosion value
        for (int i = 0; i < parts.Length; i++)
        {
            if (parts[i] != transform) // Skip the parent object
            {
                Vector3 direction = (originalPositions[i] - center).normalized; // Direction from the center to the original position
                float distance = Vector3.Distance(originalPositions[i], center); // Distance from the center to the original position
                parts[i].position = Vector3.Lerp(originalPositions[i], originalPositions[i] + direction * distance * explosionScale, value);
            }
        }
    }
}
