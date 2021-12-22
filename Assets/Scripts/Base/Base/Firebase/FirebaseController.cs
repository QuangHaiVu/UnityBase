using Firebase;
using Firebase.Analytics;
using UnityEngine;

public class FirebaseController : PersistentSingleton<FirebaseController>
{
    private DependencyStatus dependencyStatus = DependencyStatus.UnavailableOther;
    private bool firebaseInitialized = false;
    
    void Start()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            dependencyStatus = task.Result;
            if (dependencyStatus == DependencyStatus.Available)
            {
                InitializeFirebase();
            }
            else
            {
                Debug.LogError(
                    "Could not resolve all Firebase dependencies: " + dependencyStatus);
            }
        });
    }

    private void InitializeFirebase()
    {
        Debug.Log("Enabling data collection.");
        FirebaseAnalytics.SetAnalyticsCollectionEnabled(true);
        firebaseInitialized = true;
    }

    public bool IsFirebaseInitialized()
    {
        if (firebaseInitialized)
        {
            return true;
        }
        else
        {
            InitializeFirebase();
            return false;
        }
    }
}