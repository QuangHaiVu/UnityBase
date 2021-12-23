using System;
using System.Collections;
using System.Collections.Generic;
using Spine.Unity;
using UnityEngine;
using AnimationState = Spine.AnimationState;

[RequireComponent(typeof(SkeletonAnimation))]
public class SpineAnimationController : AnimationController
{
    private SkeletonAnimation skeletonAnimation;

    private void Awake()
    {
        skeletonAnimation = GetComponent<SkeletonAnimation>();
    }

    public override void PlayAnim(string animName)
    {
        try
        {
            skeletonAnimation.AnimationName = animName;
        }
        catch (Exception e)
        {
            Debug.LogError($"Can't not find {animName} animation");
            return;
        }
    }

    public override void PlayAnim(string animName, bool isLoop)
    {
        skeletonAnimation.loop = isLoop;
        PlayAnim(animName);

        // skeletonAnimation.AnimationState.SetAnimation(0, animName, isLoop);
    }

    public override void ChangeSkin(string skinName)
    {
        try
        {
            skeletonAnimation.Skeleton.SetSkin(skinName);
        }
        catch (Exception e)
        {
            Debug.LogError($"Can't not find {skinName} skin");
            return;
        }
        
        skeletonAnimation.Skeleton.SetToSetupPose();
        skeletonAnimation.AnimationState.Apply(skeletonAnimation.Skeleton);
    }
}