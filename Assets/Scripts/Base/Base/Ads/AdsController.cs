using UnityEngine;
using System;
using EasyMobile;

public class AdsController : PersistentSingleton<AdsController>
{
    [SerializeField] private BannerAdPosition bannerPosition;
    [SerializeField] private float timeToShowAds = 10f;
    public float TimeToShowAds
    {
        get => timeToShowAds;
        set => timeToShowAds = value;
    }

    private static DateTime lastTimeShowAd = DateTime.Now;

    public static Action OnInterShow = null;
    public static Action OnRewardShow = null;

    private Action onInterstitialComplete = null;
    private Action onInterstitialDefaultAction = null;
    private Action onRewardComplete = null;
    private Action onRewardSkip = null;

    private void OnEnable()
    {
        Advertising.InterstitialAdCompleted += OnInterstitialAdCompleted;
        Advertising.RewardedAdCompleted += OnRewardedAdCompleted;
        Advertising.RewardedAdSkipped += OnRewardedAdSkipped;
    }

    void OnDisable()
    {
        Advertising.InterstitialAdCompleted -= OnInterstitialAdCompleted;
        Advertising.RewardedAdCompleted -= OnRewardedAdCompleted;
        Advertising.RewardedAdSkipped -= OnRewardedAdSkipped;
    }

    public void OnApplicationPause(bool paused)
    {
        if (paused)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
    }

    private void OnApplicationFocus(bool hasFocus)
    {
        if (hasFocus)
        {
            Time.timeScale = 1;
        }
        else
        {
            Time.timeScale = 0;
        }
    }

    #region Banner

    public void ShowBanner()
    {
        Advertising.ShowBannerAd(bannerPosition, BannerAdSize.Banner);
    }

    public void HideBanner()
    {
        Advertising.HideBannerAd();
    }

    public void DestroyBanner()
    {
        Advertising.DestroyBannerAd();
    }

    #endregion

    #region VideoReward

    public bool IsRewardedAdReady()
    {
        Debug.Log($"unity-script: AdsRewardedVideoAvailable - {Advertising.IsRewardedAdReady()}");
        return Advertising.IsRewardedAdReady();
    }

    public void ShowRewardedVideo(Action onComplete, Action onSkip = null)
    {
        onRewardComplete = onComplete;
        onRewardSkip = onSkip;

#if UNITY_EDITOR
        ShowRewardOnUnityEditor();
#else
        ShowRewardOnMobile();
#endif
    }

    private void ShowRewardOnUnityEditor()
    {
        onRewardComplete?.Invoke();
        onRewardComplete = null;
    }

    private void ShowRewardOnMobile()
    {
        if (IsRewardedAdReady())
        {
            OnRewardShow?.Invoke();
            Advertising.ShowRewardedAd();
        }
        else
        {
            Advertising.LoadRewardedAd();
        }
    }

    private void OnRewardedAdCompleted(RewardedAdNetwork arg1, AdPlacement arg2)
    {
        onRewardComplete?.Invoke();
        onRewardComplete = null;

        onRewardSkip = null;
    }

    private void OnRewardedAdSkipped(RewardedAdNetwork arg1, AdPlacement arg2)
    {
        onRewardComplete = null;

        onRewardSkip?.Invoke();
        onRewardSkip = null;
    }

    #endregion

    #region Interstitial

    public bool IsInterstitialAdReady()
    {
        Debug.Log($"unity-script: AdsInterstitialReady - {Advertising.IsInterstitialAdReady()}");
        return Advertising.IsInterstitialAdReady();
    }

    public void ShowInterstitial(Action onDefault = null, Action onComplete = null)
    {
        onInterstitialDefaultAction = onDefault;
        onInterstitialComplete = onComplete;

#if UNITY_EDITOR
        ShowInterstitialOnUnityEditor();
#else
        ShowInterstitialOnMobile();
#endif
    }

    private void ShowInterstitialOnUnityEditor()
    {
        onInterstitialDefaultAction?.Invoke();
        onInterstitialDefaultAction = null;
        
        onInterstitialComplete?.Invoke();
        onInterstitialComplete = null;
    }

    private void ShowInterstitialOnMobile()
    {
        if (IsInterstitialAdReady() && CanShowAds())
        {
            OnInterShow?.Invoke();
            Advertising.ShowInterstitialAd();
            CalculateTimeAdsShowSuccess();
        }
        else
        {
            Advertising.LoadInterstitialAd();

            onInterstitialDefaultAction?.Invoke();
            onInterstitialDefaultAction = null;
        }
    }

    private void OnInterstitialAdCompleted(InterstitialAdNetwork arg1, AdPlacement arg2)
    {
        onInterstitialDefaultAction?.Invoke();
        onInterstitialDefaultAction = null;

        onInterstitialComplete?.Invoke();
        onInterstitialComplete = null;
    }

    #endregion

    #region Helper

    private void CalculateTimeAdsShowSuccess()
    {
        lastTimeShowAd = DateTime.Now;
    }
    
    private bool CanShowAds()
    {
        var timeLastAds = (float)(DateTime.Now - lastTimeShowAd).TotalSeconds;
        Debug.Log($"Time since last ads: {timeLastAds}. Can show ads: {timeLastAds >= timeToShowAds}");
        return timeLastAds >= timeToShowAds;
    }

    #endregion
}