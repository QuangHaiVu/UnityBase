using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonController : MonoBehaviour, IPointerUpHandler, IPointerDownHandler, IPointerClickHandler
{
    [SerializeField] private UnityEvent onClick;
    protected EffectManager effectManager;
    private bool canClick = true;
    
    public UnityEvent OnClick
    {
        get => onClick;
        set => onClick = value;
    }
    
    protected virtual void Awake()
    {
        effectManager = GetComponent<EffectManager>();
    }

    public virtual void OnPointerUp(PointerEventData eventData)
    {
        if(!canClick) return;
        if(!effectManager) return;
        effectManager.HideEffect();
    }

    public virtual void OnPointerDown(PointerEventData eventData)
    {
        if(!canClick) return;
        if(!effectManager) return;
        effectManager.ShowEffect();
    }

    public virtual void CanClick(bool canClick)
    {
        this.canClick = canClick;
    }

    public virtual void OnPointerClick(PointerEventData eventData)
    {
        onClick?.Invoke();
    }
}