using System;
using Candlestick.Utils;
using UnityEngine;
using Logger = Candlestick.Utils.Logger;

namespace Candlestick
{
    [Serializable]
    public class AppConnectStateJson
    {
        [SerializeField] public string status;

        [SerializeField] public string userId;

        [SerializeField] public string accessToken;
    }

    public class AppConnectManagerCallbacks : MonoBehaviour
    {
        public static AppConnectManagerCallbacks Instance { get; private set; }

        private static Action<AppConnectState> _onStateUpdatedEvent;

        public AppConnectState State { get; private set; } = new AppConnectState(
            AppConnectStatus.NotInitialized,
            null,
            null
        );

        public static event Action<AppConnectState> OnStateUpdated
        {
            add
            {
                Logger.LogSubscribedToEvent("OnStateUpdated");
                _onStateUpdatedEvent += value;
            }
            remove
            {
                Logger.LogUnsubscribedToEvent("OnStateUpdated");
                _onStateUpdatedEvent -= value;
            }
        }

        public void ForwardOnStateUpdatedEvent(string connectResultJson)
        {
            var eventJson = JsonUtility.FromJson<AppConnectStateJson>(connectResultJson);

            var statusParsed = Enum.TryParse(eventJson.status, out AppConnectStatus parsedStatus);
            if (!statusParsed)
            {
                Logger.E("Failed to parse app connect status '" + eventJson.status + "'");
                parsedStatus = AppConnectStatus.Disconnected;
            }

            State = new AppConnectState(parsedStatus, eventJson.userId, eventJson.accessToken);
            EventInvoker.InvokeEvent(_onStateUpdatedEvent, State);
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