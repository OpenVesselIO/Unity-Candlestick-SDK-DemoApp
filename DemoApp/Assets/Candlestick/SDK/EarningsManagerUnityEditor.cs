using System;
using System.Collections;
using UnityEngine;
using Logger = Candlestick.Utils.Logger;

namespace Candlestick
{
    public class EarningsManagerUnityEditor : EarningsManagerBase
    {

        public void TrackRevenuedAd(AdType adType)
        {
        }

        public void TrackImpression(string triggerName)
        {
        }

        public void ShowEarnings()
        {
            ShowEarnings(new EarningsPresentationSettings());
        }

        public void ShowEarnings(string userId)
        {
            ShowEarnings(new EarningsPresentationSettings(userId));
        }

        public void ShowEarnings(EarningsPresentationSettings settings)
        {
            Logger.UserDebug("Showing in-app page for earnings of '" + settings.UserId + "' with promo type '" + settings.PromoType.ToString() + "'...");
        }

        public ExperimentInfo GetExperimentInfo()
        {
            return null;
        }

    }
}