// using System;
// using UnityEngine;

// namespace Candlestick
// {
// #if UNITY_ANDROID
//     public class AppConnectManagerAndroid : AppConnectManagerBase
//     {
//         private static readonly AndroidJavaClass SdkPluginClass =
//             new AndroidJavaClass("com.candlestick.sdk.unity.AppConnectManagerPlugin");

//         /// <summary>
//         /// Disconnect from the current session
//         /// </summary>
//         public void DisconnectCurrentSession()
//         {
//             SdkPluginClass.CallStatic("disconnectCurrentSession");
//         }

//         /// <summary>
//         /// Disconnect from all of the sessions
//         /// </summary>
//         public void DisconnectAllSessions()
//         {
//             SdkPluginClass.CallStatic("disconnectAllSessions");
//         }
//     }
// #endif
// }