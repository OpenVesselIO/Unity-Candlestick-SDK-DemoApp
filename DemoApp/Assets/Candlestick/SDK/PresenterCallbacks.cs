using System;
using Candlestick.Utils;
using UnityEngine;
using Logger = Candlestick.Utils.Logger;

namespace Candlestick
{
    public class PresenterCallbacks : MonoBehaviour
    {
        public static PresenterCallbacks Instance { get; private set; }

        private static Action _onPortalShow;
        private static Action _onPortalDismiss;

        public static event Action OnPortalShow
        {
            add
            {
                Logger.LogSubscribedToEvent("OnPortalShow");
                _onPortalShow += value;
            }
            remove
            {
                Logger.LogUnsubscribedToEvent("OnPortalShow");
                _onPortalShow -= value;
            }
        }

        public static event Action OnPortalDismiss
        {
            add
            {
                Logger.LogSubscribedToEvent("OnPortalDismiss");
                _onPortalDismiss += value;
            }
            remove
            {
                Logger.LogUnsubscribedToEvent("OnPortalDismiss");
                _onPortalDismiss -= value;
            }
        }

        public void ForwardOnPortalShowEvent(string msg)
        {
            EventInvoker.InvokeEvent(_onPortalShow);
        }

        public void ForwardOnPortalDismissEvent(string msg)
        {
            EventInvoker.InvokeEvent(_onPortalDismiss);
        }

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