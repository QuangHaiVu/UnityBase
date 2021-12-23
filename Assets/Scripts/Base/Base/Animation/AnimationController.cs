using UnityEngine;

public abstract class AnimationController : MonoBehaviour
{
    public abstract void PlayAnim(string animName);
    public abstract void PlayAnim(string animName, bool isLoop);
    public abstract void ChangeSkin(string skinName);
}