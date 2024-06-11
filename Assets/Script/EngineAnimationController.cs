using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EngineAnimationController : MonoBehaviour
{
    [SerializeField] private Animator engineAnimator;

    private bool isAnimationPlaying = false;

    public void ToggleAnimation()
    {
        if (isAnimationPlaying)
        {
            engineAnimator.SetBool("ToggleAnimation", false);
            isAnimationPlaying = false;
        }
        else
        {
            engineAnimator.SetBool("ToggleAnimation", true);
            isAnimationPlaying = true;
        }
    }
}
