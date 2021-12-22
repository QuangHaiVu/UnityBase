using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PopupController : MonoBehaviour
{
    #region variable
    
    [SerializeField] protected ButtonController btnExit;
    [SerializeField] protected UnityEvent onShowComplete;
    [SerializeField] protected UnityEvent onHideComplete;

    private EffectManager effectManager;
    
    #endregion
    
    #region Property

    public UnityEvent OnShowComplete
    {
        get => onShowComplete;
        set => onShowComplete = value;
    }

    public UnityEvent OnHideComplete
    {
        get => onHideComplete;
        set => onHideComplete = value;
    }

    #endregion
    
    private void Awake()
    {
        effectManager = GetComponent<EffectManager>();
        
        if (btnExit != null)
        {
            btnExit.OnClick.AddListener(() =>
            {
                Close();
            });
        }
    }

    private void OnDestroy()
    {
        btnExit?.OnClick.RemoveAllListeners();
    }

    public virtual void Show()
    {
        if (effectManager != null)
        {
            effectManager.ShowEffect(() =>
            {
                onShowComplete?.Invoke();
            });
        }
        else
        {
            onShowComplete?.Invoke();
        }
    }

    public virtual void Close()
    {
        if (effectManager != null)
        {
            effectManager.HideEffect(() =>
            {
                onHideComplete?.Invoke();
                TheLegends.Unity.Base.PopupManager.Instance.ClosePopUp(this);
            });
        }
        else
        {
            onHideComplete?.Invoke();
            TheLegends.Unity.Base.PopupManager.Instance.ClosePopUp(this);
        }
    }
}
