using System;
using System.Collections;
using System.Collections.Generic;
using Spine.Unity;
using UnityEngine;

[RequireComponent(typeof(SkeletonMecanim))]
public class SpineMecanimController : AnimationController
{
    private SkeletonMecanim skeletonMecanim;
    private Animator animator;

    private void Awake()
    {
        skeletonMecanim = GetComponent<SkeletonMecanim>();
        animator = GetComponent<Animator>();
    }

    public override void PlayAnim(string animName)
    {
        try
        {
            animator.Play(animName);
        }
        catch (Exception e)
        {
            Debug.LogError($"Can't not find {animName} animation");
            throw;
        }
    }

    public override void PlayAnim(string animName, bool isLoop)
    {
        PlayAnim(animName);
    }

    public override void ChangeSkin(string skinName)
    {
        try
        {
            skeletonMecanim.Skeleton.SetSkin(skinName);
        }
        catch (Exception e)
        {
            Debug.LogError($"Can't not find {skinName} skin");
            return;
        }
        
        skeletonMecanim.Skeleton.SetSlotsToSetupPose();
        skeletonMecanim.Translator.Apply(skeletonMecanim.Skeleton);
    }
}
