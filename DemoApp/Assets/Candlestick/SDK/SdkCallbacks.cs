using System;
using System.Collections;
using Candlestick.Utils;
using UnityEngine;
using Logger = Candlestick.Utils.Logger;

namespace Candlestick
{
    public class SdkCallbacks : MonoBehaviour
    {

        [Serializable]
        private class ConsentFlowFinishParametersJson
        {

            [SerializeField] internal bool hasUserConsent;

        }

        public static SdkCallbacks Instance { get; private set; }

        private static Action _onSdkInitialized;
#if UNITY_IOS
        private static Action<bool> _onSdkConsentFlowFinished;
#endif

        public static event Action OnSdkInitialized
        {
            add
            {
                Logger.LogSubscribedToEvent("OnSdkInitialized");
                _onSdkInitialized += value;
            }
            remove
            {
                Logger.LogUnsubscribedToEvent("OnSdkInitialized");
                _onSdkInitialized -= value;
            }
        }
#if UNITY_IOS
        public static event Action<bool> OnSdkConsentFlowFinished
        {
            add
            {
                Logger.LogSubscribedToEvent("OnSdkConsentFlowFinished");
                _onSdkConsentFlowFinished += value;
            }
            remove
            {
                Logger.LogUnsubscribedToEvent("OnSdkConsentFlowFinished");
                _onSdkConsentFlowFinished -= value;
            }
        }
#endif
        public void ForwardOnSdkInitializedEvent(string msg)
        {
            EventInvoker.InvokeEvent(_onSdkInitialized);
        }
#if UNITY_IOS
        public void ForwardOnSdkConsentFlowFinishedEvent(string json)
        {
            var parameters = JsonUtility.FromJson<ConsentFlowFinishParametersJson>(json);

            EventInvoker.InvokeEvent(_onSdkConsentFlowFinished, parameters.hasUserConsent);
        }
#endif
        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
        }
    }
}