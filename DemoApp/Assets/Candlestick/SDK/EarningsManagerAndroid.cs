using System;
using System.Collections;
using System.Runtime.InteropServices;
using UnityEngine;
using Logger = Candlestick.Utils.Logger;

namespace Candlestick
{
#if UNITY_ANDROID
    public class EarningsManagerAndroid: EarningsManagerBase
    {

        private static readonly AndroidJavaClass PluginClass =
            new AndroidJavaClass("com.candlestick.sdk.unity.EarningsManagerPlugin");

        public void TrackRevenuedAd(AdType adType)
        {
            PluginClass.CallStatic("trackRevenuedAd", adType.ToString().ToUpperInvariant());
        }

    }
#endif
}