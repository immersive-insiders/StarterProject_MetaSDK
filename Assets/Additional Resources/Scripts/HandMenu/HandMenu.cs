using Oculus.Interaction;
using Oculus.Interaction.Input;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandMenu : MonoBehaviour
{
    // The hand to which the menu is attached
    [SerializeField] private Hand _hand;

    // The button that appears on the wrist to open the menu
    [SerializeField] private GameObject wristButton;

    // The actual hand menu that will be displayed
    [SerializeField] private GameObject handMenu;

    // Active state selector to detect hand poses
    [SerializeField] private ActiveStateSelector poseActiveStateSelector;

    // Offset positions for the button and menu relative to the hand joint
    [SerializeField] private Vector3 buttonPositionOffset;

    [SerializeField] private Vector3 menuPositionOffset;

    // The specific hand joint to which the button and menu will be attached
    [SerializeField] private HandJointId handJointId = HandJointId.HandStart;

    // Internal references to the hand joints for button and menu
    private HandJoint buttonAttachPoint;

    private HandJoint menuAttachPoint;

    // Boolean to track whether the menu is currently active
    private bool isMenuActive = false;

    private void OnEnable()
    {
        // Subscribe to the pose selection events
        poseActiveStateSelector.WhenSelected += ShowWristButton;
        poseActiveStateSelector.WhenUnselected += HideWristButton;

        // Ensure the hand menu is initially inactive
        if (handMenu.activeSelf)
            handMenu.SetActive(false);
    }

    private void Start()
    {
        // Initialize the button position and attach it to the hand joint
        if (wristButton.GetComponent<HandJoint>() == null)
        {
            buttonAttachPoint = wristButton.AddComponent<HandJoint>();
            InitilizeButtonPosition();
        }
        else
        {
            buttonAttachPoint = wristButton.GetComponent<HandJoint>();
            InitilizeButtonPosition();
        }

        // Initialize the menu position and attach it to the hand joint
        if (handMenu.GetComponent<HandJoint>() == null)
        {
            menuAttachPoint = handMenu.AddComponent<HandJoint>();
            InitilizeMenuPosition();
        }
        else
        {
            menuAttachPoint = handMenu.GetComponent<HandJoint>();
            InitilizeMenuPosition();
        }
    }

    // Method to initialize the button position based on the offset
    private void InitilizeButtonPosition()
    {
        buttonAttachPoint.InjectHand(_hand);
        buttonAttachPoint.HandJointId = handJointId;
        buttonAttachPoint.LocalPositionOffset = buttonPositionOffset;
    }

    // Method to initialize the menu position based on the offset
    private void InitilizeMenuPosition()
    {
        menuAttachPoint.InjectHand(_hand);
        menuAttachPoint.HandJointId = handJointId;
        menuAttachPoint.LocalPositionOffset = menuPositionOffset;
    }

    // Method to show the wrist button on the when pose is detected
    public void ShowWristButton()
    {
        wristButton.SetActive(true);

        if (isMenuActive)
            handMenu.SetActive(true);

        // Add animation to make it look better
    }

    // Method to hide the wrist button on the when pose is not detected
    public void HideWristButton()
    {
        wristButton.SetActive(false);
        handMenu.SetActive(false);

        // Add animation to make it look better
    }

    // Method to toggle the UI menu visibility
    public void ToggleUImenu()
    {
        if (handMenu.activeSelf)
        {
            isMenuActive = false;
            handMenu.SetActive(false);
        }
        else
        {
            isMenuActive = true;
            handMenu.SetActive(true);
        }
    }
}
