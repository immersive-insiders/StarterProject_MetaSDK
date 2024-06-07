using Oculus.Interaction;
using Oculus.Interaction.HandGrab;
using Oculus.Interaction.Input;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandPoseTracker : MonoBehaviour
{
    // Reference to the hand reference
    [SerializeField] private HandRef leftHandRef;

    [SerializeField] private HandRef rightHandRef;

    // Reference to the SkinnedMeshRenderer for visualization
    [SerializeField] private SkinnedMeshRenderer rightHandRenderer;

    [SerializeField] private SkinnedMeshRenderer leftHandRenderer;

    // Colors for hands when detected
    [SerializeField] private Color defaultHandColor;

    [SerializeField] private Color poseDetectedHandColor;

    // Reference to the ActiveStateSelector for when the poses are detected
    [SerializeField] private ActiveStateSelector leftActiveStateSelector;

    [SerializeField] private ActiveStateSelector rightActiveStateSelector;

    // Boolean values for pose detection
    public bool leftHandPose = false;

    public bool rightHandPose = false;

    // Variables to store the position and rotation of the index finger
    private Pose leftIndexPose;

    private Pose rightIndexPose;

    // Property to access the index finger pose
    public Pose LeftIndexPose { get => leftIndexPose; }

    public Pose RightIndexPose { get => rightIndexPose; }

    // Variables to store the hand material for visualization
    private Material leftHandMaterial;

    private Material rightHandMaterial;

    private void Awake()
    {
        leftHandMaterial = leftHandRenderer.sharedMaterials[0];
        rightHandMaterial = rightHandRenderer.sharedMaterials[0];
    }

    private void OnEnable()
    {
        leftActiveStateSelector.WhenSelected += HandleSelectedLeftHand;
        leftActiveStateSelector.WhenUnselected += HandleUnselectedLeftHand;

        rightActiveStateSelector.WhenSelected += HandleSelectedRightHand;
        rightActiveStateSelector.WhenUnselected += HandleUnselectedRightHand;
    }

    private void HandleSelectedLeftHand()
    {
        IsLeftHandPoseDetected(true);
    }

    private void HandleUnselectedLeftHand()
    {
        IsLeftHandPoseDetected(false);
    }

    private void HandleSelectedRightHand()
    {
        IsRightHandPoseDetected(true);
    }

    private void HandleUnselectedRightHand()
    {
        IsRightHandPoseDetected(false);
    }

    // Update the left hand pose detection status and adjust material color
    public void IsLeftHandPoseDetected(bool handPose)
    {
        leftHandPose = handPose;
        if (leftHandPose)
            leftHandMaterial.SetColor("_OutlineColor", poseDetectedHandColor);
        else
            leftHandMaterial.SetColor("_OutlineColor", defaultHandColor);
    }

    // Update the right hand pose detection status and adjust material color
    public void IsRightHandPoseDetected(bool handPose)
    {
        rightHandPose = handPose;
        if (rightHandPose)
            rightHandMaterial.SetColor("_OutlineColor", poseDetectedHandColor);
        else
            rightHandMaterial.SetColor("_OutlineColor", defaultHandColor);
    }

    private void Update()
    {
        // Only update index finger poses if both hand poses are detected
        if (!leftHandPose || !rightHandPose)
            return;

        // Get the index finger pose for both hands
        leftHandRef.GetJointPose(HandJointId.HandIndexTip, out leftIndexPose);
        rightHandRef.GetJointPose(HandJointId.HandIndexTip, out rightIndexPose);
    }

    private void OnDisable()
    {
        // Reset hand material color when script is disabled
        leftHandMaterial.SetColor("_OutlineColor", defaultHandColor);
        rightHandMaterial.SetColor("_OutlineColor", defaultHandColor);

        leftActiveStateSelector.WhenSelected -= HandleSelectedLeftHand;
        leftActiveStateSelector.WhenUnselected -= HandleUnselectedLeftHand;

        rightActiveStateSelector.WhenSelected -= HandleSelectedRightHand;
        rightActiveStateSelector.WhenUnselected -= HandleUnselectedRightHand;
    }
}
