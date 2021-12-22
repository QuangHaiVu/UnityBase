using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EffectImage : MonoBehaviour, IEffect
{
    public Image image;
    public Sprite showSprite;
    public Sprite hideSprite;
    public void ShowEffect(Action showComplete = null)
    {
        image.sprite = showSprite;
    }

    public void HideEffect(Action endComplete = null)
    {
        image.sprite = hideSprite;
    }

    public void Disable()
    {
    }
}
