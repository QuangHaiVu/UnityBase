using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class EffectScale : MonoBehaviour, IEffect
{
    [SerializeField] private Vector3 startScale = Vector3.one;
    [SerializeField] private Vector3 endScale = Vector3.one;
    [SerializeField] private float timeScale = 1f;

    public void ShowEffect(Action showComplete = null)
    {
        this.transform.localScale = startScale;
        this.transform.DOScale(endScale, timeScale).SetUpdate(true).OnComplete(() => { showComplete?.Invoke(); });
    }

    public void HideEffect(Action endComplete = null)
    {
        this.transform.DOScale(startScale, timeScale).SetUpdate(true).OnComplete(() => { endComplete?.Invoke(); });
    }

    public void Disable()
    {
        this.transform.DOKill();
    }

    private void OnDisable()
    {
        Disable();
    }
}