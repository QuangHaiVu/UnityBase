using System;
using System.Collections;
using UnityEngine;

public class EffectManager : MonoBehaviour
{
    protected IEffect[] effects;

    protected virtual void Awake()
    {
        effects = GetComponents<IEffect>();
    }

    public void ShowEffect(Action onShowComplete = null)
    {
        StartCoroutine(IeShowEffects(onShowComplete));
    }
    
    IEnumerator IeShowEffects(Action onShowComplete = null)
    {
        int countShow = 0;
        foreach (IEffect effect in effects)
        {
            effect.ShowEffect(() =>
            {
                countShow++;
            });
        }
        yield return new WaitUntil(()=> countShow >= effects.Length);
        onShowComplete?.Invoke();
    }

    public void HideEffect(Action onHideComplete = null)
    {
        StartCoroutine(IeHideEffects(onHideComplete));
    }
    
    IEnumerator IeHideEffects(Action onHideComplete = null)
    {
        int countShow = 0;
        foreach (IEffect effect in effects)
        {
            effect.HideEffect(() =>
            {
                countShow++;
            });
        }
        yield return new WaitUntil(()=> countShow >= effects.Length);
        onHideComplete?.Invoke();
    }
}