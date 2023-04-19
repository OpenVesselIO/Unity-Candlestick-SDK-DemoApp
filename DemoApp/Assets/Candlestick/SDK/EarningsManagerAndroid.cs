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

        public void TrackImpression(string triggerName)
        {
            PluginClass.CallStatic("trackImpression", triggerName);
        }

        public void ShowEarnings(string userId)
        {
            ShowEarnings(new EarningsPresentationSettings(userId));
        }

        public void ShowEarnings(EarningsPresentationSettings settings)
        {
            PluginClass.CallStatic("showEarnings", JsonUtility.ToJson(new PresentationSettingsJson(settings)));
        }

        public void GenerateAuthCodeForPhoneNumber(string phoneNumber)
        {
            PluginClass.CallStatic("generatePhoneAuthCode", phoneNumber);
        }

        public void LoginByPhoneAuthCode(string phoneNumber, string code, Int64 codeCreatedAt, string userId)
        {
            var json = new LoginJson(phoneNumber, code, codeCreatedAt, userId);

            PluginClass.CallStatic("loginByPhoneAuthCode", JsonUtility.ToJson(json));
        }

        public void GenerateVerificationCodeForEmail(string email)
        {
            PluginClass.CallStatic("generateEmailVerificationCode", email);
        }

        public void VerifyEmail(string email, string code, Int64 codeCreatedAt)
        {
            var json = new VerificationJson(email, code, codeCreatedAt);

            PluginClass.CallStatic("verifyEmailByCode", JsonUtility.ToJson(json));
        }

    }
#endif
}