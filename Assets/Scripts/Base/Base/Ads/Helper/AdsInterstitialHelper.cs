using System;
using System.Collections;
using System.Collections.Generic;
using TheLegends.Unity.Base;
using UnityEngine;

public class AdsInterstitialHelper : MonoBehaviour
{
    [SerializeField] private TimerController _timerCoolDown;
    private Action onInterstitialShow = null;

    public void Init()
    {
        IronSourceEvents.onInterstitialAdReadyEvent += InterstitialAdReadyEvent;
        IronSourceEvents.onInterstitialAdLoadFailedEvent += InterstitialAdLoadFailedEvent;
        IronSourceEvents.onInterstitialAdShowSucceededEvent += InterstitialAdShowSucceededEvent;
        IronSourceEvents.onInterstitialAdShowFailedEvent += InterstitialAdShowFailedEvent;
        IronSourceEvents.onInterstitialAdClickedEvent += InterstitialAdClickedEvent;
        IronSourceEvents.onInterstitialAdOpenedEvent += InterstitialAdOpenedEvent;
        IronSourceEvents.onInterstitialAdClosedEvent += InterstitialAdClosedEvent;

        LoadInterstitial();

        Debug.Log("unity-script: Interstitial initialized");
    }


    public void LoadInterstitial()
    {
        IronSource.Agent.loadInterstitial();
    }

    public void ShowInterstitial(Action onShow)
    {
        if (_timerCoolDown.IsActive) return;

        this.onInterstitialShow = onShow;
        Debug.Log("unity-script: ShowInterstitialButtonClicked");
        if (IronSource.Agent.isInterstitialReady())
        {
#if UNITY_EDITOR
            onInterstitialShow?.Invoke();
#else
            IronSource.Agent.showInterstitial();
#endif
            _timerCoolDown.IsActive = true;
        }
        else
        {
            Debug.Log("unity-script: IronSource.Agent.isInterstitialReady - False");
            LoadInterstitial();
        }
    }

    #region Interstitial DELEGATES

    void InterstitialAdReadyEvent()
    {
        Debug.Log("unity-script: I got InterstitialAdReadyEvent");
    }

    void InterstitialAdLoadFailedEvent(IronSourceError error)
    {
        Debug.Log("unity-script: I got InterstitialAdLoadFailedEvent, code: " + error.getCode() + ", description : " +
                  error.getDescription());
    }

    void InterstitialAdShowSucceededEvent()
    {
        Debug.Log("unity-script: I got InterstitialAdShowSucceededEvent");
        onInterstitialShow?.Invoke();
        onInterstitialShow = null;
    }

    void InterstitialAdShowFailedEvent(IronSourceError error)
    {
        Debug.Log("unity-script: I got InterstitialAdShowFailedEvent, code :  " + error.getCode() + ", description : " +
                  error.getDescription());
    }

    void InterstitialAdClickedEvent()
    {
        Debug.Log("unity-script: I got InterstitialAdClickedEvent");
    }

    void InterstitialAdOpenedEvent()
    {
        Debug.Log("unity-script: I got InterstitialAdOpenedEvent");
    }

    void InterstitialAdClosedEvent()
    {
        Debug.Log("unity-script: I got InterstitialAdClosedEvent");
        LoadInterstitial();
    }

    #endregion
}