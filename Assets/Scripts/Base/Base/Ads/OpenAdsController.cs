using System;
using System.Collections;
using System.Collections.Generic;
using EasyMobile;
using GoogleMobileAds.Api;
using UnityEngine;

public class OpenAdsController : MonoBehaviour
{
    [SerializeField] private string androidOpenAdsID = null;
    [SerializeField] private string iOSOpenAdsID = null;

    private AppOpenAd ad;

    private bool isShowingAppOpenAd = false;

    private bool IsAppOpenAdAvailable
    {
        get { return ad != null; }
    }

    private bool isPausedByAds;

    private void OnEnable()
    {
        AdsController.OnInterShow += OnInterShowed;
        AdsController.OnRewardShow += OnRewardShowed;
    }
    
    private void OnDisable()
    {
        AdsController.OnInterShow -= OnInterShowed;
        AdsController.OnRewardShow -= OnRewardShowed;
    }

    private void Start()
    {
        LoadAd();
        StartCoroutine(ShowFirstOpen());
    }
    
    #region EventListen
    
    private void OnInterShowed()
    {
        isPausedByAds = true;
    }

    private void OnRewardShowed()
    {
        isPausedByAds = true;
    }
    
    #endregion
    
    
    #region AppOpenAds

    private IEnumerator ShowFirstOpen()
    {
        yield return new WaitForSeconds(2f);

        float time = 0f;
        while (time < 2 && ad == null)
        {
            yield return new WaitForSeconds(0.2f);
            time += 0.2f;
        }

        ShowAppOpenAd();
    }

    public void LoadAd()
    {
#if UNITY_ANDROID
        string AD_UNIT_ID = androidOpenAdsID;
#elif UNITY_IOS
        string AD_UNIT_ID = iOSOpenAdsID;
#else
        string AD_UNIT_ID = "unexpected_platform";
#endif
        
        AdRequest request = new AdRequest.Builder().Build();

        // Load an app open ad for portrait orientation
        AppOpenAd.LoadAd(AD_UNIT_ID, ScreenOrientation.LandscapeLeft, request, ((appOpenAd, error) =>
        {
            if (error != null)
            {
                // Handle the error.
                Debug.LogError($"Failed to load the ad. (reason: {error.LoadAdError.GetMessage()})");
                return;
            }

            // App open ad is loaded.
            Debug.Log("Open Ads Load Success");
            ad = appOpenAd;
        }));
    }

    public void ShowAppOpenAd()
    {
        if (!IsAppOpenAdAvailable || isShowingAppOpenAd)
        {
            return;
        }

        ad.OnAdDidDismissFullScreenContent += HandleAdDidDismissFullScreenContent;
        ad.OnAdFailedToPresentFullScreenContent += HandleAdFailedToPresentFullScreenContent;
        ad.OnAdDidPresentFullScreenContent += HandleAdDidPresentFullScreenContent;
        ad.OnAdDidRecordImpression += HandleAdDidRecordImpression;
        ad.OnPaidEvent += HandlePaidEvent;

        Debug.Log("Call show Open Ads");
        ad.Show();
    }

    private void HandleAdDidDismissFullScreenContent(object sender, EventArgs args)
    {
        Debug.Log("Closed app open ad");
        // Set the ad to null to indicate that AppOpenAdManager no longer has another ad to show.
        ad = null;
        isShowingAppOpenAd = false;
        LoadAd();
    }

    private void HandleAdFailedToPresentFullScreenContent(object sender, AdErrorEventArgs args)
    {
        Debug.LogFormat("Failed to present the ad (reason: {0})", args.AdError.GetMessage());
        // Set the ad to null to indicate that AppOpenAdManager no longer has another ad to show.
        ad = null;
        isShowingAppOpenAd = false;
        LoadAd();
    }

    private void HandleAdDidPresentFullScreenContent(object sender, EventArgs args)
    {
        Debug.Log("Displayed app open ad");
        isShowingAppOpenAd = true;
    }

    private void HandleAdDidRecordImpression(object sender, EventArgs args)
    {
        Debug.Log("Recorded ad impression");
    }

    private void HandlePaidEvent(object sender, AdValueEventArgs args)
    {
        Debug.LogFormat("Received paid event. (currency: {0}, value: {1}",
            args.AdValue.CurrencyCode, args.AdValue.Value);
    }

    public void OnApplicationPause(bool paused)
    {
        Debug.Log("App Pause " + paused);

        if (!paused && !isPausedByAds)
        {
            ShowAppOpenAd();
        }

        if (!paused && isPausedByAds)
        {
            isPausedByAds = false;
        }
    }

    #endregion
}