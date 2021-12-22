using System;
using UnityEngine;

public class AdsController : PersistentSingleton<AdsController>
{
    [SerializeField] private AdsRewardVideoHelper reward;
    [SerializeField] private AdsInterstitialHelper interstitial;
    [SerializeField] private AdsBannerHelper banner;

    [SerializeField] private bool isUsingAds;

    private void Start()
    {
        if(!isUsingAds) return;
        InitService();
    }

    private void InitService()
    {
        Debug.Log("MyAppStart Start called");

        //Dynamic config example
        IronSourceConfig.Instance.setClientSideCallbacks(true);

        string id = IronSource.Agent.getAdvertiserId();
        Debug.Log("IronSource.Agent.getAdvertiserId : " + id);

        Debug.Log("IronSource.Agent.validateIntegration");
        IronSource.Agent.validateIntegration();

        Debug.Log("unity version" + IronSource.unityVersion());


        var developerSettings =
            Resources.Load<IronSourceMediationSettings>(IronSourceConstants.IRONSOURCE_MEDIATION_SETTING_NAME);
        if (developerSettings != null)
        {
#if UNITY_ANDROID
            string appKey = developerSettings.AndroidAppKey;
#elif UNITY_IOS
			string appKey = developerSettings.IOSAppKey;
#endif
            Debug.Log("App key :" + appKey);

            if (appKey.Equals(string.Empty))
            {
                Debug.LogWarning("IronSourceInitilizer Cannot init without AppKey");
            }
            else
            {
                IronSource.Agent.init(appKey);
                InitHelper();
            }
        }
    }

    private void InitHelper()
    {
        reward.init();
        interstitial.Init();
        banner.Init();
    }

    void OnApplicationPause(bool isPaused)
    {
        IronSource.Agent.onApplicationPause(isPaused);
    }

    #region PUBLIC METHOD

    public void ShowReward(Action onComplete)
    {
        if (!isUsingAds)
        {
            onComplete?.Invoke();
            return;
        }
        if (!reward)
        {
            Debug.LogError("Reward not assigned");
            return;
        }

        reward.ShowRewardedVideo(onComplete);
    }

    public void ShowInterstitial(Action onShow)
    {
        if (!isUsingAds)
        {
            onShow?.Invoke();
            return;
        }
        if (!interstitial)
        {
            Debug.LogError("Interstitial not assigned");
            return;
        }

        interstitial.ShowInterstitial(onShow);
    }

    public void ShowBanner()
    {
        if(!isUsingAds) return;
        if (!banner)
        {
            Debug.LogError("Banner not assigned");
            return;
        }

        banner.ShowBanner();
    }

    public void HideBanner()
    {
        if(!isUsingAds) return;
        if (!banner)
        {
            Debug.LogError("Banner not assigned");
            return;
        }

        banner.HideBanner();
    }

    #endregion
}