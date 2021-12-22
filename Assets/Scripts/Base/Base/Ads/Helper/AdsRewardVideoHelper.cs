using System;
using System.Collections;
using System.Collections.Generic;
using TheLegends.Unity.Base;
using UnityEngine;

public class AdsRewardVideoHelper : MonoBehaviour
{
    private static string REWARDED_INSTANCE_ID = "0";
    private Action onRewardComplete = null;
    private bool isRewardComplete = false;

    public void init()
    {
        IronSourceEvents.onRewardedVideoAdOpenedEvent += RewardedVideoAdOpenedEvent;
        IronSourceEvents.onRewardedVideoAdClosedEvent += RewardedVideoAdClosedEvent;
        IronSourceEvents.onRewardedVideoAvailabilityChangedEvent += RewardedVideoAvailabilityChangedEvent;
        IronSourceEvents.onRewardedVideoAdStartedEvent += RewardedVideoAdStartedEvent;
        IronSourceEvents.onRewardedVideoAdEndedEvent += RewardedVideoAdEndedEvent;
        IronSourceEvents.onRewardedVideoAdRewardedEvent += RewardedVideoAdRewardedEvent;
        IronSourceEvents.onRewardedVideoAdShowFailedEvent += RewardedVideoAdShowFailedEvent;
        IronSourceEvents.onRewardedVideoAdClickedEvent += RewardedVideoAdClickedEvent;

        // LoadRewardedVideo();

        Debug.Log("unity-script: AdsRewardVideoHelper initialized");
    }

    public void LoadRewardedVideo()
    {
        IronSource.Agent.loadISDemandOnlyRewardedVideo(REWARDED_INSTANCE_ID);
    }

    public void ShowRewardedVideo(Action onComplete)
    {
        this.onRewardComplete = onComplete;

        Debug.Log("unity-script: ShowRewardedVideoButtonClicked");
        if (IsRewardAvailable())
        {
#if UNITY_EDITOR
            onRewardComplete?.Invoke();
#else
            IronSource.Agent.showRewardedVideo();
#endif
        }
        else
        {
            Debug.Log("unity-script: IronSource.Agent.isRewardedVideoAvailable - False");
            PopupManager.Instance.ShowPopUp<PopupController>(PopUpName.PopupAdsUnavailable);
            // LoadRewardedVideo();
        }
    }

    private bool IsRewardAvailable()
    {
#if UNITY_EDITOR
        return true;
#endif
        Debug.Log("IsRewardedAvailable:" + IronSource.Agent.isRewardedVideoAvailable());
        return IronSource.Agent.isRewardedVideoAvailable();
    }

    #region REWARD VIDEO DELEGATES

    void RewardedVideoAvailabilityChangedEvent(bool canShowAd)
    {
        Debug.Log("unity-script: I got RewardedVideoAvailabilityChangedEvent, value = " + canShowAd);
    }

    void RewardedVideoAdOpenedEvent()
    {
        Debug.Log("unity-script: I got RewardedVideoAdOpenedEvent");
        isRewardComplete = false;
    }

    void RewardedVideoAdRewardedEvent(IronSourcePlacement ssp)
    {
        Debug.Log("unity-script: I got RewardedVideoAdRewardedEvent, amount = " + ssp.getRewardAmount() + " name = " +
                  ssp.getRewardName());
        isRewardComplete = true;
    }

    void RewardedVideoAdClosedEvent()
    {
        Debug.Log("unity-script: I got RewardedVideoAdClosedEvent");
        if (isRewardComplete)
        {
            onRewardComplete?.Invoke();
        }

        onRewardComplete = null;
        // LoadRewardedVideo();
    }

    void RewardedVideoAdStartedEvent()
    {
        Debug.Log("unity-script: I got RewardedVideoAdStartedEvent");
    }

    void RewardedVideoAdEndedEvent()
    {
        Debug.Log("unity-script: I got RewardedVideoAdEndedEvent");
    }

    void RewardedVideoAdShowFailedEvent(IronSourceError error)
    {
        Debug.Log("unity-script: I got RewardedVideoAdShowFailedEvent, code :  " + error.getCode() +
                  ", description : " + error.getDescription());
    }

    void RewardedVideoAdClickedEvent(IronSourcePlacement ssp)
    {
        Debug.Log("unity-script: I got RewardedVideoAdClickedEvent, name = " + ssp.getRewardName());
    }

    #endregion
}