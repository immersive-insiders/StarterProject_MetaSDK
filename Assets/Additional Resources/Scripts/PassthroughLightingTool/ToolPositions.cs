using Oculus.Interaction.Input;
using Oculus.Interaction;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolPositions : MonoBehaviour
{
    [SerializeField, Interface(typeof(IHand))]
    private Object _leftHand;

    [SerializeField, Interface(typeof(IHand))]
    private Object _rightHand;

    [SerializeField]
    private Vector3 _leftAnchorPoint = new Vector3(-0.0608603321f, 0.00953984447f, 0.000258127693f);

    [SerializeField]
    private Vector3 _leftAimPoint = new Vector3(-0.0749258399f, 0.0893092677f, 0.000258127693f);

    [SerializeField]
    private Vector3 _rightAnchorPoint = new Vector3(0.0652603358f, -0.011439844f, -0.00455812784f);

    [SerializeField]
    private Vector3 _rightAimPoint = new Vector3(0.0793258473f, -0.0912092775f, -0.00455812784f);

    private IHand LeftHand { get; set; }
    private IHand RightHand { get; set; }

    protected virtual void Awake()
    {
        LeftHand = _leftHand as IHand;
        RightHand = _rightHand as IHand;
    }

    private void OnEnable()
    {
        var anchor = LeftHand.IsDominantHand ? _rightAnchorPoint : _leftAnchorPoint;
        var aim = LeftHand.IsDominantHand ? _rightAimPoint : _leftAimPoint;
        var hand = LeftHand.IsDominantHand ? RightHand : LeftHand;
        Pose wristPose;
        if (hand.GetJointPose(HandJointId.HandWristRoot, out wristPose))
        {
            var anchorPose = new Pose(anchor, Quaternion.identity).GetTransformedBy(wristPose);
            var aimPose = new Pose(aim, Quaternion.identity).GetTransformedBy(wristPose);
            this.transform.SetPositionAndRotation(anchorPose.position, Quaternion.LookRotation((anchorPose.position - aimPose.position).normalized));
        }
    }
}
