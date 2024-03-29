using System;
using System.Collections;
using System.Runtime.InteropServices;
using UnityEditor;
using UnityEngine;
using Logger = Candlestick.Utils.Logger;

namespace Candlestick
{
#if UNITY_IOS
    public class EarningsManageriOs: EarningsManagerBase
    {

        [DllImport("__Internal")]
        private static extern void _CSTrackRevenuedAd(string adType);

        public void TrackRevenuedAd(AdType adType)
        {
            _CSTrackRevenuedAd(adType.ToString().ToUpperInvariant());
        }

        [DllImport("__Internal")]
        private static extern string _CSGetEarningsExperimentUserInfo();

        public ExperimentInfo GetExperimentInfo()
        {
            var jsonString = _CSGetEarningsExperimentUserInfo();

            if (jsonString == null)
            {
                return null;
            }

            var json = JsonUtility.FromJson<ExperimentInfoJson>(jsonString);

            return new ExperimentInfo(json.installationId, json.extras);
        }

    }
#endif
}