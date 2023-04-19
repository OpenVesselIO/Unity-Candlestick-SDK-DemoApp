using UnityEngine;
using Logger = Candlestick.Utils.Logger;

namespace Candlestick
{
    public class PresenterUnityEditor : PresenterBase
    {

        /// <summary>
        /// Check if the companion application is installed.
        /// </summary>
        public bool IsCompanionApplicationInstalled()
        {
            Logger.UserDebug("Checking if the companion app is installed");
            return false;
        }

    }
}