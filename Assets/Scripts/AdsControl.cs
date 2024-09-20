using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.SocialPlatforms;
using GoogleMobileAds.Api;
using System;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using GoogleMobileAds.Common;
using UnityEngine.Advertisements;
/*
 * 
 * Document for Unity Ads : https://docs.unity.com/ads/ImplementingBasicAdsUnity.html
 */
/*
 * 
 * Document for Google Admob : https://developers.google.com/admob/unity/quick-start
 */

public class AdsControl : MonoBehaviour,IUnityAdsInitializationListener, IUnityAdsLoadListener, IUnityAdsShowListener
{

    private static AdsControl instance;

    //for Admob

    public string Android_AppID, IOS_AppID;

    public string Android_Banner_Key, IOS_Banner_Key;

    public string Android_Interestital_Key, IOS_Interestital_Key;

    public string Android_RW_Key, IOS_RW_Key;



    private AppOpenAd appOpenAd;
    private BannerView bannerView;
    private InterstitialAd interstitialAd;
    private RewardedAd rewardedAd;
    private RewardedInterstitialAd rewardedInterstitialAd;
    private bool isShowingAppOpenAd;

    public UnityEvent OnAdLoadedEvent;
    public UnityEvent OnAdFailedToLoadEvent;
    public UnityEvent OnAdOpeningEvent;
    public UnityEvent OnAdFailedToShowEvent;
    public UnityEvent OnUserEarnedRewardEvent;
    public UnityEvent OnAdClosedEvent;

    //for Unity Ads

    [SerializeField] string androidUnityGameId;
    [SerializeField] string iOSUnityGameId;
    [SerializeField] string androidUnityAdUnitId;
    [SerializeField] string iOsUnityAdUnitId;
    string adUnityUnitId;
    [SerializeField] string androidUnityRWAdUnitId;
    [SerializeField] string iOSUnityRWAdUnitId;
    string adUnityRWUnitId = null; // This will remain null for unsupported platforms
    [SerializeField] bool testMode;
    private string unityGameId;


    public enum REWARD_TYPE
    {
        GEMS_REWARD,
        X2GOLD_GAME_OVER,
        X2GOLD_CHEST,
        X4_COIN,
        RETRIVE
       
    }

    public REWARD_TYPE currentRWType;


    public enum ADS_TYPE
    {
        ADMOB,
        UNITY,
        MEDIATION
    }

    public ADS_TYPE currentAdsType;

    public static AdsControl Instance { get { return instance; } }

    void Awake()
    {
        if (FindObjectsOfType(typeof(AdsControl)).Length > 1)
        {
            Destroy(gameObject);
            return;
        }


        instance = this;
        DontDestroyOnLoad(gameObject);
    }





    
    private void Start()
    {
        //if (!CanShowAds())
        //     return;

        // Get the Ad Unit ID for the current platform:
        adUnityUnitId = (Application.platform == RuntimePlatform.IPhonePlayer)
            ? iOsUnityAdUnitId
            : androidUnityAdUnitId;

#if UNITY_IOS
        adUnityRWUnitId = iOSUnityRWAdUnitId;
#elif UNITY_ANDROID
        adUnityRWUnitId = androidUnityRWAdUnitId;
#endif

        MobileAds.SetiOSAppPauseOnBackground(true);
        // Initialize the Google Mobile Ads SDK.
        MobileAds.Initialize(HandleInitCompleteAction);

        InitializeUnityAds();
    }

    private void Update()
    {
        
    }

    public void InitializeUnityAds()
    {
#if UNITY_IOS
        unityGameId = iOSUnityGameId;
#elif UNITY_ANDROID
            unityGameId  = androidUnityGameId;
#elif UNITY_EDITOR
            unityGameId = androidUnityGameId; //Only for testing the functionality in the Editor
#endif
        if (!Advertisement.isInitialized && Advertisement.isSupported)
        {
            Advertisement.Initialize(unityGameId, testMode, this);
        }
    }

