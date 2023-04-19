using UnityEngine;

namespace Candlestick
{
#if UNITY_ANDROID
    public class PresenterAndroid : PresenterBase
    {

        private static readonly AndroidJavaClass PluginClass =
            new AndroidJavaClass("com.candlestick.sdk.unity.PresenterPlugin");

        /// <summary>
        /// Check if the companion application is installed.
        /// </summary>
        public bool IsCompanionApplicationInstalled()
        {
            return PluginClass.CallStatic<bool>("isCompanionApplicationInstalled");
        }

    }
#endif
}