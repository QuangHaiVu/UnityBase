using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class UnityAnimationController : AnimationController
{
    private Animator animator;

    [SerializeField] private RuntimeAnimatorController[] animators;

    private void Awake()
    {
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
        var selectedAnimator = animators.FirstOrDefault(_ => _.name.Equals(skinName));
        if (!selectedAnimator)
        {
            Debug.LogError($"Can't not find {skinName} skin");
            return;
        }

        animator.runtimeAnimatorController = selectedAnimator;
    }
}
