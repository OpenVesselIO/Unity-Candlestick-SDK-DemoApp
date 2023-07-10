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

    public enum EarningsPromoType
    {

        Static,

        //Video,

    }

    public class EarningsPresentationSettings
    {

        public string UserId { get; }
        public EarningsPromoType PromoType = EarningsPromoType.Static;
        public string TriggerName;

        public EarningsPresentationSettings() : this(null) {}

        public EarningsPresentationSettings(string userId)
        {
            this.UserId = userId;
        }

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
        protected class PresentationSettingsJson
        {

            [SerializeField]
            string userId;

            [SerializeField]
            string promoType;

            [SerializeField]
            string triggerName;

            internal PresentationSettingsJson(EarningsPresentationSettings settings)
            {
                userId = settings.UserId;
                promoType = settings.PromoType.ToString().ToUpperInvariant();
                triggerName = settings.TriggerName;
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