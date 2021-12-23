using System;
using System.Collections;
using System.Collections.Generic;
using Spine.Unity;
using UnityEngine;

[RequireComponent(typeof(SkeletonGraphic))]
public class SpineGraphicController : AnimationController
{
    private SkeletonGraphic skeletonGraphic;

    private void Awake()
    {
        skeletonGraphic = GetComponent<SkeletonGraphic>();
    }

    public override void PlayAnim(string animName)
    {
        try
        {
            skeletonGraphic.startingAnimation = animName;
            skeletonGraphic.Initialize(true);
        }
        catch (Exception e)
        {
            Debug.LogError($"Can't not find {animName} animation");
            return;
        }
        
    }

    public override void PlayAnim(string animName, bool isLoop)
    {
        skeletonGraphic.startingLoop = isLoop;
        PlayAnim(animName);
        // skeletonGraphic.AnimationState.SetAnimation(0, animName, isLoop);
    }

    public override void ChangeSkin(string skinName)
    {
        try
        {
            skeletonGraphic.Skeleton.SetSkin(skinName);
        }
        catch (Exception e)
        {
            Debug.LogError($"Can't not find {skinName} skin");
            return;
        }
        
        skeletonGraphic.Skeleton.SetToSetupPose();
        skeletonGraphic.AnimationState.Apply(skeletonGraphic.Skeleton);
    }
}