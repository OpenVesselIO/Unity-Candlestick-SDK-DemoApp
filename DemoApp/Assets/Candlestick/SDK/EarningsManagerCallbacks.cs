using System;
using Candlestick.Utils;
using UnityEngine;
using static Candlestick.EarningsManagerCallbacks;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;
using Logger = Candlestick.Utils.Logger;

namespace Candlestick
{

    public class EarningsManagerCallbacks : MonoBehaviour
    {

        public static EarningsManagerCallbacks Instance { get; private set; }

        private static Action _onExperimentInfoUpdated;

        public static event Action OnExperimentInfoUpdated
        {
            add
            {
                Logger.LogSubscribedToEvent("OnExperimentInfoUpdated");
                _onExperimentInfoUpdated += value;
            }
            remove
            {
                Logger.LogUnsubscribedToEvent("OnExperimentInfoUpdated");
                _onExperimentInfoUpdated -= value;
            }
        }

        public void ForwardOnExperimentInfoUpdatedEvent(string unused)
        {
            EventInvoker.InvokeEvent(_onExperimentInfoUpdated);
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