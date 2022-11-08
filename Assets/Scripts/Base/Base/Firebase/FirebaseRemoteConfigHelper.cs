using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Firebase.Extensions;
using Firebase.RemoteConfig;
using UnityEngine;
using UnityEngine.EventSystems;
using static Firebase.RemoteConfig.FirebaseRemoteConfig;

namespace TheLegends.Unity.Base
{
    public static class FirebaseRemoteConfigHelper
    {
        public static T GetFirebaseRemoteConfigValue<T>(string key)
        {
            var type = typeof(T);
            object value = null;

            if (type == typeof(bool))
            {
                if (IsExistKey(key))
                {
                    value = DefaultInstance.GetValue(key).BooleanValue;
                }
                else
                {
                    value = false;
                }
            }

            if (type == typeof(string))
            {
                if (IsExistKey(key))
                {
                    value = DefaultInstance.GetValue(key).StringValue;
                }
                else
                {
                    value = "";
                }
            }

            if (type == typeof(byte))
            {
                if (IsExistKey(key))
                {
                    value = DefaultInstance.GetValue(key).ByteArrayValue;
                }
                else
                {
                    value = null;
                }
            }

            if (type == typeof(int))
            {
                if (IsExistKey(key))
                {
                    value = (int)DefaultInstance.GetValue(key).DoubleValue;
                }
                else
                {
                    value = int.MinValue;
                }
            }

            if (type == typeof(float))
            {
                if (IsExistKey(key))
                {
                    value = (float)DefaultInstance.GetValue(key).DoubleValue;
                }
                else
                {
                    value = float.MinValue;
                }
            }

            return (T)value;
        }

        public static void Init(Dictionary<string, object> remoteDefaultConfig = null)
        {
            if (remoteDefaultConfig != null && remoteDefaultConfig.Count > 0)
            {
                DefaultInstance.SetDefaultsAsync(remoteDefaultConfig).ContinueWithOnMainThread((task) =>
                {
                    Debug.Log("[Firebase] RemoteConfig SetDefaultsAsync");
                });
            }
        }

        public static void FetchDataAsync(Action onComplete)
        {
            Debug.Log("[Firebase] Fetching data...");
            DefaultInstance.FetchAsync(TimeSpan.FromHours(GameConfig.FIREBASE_REMOTE_TIMESPAN))
                .ContinueWithOnMainThread((task) => { FetchComplete(task, onComplete); });
        }

        private static void FetchComplete(Task fetchTask, Action onComplete)
        {
            if (fetchTask.IsCanceled)
            {
                Debug.Log("[Firebase] Fetch canceled.");
            }
            else if (fetchTask.IsFaulted)
            {
                Debug.Log("[Firebase] Fetch encountered an error.");
            }
            else if (fetchTask.IsCompleted)
            {
                Debug.Log("[Firebase] Fetch completed successfully!");
            }

            var info = DefaultInstance.Info;
            switch (info.LastFetchStatus)
            {
                case LastFetchStatus.Success:
                    DefaultInstance.ActivateAsync()
                        .ContinueWithOnMainThread(task =>
                        {
                            Debug.Log(String.Format("[Firebase] Remote data loaded and ready (last fetch time {0}).",
                                info.FetchTime));
                            onComplete?.Invoke();
                        });

                    break;
                case LastFetchStatus.Failure:
                    switch (info.LastFetchFailureReason)
                    {
                        case FetchFailureReason.Error:
                            Debug.Log("[Firebase] Fetch failed for unknown reason");
                            break;
                        case FetchFailureReason.Throttled:
                            Debug.Log("[Firebase] Fetch throttled until " + info.ThrottledEndTime);
                            break;
                    }

                    break;
                case LastFetchStatus.Pending:
                    Debug.Log("[Firebase] Latest Fetch call still pending.");
                    break;
            }
        }

        private static bool IsExistKey(string key)
        {
            var isExist = DefaultInstance.Keys != null &&
                          DefaultInstance.Keys.Contains(key);
            if (!isExist)
            {
                Debug.Log($"[Firebase] Cant find key {key} in Remote Config");
            }

            return DefaultInstance.Keys != null &&
                   DefaultInstance.Keys.Contains(key);
        }
    }
}