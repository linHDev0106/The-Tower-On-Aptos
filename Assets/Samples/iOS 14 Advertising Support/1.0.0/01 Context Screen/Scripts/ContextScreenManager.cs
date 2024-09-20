using Unity.Advertisement.IosSupport.Components;
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

namespace Unity.Advertisement.IosSupport.Samples
{
    /// <summary>
    /// This component will trigger the context screen to appear when the scene starts,
    /// if the user hasn't already responded to the iOS tracking dialog.
    /// </summary>
    //    public class ContextScreenManager : MonoBehaviour
    //    {
    //        /// <summary>
    //        /// The prefab that will be instantiated by this component.
    //        /// The prefab has to have an ContextScreenView component on its root GameObject.
    //        /// </summary>
    //        public ContextScreenView contextScreenPrefab;

    //        void Start()
    //        {
    //#if UNITY_IOS

    //            int mainVersion = 0;
    //            string[] versionPart = UnityEngine.iOS.Device.systemVersion.Split('.');
    //            int.TryParse(versionPart[0], out mainVersion);


    //            /// only run on iOS 14 devices

    //            if (mainVersion >= 14)
    //                ShowIOSTracking();
    //            else
    //                SceneManager.LoadScene("Game");

    //#endif
    //#if UNITY_ANDROID
    //SceneManager.LoadScene("Game");

    //#endif

    //        }


    //        public void ShowIOSTracking()
    //        {
    //#if UNITY_IOS
    //            // check with iOS to see if the user has accepted or declined tracking
    //            var status = ATTrackingStatusBinding.GetAuthorizationTrackingStatus();

    //            if (status == ATTrackingStatusBinding.AuthorizationTrackingStatus.NOT_DETERMINED)
    //            {
    //                var contextScreen = Instantiate(contextScreenPrefab).GetComponent<ContextScreenView>();

    //                // after the Continue button is pressed, and the tracking request
    //                // has been sent, automatically destroy the popup to conserve memory
    //                contextScreen.sentTrackingAuthorizationRequest += () => Destroy(contextScreen.gameObject);
    //            }
    //#else
    //            Debug.Log("Unity iOS Support: App Tracking Transparency status not checked, because the platform is not iOS.");
    //#endif

    //            StartCoroutine(LoadNextScene());

    //        }

    //        private IEnumerator LoadNextScene()
    //        {
    //#if UNITY_IOS && !UNITY_EDITOR
    //            var status = ATTrackingStatusBinding.GetAuthorizationTrackingStatus();

    //            while (status == ATTrackingStatusBinding.AuthorizationTrackingStatus.NOT_DETERMINED)
    //            {
    //                status = ATTrackingStatusBinding.GetAuthorizationTrackingStatus();
    //                yield return null;
    //            }

    //#endif
    //            SceneManager.LoadScene("Game");
    //            yield return null;
    //        }
    //    }
    //    
    public class ContextScreenManager : MonoBehaviour
    {
        void Start()
        {
            //#if UNITY_WEBGL
            //            // Chạy trực tiếp cảnh "Game" cho WebGL
            //            SceneManager.LoadScene("Game");
            //#elif UNITY_IOS || UNITY_ANDROID
            //            // Chạy trực tiếp cảnh "Game" cho iOS và Android
            //            SceneManager.LoadScene("Game");
            //#endif
            //        }
            SceneManager.LoadScene("Game");
        }
    }
}
