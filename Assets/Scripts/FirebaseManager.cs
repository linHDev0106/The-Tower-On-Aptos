using System.Collections;
using System.Collections.Generic;
using Firebase;
using Firebase.Analytics;
using UnityEngine;

public class FirebaseManager : MonoBehaviour
{

	DependencyStatus dependencyStatus = DependencyStatus.UnavailableOther;

	public static FirebaseManager instance;

	void Awake()
	{
		if (FindObjectsOfType(typeof(FirebaseManager)).Length > 1)
		{
			Destroy(gameObject);
			return;
		}

		instance = this;

		DontDestroyOnLoad(gameObject);
	}

	// Use this for initialization
	void Start()
	{

		Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => {
			var dependencyStatus = task.Result;
			if (dependencyStatus == Firebase.DependencyStatus.Available)
			{
				// Create and hold a reference to your FirebaseApp,
				// where app is a Firebase.FirebaseApp property of your application class.
				InitializeFirebase();

				// Set a flag here to indicate whether Firebase is ready to use by your app.
			}
			else
			{
				UnityEngine.Debug.LogError(System.String.Format(
				  "Could not resolve all Firebase dependencies: {0}", dependencyStatus));
				// Firebase Unity SDK is not safe to use here.
			}
		});
		
	}

	// Update is called once per frame
	void Update()
	{

	}

	// Handle initialization of the necessary firebase modules:
	void InitializeFirebase()
	{
		//DebugLog("Enabling data collection.");
		FirebaseAnalytics.SetAnalyticsCollectionEnabled(true);

		//DebugLog("Set user properties.");
		// Set the user's sign up method.
		FirebaseAnalytics.SetUserProperty(
			FirebaseAnalytics.UserPropertySignUpMethod,
			"Google");
		// Set the user ID.
		FirebaseAnalytics.SetUserId("uber_user_510");
	}

	public void LogScreen(string _log)
	{
		FirebaseAnalytics.LogEvent(_log);
	}

}
