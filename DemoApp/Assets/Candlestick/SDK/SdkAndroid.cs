using System;
using UnityEngine;

namespace Candlestick
{
#if UNITY_ANDROID
    public class SdkAndroid : SdkBase
    {
        private static readonly AndroidJavaClass PluginClass =
            new AndroidJavaClass("com.candlestick.sdk.unity.SdkPlugin");

        private static SdkConfiguration _configuration;

        public static SdkConfiguration Configuration
        {
            get => _configuration;
            set
            {
                _configuration = value;

                var configurationStr = JsonUtility.ToJson(value);
                PluginClass.CallStatic("setConfiguration", configurationStr);
            }
        }

        static SdkAndroid()
        {
            InitSingletons();
        }

        /// <summary>
        /// App connect manager provides control over a session.
        /// </summary>
        /// <returns>Connection manager</returns>
        public static AppConnectManagerAndroid AppConnectManager => new AppConnectManagerAndroid();

        /// <summary>
        /// An object that can present various portal views
        /// </summary>
        /// <returns>Presenter</returns>
        public static PresenterAndroid Presenter => new PresenterAndroid();

        public static EarningsManagerAndroid EarningsManager => new EarningsManagerAndroid();

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
        public static void Initialize(string userId)
        {
            PluginClass.CallStatic("initialize", userId);
        }

        /// <summary>
        /// Assign the environment that should be used for this SDK.
        /// <b>Please note</b>: the environment should be set before calling <code>Initialize()</code>
        /// </summary>
        /// <param name="Environment">
        /// Environment to set. Must not be null. PRODUCTION by default.
        /// </param>
        public static CandlestickEnvironment Environment
        {
            get
            {
                var envString = PluginClass.CallStatic<string>("getEnvironment");
                var envParsed = Enum.TryParse(envString, true, out CandlestickEnvironment parsedEnv);
                if (envParsed)
                {
                    return parsedEnv;
                }
                else
                {
                    return CandlestickEnvironment.Production;
                }
            }

            set { PluginClass.CallStatic("setEnvironment", value.ToString().ToUpperInvariant()); }
        }
        
    }
#endif
}