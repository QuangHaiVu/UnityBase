using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TabButtonController : ButtonController
{
    [SerializeField] protected EffectManager selectEffectManager;
    [SerializeField] protected GameObject content;

    public override void CanClick(bool canClick)
    {
        base.CanClick(canClick);
        if (!canClick)
        {
            SelectEffect();
            content.gameObject.SetActive(true);
        }
        else
        {
            DeSelectEffect();
            content.gameObject.SetActive(false);
        }
    }

    private void SelectEffect()
    {
        selectEffectManager.ShowEffect();
    }

    private void DeSelectEffect()
    {
        selectEffectManager.HideEffect();
    }
}
