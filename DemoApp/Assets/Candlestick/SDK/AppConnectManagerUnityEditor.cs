using System;
using System.Collections;
using UnityEngine;
using Logger = Candlestick.Utils.Logger;

namespace Candlestick
{
    public class AppConnectManagerUnityEditor : AppConnectManagerBase
    {
        
        /// <summary>
        /// Disconnect from the current session  
        /// </summary>
        public void DisconnectCurrentSession()
        {
            Logger.UserDebug("Disconnected");

            AppConnectManagerCallbacks.Instance.ForwardOnStateUpdatedEvent("{\"status\": \"Disconnected\"}");
        }

        /// <summary>
        /// Disconnect from all of the sessions  
        /// </summary>
        public void DisconnectAllSessions()
        {
            Logger.UserDebug("Disconnected from all sessions");

            AppConnectManagerCallbacks.Instance.ForwardOnStateUpdatedEvent("{\"status\": \"Disconnected\"}");
        }

        private static void ExecuteWithDelay(float seconds, Action action)
        {
            AppConnectManagerCallbacks.Instance.StartCoroutine(ExecuteAction(seconds, action));
        }

        private static IEnumerator ExecuteAction(float seconds, Action action)
        {
            yield return new WaitForSeconds(seconds);

            action();
        }

    }
}