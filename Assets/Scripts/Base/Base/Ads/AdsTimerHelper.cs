using System.Collections;
using System.Collections.Generic;
using TheLegends.Unity.Base;
using UnityEngine;

public class AdsTimerHelper : TimerController
{
    public AdsInterstitialHelper inter;
    protected override void Start()
    {
        base.Start();
        OnComplete.AddListener(() =>
        {
            if (!IronSource.Agent.isInterstitialReady())
            {
                inter.LoadInterstitial();
            }
        });
    }
}
