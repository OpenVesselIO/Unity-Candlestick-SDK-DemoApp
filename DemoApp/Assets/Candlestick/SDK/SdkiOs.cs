using System;
using UnityEngine;
using System.Runtime.InteropServices;

namespace Candlestick
{
#if UNITY_IOS
    public class SdkiOs : SdkBase
    {
        [DllImport("__Internal")]
        private static extern void _CSSetConfiguration(string configurationJson);

        private static SdkConfiguration _configuration;

        public static SdkConfiguration Configuration
        {
            get => _configuration;
            set
            {
                _configuration= value;

                var configurationStr = JsonUtility.ToJson(value);
                _CSSetConfiguration(configurationStr);
            }
        }

        /// <summary>
        /// App connect manager provides control over a session.
        /// </summary>
        /// <returns>Connection manager</returns>
        public static AppConnectManageriOs AppConnectManager => new AppConnectManageriOs();

        /// <summary>
        /// An object that can present various portal views
        /// </summary>
        /// <returns>Presenter</returns>
        public static PresenteriOs Presenter => new PresenteriOs();

        public static EarningsManageriOs EarningsManager => new EarningsManageriOs();

        static SdkiOs()
        {
            InitSingletons();
        }

        [DllImport("__Internal")]
        private static extern void _CSInitialize(string userId);


        /// <summary>
        /// Initialize Candlestick SDK
        /// </summary>
        /// <param name="userId">
        /// In-app user ID
        /// </param>
        public static void Initialize(string userId)
        {
            _CSInitialize(userId);

            Application.deepLinkActivated += OnDeepLinkActivated;

            if (!string.IsNullOrEmpty(Application.absoluteURL))
            {
                OnDeepLinkActivated(Application.absoluteURL);
            }
        }

        [DllImport("__Internal")]
        private static extern void _CSSetEnvironment(string environment);

        [DllImport("__Internal")]
        private static extern string _CSGetEnvironment();

        [DllImport("__Internal")]
        private static extern bool _CSHandleDeeplink(string deeplink);

        private static void OnDeepLinkActivated(string url)
        {
            _CSHandleDeeplink(url);
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
                var envString = _CSGetEnvironment();
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

            set { _CSSetEnvironment(value.ToString().ToUpperInvariant()); }
        }
    }
#endif
}