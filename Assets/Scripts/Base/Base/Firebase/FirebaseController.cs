using System.Collections;
using Firebase;
using MoreMountains.Tools;
using UnityEngine;

namespace TheLegends.Unity.Base
{
    public class FirebaseController : MMPersistentSingleton<FirebaseController>
    {
        private DependencyStatus dependencyStatus = DependencyStatus.UnavailableOther;
        private bool firebaseInitialized = false;
        
        void Start()
        {
            StartCoroutine(InitializeFirebase());
        }

        public IEnumerator InitializeFirebase(float timeOut = 3f)
        {
            var elapsedTime = 0f;

            FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
            {
                dependencyStatus = task.Result;
                if (dependencyStatus == DependencyStatus.Available)
                {
                    Debug.Log("[Firebase] CheckDependencies: " + task.Result);
                }
                else
                {
                    Debug.LogError(
                        "[Firebase] Could not resolve all Firebase dependencies: " + dependencyStatus);
                    Debug.Log("[Firebase] CheckDependencies: " + task.Result);
                }
            });

            while (dependencyStatus != DependencyStatus.Available && elapsedTime < timeOut)
            {
                elapsedTime += Time.deltaTime;
                yield return new WaitForSeconds(1);
            }

            if (dependencyStatus == DependencyStatus.Available)
            {
                InitializeFirebaseDone();
            }
        }

        private void InitializeFirebaseDone()
        {
            firebaseInitialized = true;
            FirebaseAnalyticsHelper.EnableCollectData(true);
            FirebaseRemoteConfigHelper.Init();
        }

        public bool IsFirebaseInitialized()
        {
            return firebaseInitialized;
        }
    }
}