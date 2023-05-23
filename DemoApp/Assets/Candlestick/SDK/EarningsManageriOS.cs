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
        private class ExperimentInfoJson
        {

            [SerializeField] internal string installationId;
            [SerializeField] internal string extras;

        }

        [DllImport("__Internal")]
        private static extern void _CSTrackRevenuedAd(string adType);

        public void TrackRevenuedAd(AdType adType)
        {
            _CSTrackRevenuedAd(adType.ToString().ToUpperInvariant());
        }

        [DllImport("__Internal")]
        private static extern void _CSTrackImpression(string triggerName);

        public void TrackImpression(string triggerName)
        {
            _CSTrackImpression(triggerName);
        }

        [DllImport("__Internal")]
        private static extern void _CSShowEarnings(string settingsJson);

        public void ShowEarnings(string userId)
        {
            ShowEarnings(new EarningsPresentationSettings(userId));
        }

        public void ShowEarnings(EarningsPresentationSettings settings)
        {
            _CSShowEarnings(JsonUtility.ToJson(new PresentationSettingsJson(settings)));
        }

        [DllImport("__Internal")]
        private static extern void _CSGenerateAuthCodeForPhoneNumber(string phoneNumber);

        public void GenerateAuthCodeForPhoneNumber(string phoneNumber)
        {
            _CSGenerateAuthCodeForPhoneNumber(phoneNumber);
        }

        [DllImport("__Internal")]
        private static extern void _CSLoginByPhoneAuthCode(string loginJson);

        public void LoginByPhoneAuthCode(string phoneNumber, string code, Int64 codeCreatedAt, string userId)
        {
            var json = new LoginJson(phoneNumber, code, codeCreatedAt, userId);

            _CSLoginByPhoneAuthCode(JsonUtility.ToJson(json));
        }

        [DllImport("__Internal")]
        private static extern void _CSGenerateVerificationCodeForEmail(string email);

        public void GenerateVerificationCodeForEmail(string email)
        {
            _CSGenerateVerificationCodeForEmail(email);
        }

        [DllImport("__Internal")]
        private static extern void _CSVerifyEmail(string verifyJson);

        public void VerifyEmail(string email, string code, Int64 codeCreatedAt)
        {
            var json = new VerificationJson(email, code, codeCreatedAt);

            _CSVerifyEmail(JsonUtility.ToJson(json));
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