using System.Collections;
using System.Collections.Generic;
using TheLegends.Unity.Base;
using UnityEngine;

public class ButtonOpenPopup : ButtonController
{
    public string popupName = default;
    
    private void Start()
    {
        OnClick.AddListener(() =>
        {
            ShowPopup();
        });
    }
    
    private void ShowPopup()
    {
        AdsController.Instance.ShowInterstitial(null);
        PopupManager.Instance.ShowPopUp<PopupController>(popupName);
    }
}
