using UnityEngine;
using System.Runtime.InteropServices;

namespace Candlestick
{
#if UNITY_IOS
    public class PresenteriOs : PresenterBase
    {

        [DllImport("__Internal")]
        private static extern bool _CSIsCompanionApplicationInstalled();

        /// <summary>
        /// Check if the companion application is installed.
        /// </summary>
        public bool IsCompanionApplicationInstalled()
        {
            return _CSIsCompanionApplicationInstalled();
        }

    }
#endif
}