using System;
using UnityEngine;
using Logger = Candlestick.Utils.Logger;

namespace Candlestick
{
    public class SdkBase
    {
        protected static void InitSingletons()
        {
            var sdkCallbackObject =
                new GameObject("CSSdkCallbacks", typeof(SdkCallbacks))
                    .GetComponent<SdkCallbacks>(); // Its Awake() method sets Instance.

            if (SdkCallbacks.Instance != sdkCallbackObject)
            {
                Logger.UserWarning("It looks like you have the " + sdkCallbackObject.name +
                                   " on a GameObject in your scene. Please remove the script from your scene.");
            }

            var appConnectManagerCallbackObject =
                new GameObject("CSAppConnectManagerCallbacks", typeof(AppConnectManagerCallbacks))
                    .GetComponent<AppConnectManagerCallbacks>(); // Its Awake() method sets Instance.

            if (AppConnectManagerCallbacks.Instance != appConnectManagerCallbackObject)
            {
                Logger.UserWarning("It looks like you have the " + appConnectManagerCallbackObject.name +
                                   " on a GameObject in your scene. Please remove the script from your scene.");
            }

            var presenterCallbackObject =
                new GameObject("CSPresenterCallbacks", typeof(PresenterCallbacks))
                    .GetComponent<PresenterCallbacks>(); // Its Awake() method sets Instance.

            if (PresenterCallbacks.Instance != presenterCallbackObject)
            {
                Logger.UserWarning("It looks like you have the " + presenterCallbackObject.name +
                                   " on a GameObject in your scene. Please remove the script from your scene.");
            }

            var earningsManagerCallbackObject =
                new GameObject("CSEarningsManagerCallbacks", typeof(EarningsManagerCallbacks))
                    .GetComponent<EarningsManagerCallbacks>(); // Its Awake() method sets Instance.

            if (EarningsManagerCallbacks.Instance != earningsManagerCallbackObject)
            {
                Logger.UserWarning("It looks like you have the " + earningsManagerCallbackObject.name +
                                   " on a GameObject in your scene. Please remove the script from your scene.");
            }
        }
    }
}