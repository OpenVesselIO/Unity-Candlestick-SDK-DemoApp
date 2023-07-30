using System;
using Candlestick.Utils;
using UnityEngine;
using Logger = Candlestick.Utils.Logger;

namespace Candlestick
{

    public enum AdType
    {

        AppOpen,

        Banner,

        Interstitial,

        MREC,

        Rewarded,

    }

    public class EarningsManagerBase
    {

        public class ExperimentInfo
        {

            public string InstallationId { get; }

            // The field is nullable.
            public string Extras { get; }

            public ExperimentInfo(string installationId, string extras)
            {
                InstallationId = installationId;
                Extras = extras;
            }

        }

        [Serializable]
        protected class ExperimentInfoJson
        {

            [SerializeField] internal string installationId;
            [SerializeField] internal string extras;

        }

    }

}