    public void OnInitializationComplete()
    {
        Debug.Log("Unity Ads initialization complete.");
        LoadUnityAd();
        LoadUnityRWAd();
    }

    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
        Debug.Log($"Unity Ads Initialization Failed: {error.ToString()} - {message}");
    }

    // Load content to the Ad Unit:
    public void LoadUnityAd()
    {
        // IMPORTANT! Only load content AFTER initialization (in this example, initialization is handled in a different script).
        Debug.Log("Loading Ad: " + adUnityUnitId);
        Advertisement.Load(adUnityUnitId, this);
    }

    // Show the loaded content in the Ad Unit:
    public void ShowUnityAd()
    {
        // Note that if the ad content wasn't previously loaded, this method will fail
        Debug.Log("Showing Ad: " + adUnityUnitId);
        Advertisement.Show(adUnityUnitId, this);
    }

   

    public void OnUnityAdsFailedToLoad(string _adUnitId, UnityAdsLoadError error, string message)
    {
        Debug.Log($"Error loading Ad Unit: {_adUnitId} - {error.ToString()} - {message}");
        // Optionally execute code if the Ad Unit fails to load, such as attempting to try again.
    }

    public void OnUnityAdsShowFailure(string _adUnitId, UnityAdsShowError error, string message)
    {
        Debug.Log($"Error showing Ad Unit {_adUnitId}: {error.ToString()} - {message}");
        // Optionally execute code if the Ad Unit fails to show, such as loading another ad.
    }

    public void OnUnityAdsShowStart(string _adUnitId) { }
    public void OnUnityAdsShowClick(string _adUnitId) { }


    //public void OnUnityAdsShowComplete(string _adUnitId, UnityAdsShowCompletionState showCompletionState) { }


    // Call this public method when you want to get an ad ready to show.
    public void LoadUnityRWAd()
    {
        // IMPORTANT! Only load content AFTER initialization (in this example, initialization is handled in a different script).
        Debug.Log("Loading Ad: " + adUnityRWUnitId);
        Advertisement.Load(adUnityRWUnitId, this);
    }

    // Implement a method to execute when the user clicks the button:
    public void ShowUnityRWAd()
    {
       
        // Then show the ad:
        Advertisement.Show(adUnityRWUnitId, this);
    }

    // If the ad successfully loads, add a listener to the button and enable it:
    public void OnUnityAdsAdLoaded(string adUnitId)
    {
        Debug.Log("Ad Loaded: " + adUnitId);

        if (adUnitId.Equals(adUnityRWUnitId))
        {

        }
    }

    // Implement the Show Listener's OnUnityAdsShowComplete callback method to determine if the user gets a reward:
    public void OnUnityAdsShowComplete(string adUnitId, UnityAdsShowCompletionState showCompletionState)
    {
        if (adUnitId.Equals(adUnityRWUnitId) && showCompletionState.Equals(UnityAdsShowCompletionState.COMPLETED))
        {

            if (currentRWType == REWARD_TYPE.X2GOLD_CHEST)
                GameManager.instance.uiManager.rewardView.ClaimX2CB();
            else if (currentRWType == REWARD_TYPE.GEMS_REWARD)
                GameManager.instance.uiManager.storeView.GetFreeGemsCB();
            else if (currentRWType == REWARD_TYPE.X2GOLD_GAME_OVER)
                GameManager.instance.uiManager.resultView.ClaimX2CB();
            else if (currentRWType == REWARD_TYPE.RETRIVE)
                GameManager.instance.uiManager.retriveView.RetriveByRewardVideoCB();
            else if (currentRWType == REWARD_TYPE.X4_COIN)
                GameManager.instance.uiManager.gameView.GetX4GoldCB();
            LoadUnityRWAd();
        }

        if (adUnitId.Equals(adUnityUnitId) && showCompletionState.Equals(UnityAdsShowCompletionState.COMPLETED))
        {
            LoadUnityAd();
        }
    }
    private void HandleInitCompleteAction(InitializationStatus initstatus)
    {
        Debug.Log("Initialization complete.");

        // Callbacks from GoogleMobileAds are not guaranteed to be called on
        // the main thread.
        // In this example we use MobileAdsEventExecutor to schedule these calls on
        // the next Update() loop.
        MobileAdsEventExecutor.ExecuteInUpdate(() =>
        {
            //RequestBannerAd();
            RequestAndLoadInterstitialAd();
            RequestAndLoadRewardedAd();
        });
    }

    #region HELPER METHODS

    private AdRequest CreateAdRequest()
    {
        return new AdRequest.Builder()
            .AddKeyword("unity-admob-sample")
            .Build();
    }

    public void OnApplicationPause(bool paused)
    {
        // Display the app open ad when the app is foregrounded.
        if (!paused)
        {
            // ShowAppOpenAd();
        }
    }

    #endregion

    #region BANNER ADS

    public void RequestBannerAd()
    {

        //PrintStatus("Requesting Banner ad.");

        // These ad units are configured to always serve test ads.
#if UNITY_EDITOR
        string adUnitId = "unused";

#elif UNITY_ANDROID
        string adUnitId =  Android_Banner_Key;
#elif UNITY_IPHONE
        string adUnitId = IOS_Banner_Key;
#else
        string adUnitId = "unexpected_platform";
#endif

        // Clean up banner before reusing
        if (bannerView != null)
        {
            bannerView.Destroy();
        }

        // Create a 320x50 banner at top of the screen
        bannerView = new BannerView(adUnitId, AdSize.Banner, AdPosition.Bottom);

        // Add Event Handlers
        bannerView.OnAdLoaded += (sender, args) =>
        {
            //PrintStatus("Banner ad loaded.");
            OnAdLoadedEvent.Invoke();
            HideBannerAd();
            
        };
        bannerView.OnAdFailedToLoad += (sender, args) =>
        {
            // PrintStatus("Banner ad failed to load with error: " + args.LoadAdError.GetMessage());
            OnAdFailedToLoadEvent.Invoke();
        };
        bannerView.OnAdOpening += (sender, args) =>
        {
            //PrintStatus("Banner ad opening.");
            OnAdOpeningEvent.Invoke();
        };
        bannerView.OnAdClosed += (sender, args) =>
        {
            //PrintStatus("Banner ad closed.");
            OnAdClosedEvent.Invoke();
        };
        bannerView.OnPaidEvent += (sender, args) =>
        {
            string msg = string.Format("{0} (currency: {1}, value: {2}",
                                        "Banner ad received a paid event.",
                                        args.AdValue.CurrencyCode,
                                        args.AdValue.Value);
            //PrintStatus(msg);
        };

        // Load a banner ad
        bannerView.LoadAd(CreateAdRequest());

    }

    public void DestroyBannerAd()
    {
        if (bannerView != null)
        {
            bannerView.Destroy();
        }
    }

    public void ShowBannerAd()
    {
        if (bannerView != null)
        {
            bannerView.Show();
        }
    }

    public void HideBannerAd()
    {
        if (bannerView != null)
        {
            bannerView.Hide();
        }
    }

    #endregion

    #region INTERSTITIAL ADS

    public void RequestAndLoadInterstitialAd()
    {
        // PrintStatus("Requesting Interstitial ad.");

#if UNITY_EDITOR
        string adUnitId = "unused";
#elif UNITY_ANDROID
        string adUnitId = Android_Interestital_Key;
#elif UNITY_IPHONE
        string adUnitId = IOS_Interestital_Key;
#else
        string adUnitId = "unexpected_platform";
#endif

        // Clean up interstitial before using it
        if (interstitialAd != null)
        {
            interstitialAd.Destroy();
        }

        interstitialAd = new InterstitialAd(adUnitId);

        // Add Event Handlers
        interstitialAd.OnAdLoaded += (sender, args) =>
        {
            //PrintStatus("Interstitial ad loaded.");
            OnAdLoadedEvent.Invoke();
        };
        interstitialAd.OnAdFailedToLoad += (sender, args) =>
        {
            //PrintStatus("Interstitial ad failed to load with error: " + args.LoadAdError.GetMessage());
            OnAdFailedToLoadEvent.Invoke();
        };
        interstitialAd.OnAdOpening += (sender, args) =>
        {
            //PrintStatus("Interstitial ad opening.");
            OnAdOpeningEvent.Invoke();
        };
        interstitialAd.OnAdClosed += (sender, args) =>
        {
            // PrintStatus("Interstitial ad closed.");
            RequestAndLoadInterstitialAd();
            OnAdClosedEvent.Invoke();
        };
        interstitialAd.OnAdDidRecordImpression += (sender, args) =>
        {
            //PrintStatus("Interstitial ad recorded an impression.");
        };
        interstitialAd.OnAdFailedToShow += (sender, args) =>
        {
            //PrintStatus("Interstitial ad failed to show.");
        };
        interstitialAd.OnPaidEvent += (sender, args) =>
        {
            string msg = string.Format("{0} (currency: {1}, value: {2}",
                                        "Interstitial ad received a paid event.",
                                        args.AdValue.CurrencyCode,
                                        args.AdValue.Value);
            //PrintStatus(msg);
        };

        // Load an interstitial ad
        interstitialAd.LoadAd(CreateAdRequest());
    }

    public void ShowInterstitialAd()
    {
        if (interstitialAd != null && interstitialAd.IsLoaded())
        {
            interstitialAd.Show();
        }
        else
        {
            //PrintStatus("Interstitial ad is not ready yet.");
        }
    }

    public void DestroyInterstitialAd()
    {
        if (interstitialAd != null)
        {
            interstitialAd.Destroy();
        }
    }

    #endregion

    #region REWARDED ADS

    public void RequestAndLoadRewardedAd()
    {
        // PrintStatus("Requesting Rewarded ad.");
#if UNITY_EDITOR
        string adUnitId = "unused";
#elif UNITY_ANDROID
        string adUnitId = Android_RW_Key;
#elif UNITY_IPHONE
        string adUnitId = IOS_RW_Key;
#else
        string adUnitId = "unexpected_platform";
#endif

        // create new rewarded ad instance
        rewardedAd = new RewardedAd(adUnitId);

        // Add Event Handlers
        rewardedAd.OnAdLoaded += (sender, args) =>
        {
            // PrintStatus("Reward ad loaded.");
            OnAdLoadedEvent.Invoke();
        };
        rewardedAd.OnAdFailedToLoad += (sender, args) =>
        {
            // PrintStatus("Reward ad failed to load.");
            OnAdFailedToLoadEvent.Invoke();
        };
        rewardedAd.OnAdOpening += (sender, args) =>
        {
            // PrintStatus("Reward ad opening.");
            OnAdOpeningEvent.Invoke();
        };
        rewardedAd.OnAdFailedToShow += (sender, args) =>
        {
            // PrintStatus("Reward ad failed to show with error: " + args.AdError.GetMessage());
            OnAdFailedToShowEvent.Invoke();
        };
        rewardedAd.OnAdClosed += (sender, args) =>
        {
            // PrintStatus("Reward ad closed.");
            RequestAndLoadRewardedAd();
            OnAdClosedEvent.Invoke();
        };
        rewardedAd.OnUserEarnedReward += (sender, args) =>
        {

            // PrintStatus("User earned Reward ad reward: " + args.Amount);

            if (currentRWType == REWARD_TYPE.X2GOLD_CHEST)
                GameManager.instance.uiManager.rewardView.ClaimX2CB();
            else if (currentRWType == REWARD_TYPE.GEMS_REWARD)
                GameManager.instance.uiManager.storeView.GetFreeGemsCB();
            else if (currentRWType == REWARD_TYPE.X2GOLD_GAME_OVER)
                GameManager.instance.uiManager.resultView.ClaimX2CB();
            else if (currentRWType == REWARD_TYPE.RETRIVE)
                GameManager.instance.uiManager.retriveView.RetriveByRewardVideoCB();
            else if (currentRWType == REWARD_TYPE.X4_COIN)
                GameManager.instance.uiManager.gameView.GetX4GoldCB();

            OnUserEarnedRewardEvent.Invoke();
        };
        rewardedAd.OnAdDidRecordImpression += (sender, args) =>
        {
            //PrintStatus("Reward ad recorded an impression.");
        };
        rewardedAd.OnPaidEvent += (sender, args) =>
        {
            string msg = string.Format("{0} (currency: {1}, value: {2}",
                                        "Rewarded ad received a paid event.",
                                        args.AdValue.CurrencyCode,
                                        args.AdValue.Value);
            //PrintStatus(msg);
        };

        // Create empty ad request
        rewardedAd.LoadAd(CreateAdRequest());
    }

    public void ShowRewardedAd(REWARD_TYPE _type)
    {
        currentRWType = _type;

        if (currentAdsType == ADS_TYPE.ADMOB)
        {
            if (rewardedAd != null)
            {

                if (rewardedAd.IsLoaded())
                {

                    rewardedAd.Show();
                }
               

            }

        }
        else if (currentAdsType == ADS_TYPE.UNITY)
        {
            ShowUnityRWAd();
        }
        else if (currentAdsType == ADS_TYPE.MEDIATION)
        {
            if (rewardedAd.IsLoaded())      
                rewardedAd.Show();
            else
                ShowUnityRWAd();
        }


       
    }

    public bool IsRewardedVideoAvailable()
    {
        bool isAvailable = false;

        isAvailable = rewardedAd.IsLoaded();

        return isAvailable;
    }


    public void RequestAndLoadRewardedInterstitialAd()
    {
        //PrintStatus("Requesting Rewarded Interstitial ad.");

        // These ad units are configured to always serve test ads.
#if UNITY_EDITOR
        string adUnitId = "unused";
#elif UNITY_ANDROID
            string adUnitId = "ca-app-pub-3940256099942544/5354046379";
#elif UNITY_IPHONE
            string adUnitId = "ca-app-pub-3940256099942544/6978759866";
#else
            string adUnitId = "unexpected_platform";
#endif

        // Create an interstitial.
        RewardedInterstitialAd.LoadAd(adUnitId, CreateAdRequest(), (rewardedInterstitialAd, error) =>
        {
            if (error != null)
            {
                //PrintStatus("Rewarded Interstitial ad load failed with error: " + error);
                return;
            }

            this.rewardedInterstitialAd = rewardedInterstitialAd;
            //PrintStatus("Rewarded Interstitial ad loaded.");

            // Register for ad events.
            this.rewardedInterstitialAd.OnAdDidPresentFullScreenContent += (sender, args) =>
            {
                // PrintStatus("Rewarded Interstitial ad presented.");
            };
            this.rewardedInterstitialAd.OnAdDidDismissFullScreenContent += (sender, args) =>
            {
                // PrintStatus("Rewarded Interstitial ad dismissed.");
                this.rewardedInterstitialAd = null;
            };
            this.rewardedInterstitialAd.OnAdFailedToPresentFullScreenContent += (sender, args) =>
            {
                // PrintStatus("Rewarded Interstitial ad failed to present with error: " + args.AdError.GetMessage());
                this.rewardedInterstitialAd = null;
            };
            this.rewardedInterstitialAd.OnPaidEvent += (sender, args) =>
            {
                string msg = string.Format("{0} (currency: {1}, value: {2}",
                                            "Rewarded Interstitial ad received a paid event.",
                                            args.AdValue.CurrencyCode,
                                            args.AdValue.Value);
                //PrintStatus(msg);
            };
            this.rewardedInterstitialAd.OnAdDidRecordImpression += (sender, args) =>
            {
                //PrintStatus("Rewarded Interstitial ad recorded an impression.");
            };
        });
    }

    public void ShowRewardedInterstitialAd()
    {
        if (rewardedInterstitialAd != null)
        {
            rewardedInterstitialAd.Show((reward) =>
            {
                //PrintStatus("Rewarded Interstitial ad Rewarded : " + reward.Amount);
            });
        }
        else
        {
            //PrintStatus("Rewarded Interstitial ad is not ready yet.");
        }
    }

    #endregion

    public void ShowInterstital()
    {

        int numberShow = PlayerPrefs.GetInt("ShowAds");

        if (numberShow < 1)
        {
            numberShow++;
            PlayerPrefs.SetInt("ShowAds", numberShow);
            return;
        }
        else
        {
            numberShow = 0;
            PlayerPrefs.SetInt("ShowAds", numberShow);

            if(currentAdsType == ADS_TYPE.ADMOB)
            {
                if (interstitialAd != null && interstitialAd.IsLoaded())
                {
                    interstitialAd.Show();
                }
            }
            else if(currentAdsType == ADS_TYPE.UNITY)
            {
                ShowUnityAd();
            }
            else if (currentAdsType == ADS_TYPE.MEDIATION)
            {
                if (interstitialAd != null && interstitialAd.IsLoaded())
                {
                    interstitialAd.Show();
                }
                else
                {
                    ShowUnityAd();
                }
            }


           

        }

       

    }




    public void RemoveAds(bool remove)
    {
        if (remove == true)
        {
            PlayerPrefs.SetInt("removeAds", 1);
            //if banner is active and user bought remove ads the banner will automatically hide
            HideBannerAd();
            DestroyBannerAd();
        }
        else
        {
            PlayerPrefs.SetInt("removeAds", 0);
        }
    }

    public bool CanShowAds()
    {
        if (!PlayerPrefs.HasKey("removeAds"))
        {
            return true;
        }
        else
        {
            if (PlayerPrefs.GetInt("removeAds") == 0)
            {
                return true;
            }
        }
        return false;
    }
}

