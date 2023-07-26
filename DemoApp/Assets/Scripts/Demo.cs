using System;
using Candlestick;
using UnityEngine;
using UnityEngine.UI;

public class Demo : MonoBehaviour
{

    [Serializable]
    private class ExperimentExtras
    {

        [SerializeField] internal string genericFlow;
        [SerializeField] internal string inittimers;

    }

    private Candlestick.AppConnectState _appConnectState;

    public Text _statusText;

    public Button _checkCompanionAppInstallButton;

    public Button _trackRandomRevenuedAdButton;

    public InputField _earningsImpressionTriggerNameInputField;
    public Button _trackEarningsImpressionButton;

    private int _portalShowCallCount;
    private int _portalDismissCallCount;

#if UNITY_IOS
    private string genericFlow
    {
        get
        {
            var info = Candlestick.Sdk.EarningsManager.GetExperimentInfo();

            if (info.Extras == null)
            {
                return null;
            }

            return JsonUtility.FromJson<ExperimentExtras>(info.Extras).genericFlow;
        }
    }
#endif

    void Start()
    {
        Debug.Log("Starting...");

        UpdateButtons(false);

        _statusText.text = "Starting...";

        var environment = CandlestickEnvironment.Staging;
#if CS_ENVIRONMENT_DEV
        environment = CandlestickEnvironment.Development;
#elif CS_ENVIRONMENT_TESTING
        environment = CandlestickEnvironment.Testing;
#elif CS_ENVIRONMENT_STAGING
        environment = CandlestickEnvironment.Staging;
#elif CS_ENVIRONMENT_PROD
        environment = CandlestickEnvironment.Production;
#endif

        Candlestick.Sdk.Environment = environment;
#if UNITY_IOS
        Candlestick.SdkCallbacks.OnSdkConsentFlowFinished += HandleConsentFlowInfo;
#endif
        Candlestick.AppConnectManagerCallbacks.OnStateUpdated += HandleAppConnectState;
        Candlestick.PresenterCallbacks.OnPortalShow += HandlePortalShow;
        Candlestick.PresenterCallbacks.OnPortalDismiss += HandlePortalDismiss;
#if UNITY_IOS
        Candlestick.EarningsManagerCallbacks.OnExperimentInfoUpdated += UpdateStatusText;
#endif

        Candlestick.Sdk.Configuration = new SdkConfiguration
        {
            MinLogLevel = SdkLogLevel.Debug
        };

        Debug.Log("Initializing the SDK...");
        Candlestick.Sdk.Initialize();
    }

    public void DisconnectCurrent()
    {
        Debug.Log("Disconnecting the session...");
        Candlestick.Sdk.AppConnectManager.DisconnectCurrentSession();
    }

    public void DisconnectAll()
    {
        Debug.Log("Disconnecting all sessions...");
        Candlestick.Sdk.AppConnectManager.DisconnectAllSessions();
    }

    public void CheckCompanionAppInstall()
    {
        string text;
        if (Candlestick.Sdk.Presenter.IsCompanionApplicationInstalled())
        {
            text = "Companion App is installed";
        }
        else
        {
            text = "Companion App is not installed";
        }

        PopupUtils.ShowPopup(text);
    }

    public void TrackRandomRevenuedAd()
    {
        Debug.Log("Tracking random revenued ad...");

        var values = (AdType[]) Enum.GetValues(typeof(AdType));
        var adType = values[new System.Random().Next(values.Length)];

        Candlestick.Sdk.EarningsManager.TrackRevenuedAd(adType);

        PopupUtils.ShowPopup(adType.ToString());
    }

    public void TrackEarningsImpression()
    {
        Candlestick.Sdk.EarningsManager.TrackImpression(_earningsImpressionTriggerNameInputField.text);
    }
#if UNITY_IOS
    private void HandleConsentFlowInfo(bool hasUserConsent)
    {
        if (hasUserConsent)
        {
            MaxSdk.SetExtraParameter("consent_flow_enabled", "false");
        }

        MaxSdk.InitializeSdk();
    }
#endif
    private void HandleAppConnectState(Candlestick.AppConnectState state)
    {
        Debug.Log("Got new state: " + state);

        _appConnectState = state;

        UpdateStatusText();
        UpdateButtons(state.Status == Candlestick.AppConnectStatus.Connected);
    }

    private void HandlePortalShow()
    {
        _portalShowCallCount++;

        UpdateStatusText();
    }

    private void HandlePortalDismiss()
    {
        _portalDismissCallCount++;

        UpdateStatusText();
    }

    private void UpdateStatusText()
    {
        var isConnected = _appConnectState.Status == Candlestick.AppConnectStatus.Connected;
        var statusText = "";

        statusText += "(" + Candlestick.Sdk.Environment + ")";
#if UNITY_IOS
        if (genericFlow != null)
        {
            statusText += "\n";
            statusText += $"Generic Flow: {genericFlow}";
        }
#endif
        statusText += "\n";
        statusText += $"Portal Show/Dismiss: {_portalShowCallCount}/{_portalDismissCallCount}";

        _statusText.text = statusText;
    }

    private void UpdateButtons(bool isConnected)
    {
        foreach (var button in GetComponentsInChildren<Button>())
        {
            if (button.name.StartsWith("BtnOpen"))
            {
                button.interactable = true;
            }
            else
            {
                button.interactable = isConnected;
            }
        }

        _checkCompanionAppInstallButton.interactable = true;
        _trackRandomRevenuedAdButton.interactable = true;
        _trackEarningsImpressionButton.interactable = true;
    }

}