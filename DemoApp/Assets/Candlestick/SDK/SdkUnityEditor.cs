using UnityEngine;
using Logger = Candlestick.Utils.Logger;

namespace Candlestick
{
    public class SdkUnityEditor : SdkBase
    {

        static SdkUnityEditor()
        {
            InitSingletons();
        }

        private static SdkConfiguration _configuration;

        public static SdkConfiguration Configuration
        {
            get => _configuration;

            set
            {
                _configuration = value;
                Logger.UserDebug("Setting Candlestick config: " + JsonUtility.ToJson(value));
            }
        }

        /// <summary>
        /// App connect manager provides control over a session.
        /// </summary>
        /// <returns>Connection manager</returns>
        public static AppConnectManagerUnityEditor AppConnectManager => new AppConnectManagerUnityEditor();

        /// <summary>
        /// An object that can present various portal views
        /// </summary>
        /// <returns>Presenter</returns>
        public static PresenterUnityEditor Presenter => new PresenterUnityEditor();

        public static EarningsManagerUnityEditor EarningsManager => new EarningsManagerUnityEditor();

        /// <summary>
        /// Assign the environment that should be used for this SDK.
        /// <b>Please note</b>: the environment should be set before calling <code>Initialize()</code>
        /// </summary>
        /// <param name="Environment">
        /// Environment to set. Must not be null. PRODUCTION by default.
        /// </param>
        public static CandlestickEnvironment Environment { get; set; }

        /// <summary>
        /// Initialize Candlestick SDK
        /// </summary>
        public static void Initialize()
        {
            Initialize(null);
        }

        /// <summary>
        /// Initialize Candlestick SDK
        /// </summary>
        /// <param name="userId">
        /// Optional in-app user ID
        /// </param>
        public static void Initialize(string userId)
        {
            AppConnectManagerCallbacks.Instance.ForwardOnStateUpdatedEvent("{\"status\": \"Disconnected\"}");
            SdkCallbacks.Instance.ForwardOnSdkInitializedEvent("");
        }

    }
}