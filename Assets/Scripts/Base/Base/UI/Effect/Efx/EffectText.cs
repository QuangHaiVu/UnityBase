using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EffectText : MonoBehaviour, IEffect
{
    public Text text;
    public string textShow;
    public string textHide;

    public void ShowEffect(Action showComplete = null)
    {
        text.text = textShow;
    }

    public void HideEffect(Action endComplete = null)
    {
        text.text = textHide;
    }

    public void Disable()
    {
        throw new NotImplementedException();
    }
}
