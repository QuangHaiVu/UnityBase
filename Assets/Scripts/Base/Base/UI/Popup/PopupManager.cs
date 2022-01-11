using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MoreMountains.Tools;
using UnityEngine;
using UnityEngine.UI;

namespace TheLegends.Unity.Base
{
    public class PopupManager : PersistentSingleton<PopupManager>
    {
        [SerializeField] private Transform canvas;
        public List<PopupController> listCurrentPopUp = new List<PopupController>();

        public T ShowPopUp<T>(string popUpName) where T : PopupController
        {
            if (IsPopupExisted(popUpName)) return null;
            
            T popUp = GameObject.Instantiate(Resources.Load<GameObject>("PopUp/" + popUpName), canvas)
                .GetComponentInChildren<T>();
            popUp.name = popUpName;
            popUp.Show();
            listCurrentPopUp.Add(popUp);


            if (listCurrentPopUp.Count > 1)
            {
                var previousPopup = listCurrentPopUp[listCurrentPopUp.Count - 2];
                var previousPopupBG = previousPopup.transform.parent.GetComponent<Image>();
                previousPopupBG.enabled = false;
            }


            return popUp;
        }

        public void ClosePopUp(PopupController popUp)
        {
            if (listCurrentPopUp.Contains(popUp))
            {
                GameObject.Destroy(popUp.transform.parent.gameObject);
                listCurrentPopUp.Remove(popUp);
            }

            if (listCurrentPopUp.Count >= 1)
            {
                var lastPopup = listCurrentPopUp[listCurrentPopUp.Count - 1];
                var lastPopupBG = lastPopup.transform.parent.GetComponent<Image>();
                lastPopupBG.enabled = true;
            }
        }

        public void CloseAllPopUp()
        {
            foreach (var popUp in listCurrentPopUp)
            {
                GameObject.Destroy(popUp.transform.parent.gameObject);
            }

            listCurrentPopUp.Clear();
        }

        public PopupController GetPopup(string popupName)
        {
            return listCurrentPopUp.FirstOrDefault(_ => _.name.Equals(popupName));
        }

        public bool IsPopupExisted(string popupName)
        {
            return (GetPopup(popupName) != null);
        }
    }

    public class PopUpName
    {
        public const string PopupSelectLevel = "PopupSelectLevel";
        public const string PopupWin = "PopupWin";
        public const string PopupSelectSkin = "PopupSelectSkin";
        public const string PopupLose = "PopupLose";
        public const string PopupSetting = "PopupSetting";
        public const string PopupPause = "PopupPause";
        public const string PopupContinue = "PopupContinue";
        public const string PopupAdsUnavailable = "PopupAdsUnavailable";
        public const string PopupDailyReward = "PopupDailyReward";
        public const string PopupRewardClaim = "PopupRewardClaim";
        public const string PopupShopSkin = "PopupShopSkin";
        public const string PopupSpin = "PopupSpin";
        public const string PopupShopIAP = "PopupShopIAP";
        public const string PopupRate = "PopupRate";
    }
}
