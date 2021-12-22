using Firebase.Analytics;
using UnityEngine;

namespace TheLegends.Unity.Base
{
    public static class FirebaseAnalyticsHelper
    {
        public static void LogEvent(string eventName, string paramsName, string parameterValue)
        {
            if(!FirebaseController.Instance.IsFirebaseInitialized()) return;
            if(!ValidateInput(eventName)) return;
            FirebaseAnalytics.LogEvent(eventName, paramsName, parameterValue);
        }

        public static void LogEvent(string eventName, string paramsName, double parameterValue)
        {
            if(!FirebaseController.Instance.IsFirebaseInitialized()) return;
            if(!ValidateInput(eventName)) return;
            FirebaseAnalytics.LogEvent(eventName, paramsName, parameterValue);
        }

        public static void LogEvent(string eventName, string paramsName, long parameterValue)
        {
            if(!FirebaseController.Instance.IsFirebaseInitialized()) return;
            if(!ValidateInput(eventName)) return;
            FirebaseAnalytics.LogEvent(eventName, paramsName, parameterValue);
        }

        public static void LogEvent(string eventName, string paramsName, int parameterValue)
        {
            if(!FirebaseController.Instance.IsFirebaseInitialized()) return;
            if(!ValidateInput(eventName)) return;
            FirebaseAnalytics.LogEvent(eventName, paramsName, parameterValue);
        }

        public static void LogEvent(string eventName)
        {
            if(!FirebaseController.Instance.IsFirebaseInitialized()) return;
            if(!ValidateInput(eventName)) return;
            FirebaseAnalytics.LogEvent(eventName);
        }

        public static void LogEvent(string eventName, params Parameter[] parameters)
        {
            if(!FirebaseController.Instance.IsFirebaseInitialized()) return;
            if(!ValidateInput(eventName)) return;
            FirebaseAnalytics.LogEvent(eventName, parameters);
        }

        private static bool ValidateInput(string eventName)
        {
            if (string.IsNullOrEmpty(eventName) || Application.internetReachability != NetworkReachability.NotReachable) return false;
            return true;
        }
    }
}