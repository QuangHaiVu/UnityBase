using System.Collections;
using System.Collections.Generic;
using TheLegends.Unity.Base;
using UnityEngine;

public class DemoFirebaseRemoteConfig : MonoBehaviour
{
    private void Start()
    {
        var defaultRemoteConfig = new Dictionary<string, object>
        {
            // {FirebaseRemoteConfigKey.TIME_TO_SHOW_ADS , FirebaseRemoteConfigKey.TIME_TO_SHOW_ADS_DEFAULT },
        };
        
        FirebaseRemoteConfigHelper.Init(defaultRemoteConfig);
        FirebaseRemoteConfigHelper.FetchDataAsync(() =>
        {
            // AdsController.Instance.TimeToShowAds =
            //     FirebaseRemoteConfigHelper.GetFirebaseRemoteConfigValue<float>(FirebaseRemoteConfigKey
            //         .TIME_TO_SHOW_ADS);
        });
        
    }
}
