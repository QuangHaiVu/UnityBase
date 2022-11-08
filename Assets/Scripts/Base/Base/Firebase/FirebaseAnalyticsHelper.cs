using System;
using System.Collections.Generic;
using System.Linq;
using Firebase.Analytics;
using UnityEngine;

namespace TheLegends.Unity.Base
{
    public static class FirebaseAnalyticsHelper
    {
        public static void EnableCollectData(bool isEnable)
        {
            Debug.Log("[Firebase] Enabling data collection.");
            FirebaseAnalytics.SetAnalyticsCollectionEnabled(isEnable);
        }
        public static void LogEvent(string eventName, Dictionary<string, object> dictionary = null)
        {
            if (!FirebaseController.Instance.IsFirebaseInitialized()) return;

            if (dictionary != null)
            {
                var param = dictionary.Select(x =>
                {
                    if (x.Key != null && x.Value != null)
                    {
                        if (x.Value is float)
                            return new Parameter(x.Key, (float)x.Value);
                        else if (x.Value is double)
                            return new Parameter(x.Key, (double)x.Value);
                        else if (x.Value is long)
                            return new Parameter(x.Key, (long)x.Value);
                        else if (x.Value is int)
                            return new Parameter(x.Key, (int)x.Value);
                        else if (x.Value is string)
                            return new Parameter(x.Key, x.Value.ToString());
                        else
                            return new Parameter(x.Key, x.Value.ToString());
                    }
                    return null;
                }).ToArray();

                if (param != null)
                    FirebaseAnalytics.LogEvent(eventName, param);
                else
                    FirebaseAnalytics.LogEvent(eventName);
            }
            else
            {
                FirebaseAnalytics.LogEvent(eventName);
            }

            // Debug.Log("[Firebase] LogEvent: " + eventName);
            LogDebug(eventName, dictionary);
        }
        private static bool ValidateInput(string eventName)
        {
            if (string.IsNullOrEmpty(eventName) ||
                Application.internetReachability == NetworkReachability.NotReachable) return false;
            return true;
        }
        private static void LogDebug(string eventName, Dictionary<string, object> dictionary = null)
        {
            var logString = $"[Firebase] LogEvent: {eventName}";
            
            // Debug.Log("[Firebase] LogEvent: " + eventName);
            if (dictionary != null)
            {
                foreach (var pa in dictionary)
                {
                    logString = logString.Insert(logString.Length,
                        $"\nLogParamName: {pa.Key} ====== LogParamValue: {pa.Value}");
                }
            }
            
            Debug.Log(logString);
        }
    }
}