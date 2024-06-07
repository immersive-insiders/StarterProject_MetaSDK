using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoseControlledExplodedView : MonoBehaviour
{
    // Reference to the HandPoseTracker for accessing the index finger pose
    private HandPoseTracker handPoseTracker;

    // Reference to the ExplodedViewController
    private ExplodedViewController explodedViewController;

    // Stores the distance value between the two index fingers
    private float indexFingerDistance;

    private void Start()
    {
        // Find the HandPoseTracker component in the scene
        handPoseTracker = FindObjectOfType<HandPoseTracker>();

        // Find the ExplodedViewController component in the scene
        explodedViewController = FindObjectOfType<ExplodedViewController>();
    }

    private void Update()
    {
        // Check if both left and right hand poses are available
        if (!handPoseTracker.leftHandPose || !handPoseTracker.rightHandPose)
        {
            return;
        }

        Debug.Log(" both poses are fetected");
        // Calculate the distance between the left and right index finger positions
        indexFingerDistance = Vector3.Distance(handPoseTracker.LeftIndexPose.position, handPoseTracker.RightIndexPose.position);

        // Trigger the explosion view action in the ExplodedViewController
        explodedViewController.ExplosionViewAction(indexFingerDistance);
    }
}
