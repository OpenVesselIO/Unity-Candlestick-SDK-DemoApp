﻿using System;
using Candlestick;
using UnityEngine;
using UnityEngine.UI;

public class Demo : MonoBehaviour
{
    private const string USER_ID = "<Unique User ID of your application>";

    private Candlestick.AppConnectState _appConnectState;

    public Text _statusText;

    public Button _checkCompanionAppInstallButton;
    public Button _showEarningsWithStaticPromoButton;
    public Button _showEarningsWithVideoPromoButton;

    public Button _trackRandomRevenuedAdButton;

    public InputField _earningsImpressionTriggerNameInputField;
    public Button _trackEarningsImpressionButton;

    public InputField _earningsAuthPhoneNumberInputField;
    public Button _earningsAuthGeneratePhoneCodeButton;

    public InputField _earningsAuthPhoneCodeInputField;
    public Button _earningsAuthLoginByPhoneCodeButton;

    public InputField _earningsVerificationEmailInputField;
    public Button _earningsVerificationGenerateEmailCodeButton;

    public InputField _earningsVerificationEmailCodeInputField;
    public Button _earningsVerifyEmailButton;

    private int _portalShowCallCount;
    private int _portalDismissCallCount;

    private EarningsManagerCallbacks.AuthCodeMetadata _earningsAuthCodeMetadata;
    private EarningsManagerCallbacks.AuthCodeMetadata _earningsVerificationCodeMetadata;

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
        Candlestick.AppConnectManagerCallbacks.OnStateUpdated += HandleAppConnectState;
        Candlestick.PresenterCallbacks.OnPortalShow += HandlePortalShow;
        Candlestick.PresenterCallbacks.OnPortalDismiss += HandlePortalDismiss;
        Candlestick.EarningsManagerCallbacks.OnAuthCodeMetadata += HandleEarningsAuthCodeMetadata;
        Candlestick.EarningsManagerCallbacks.OnVerificationCodeMetadata += HandleEarningsVerificationCodeMetadata;
        Candlestick.EarningsManagerCallbacks.OnAuthFailure += HandleEarningsAuthFailure;
        Candlestick.EarningsManagerCallbacks.OnVerificationFailure += HandleEarningsVerificationFailure;
        Candlestick.EarningsManagerCallbacks.OnVerificationSuccess += HandleEarningsVerificationSuccess;

        Candlestick.Sdk.Configuration = new SdkConfiguration
        {
            MinLogLevel = SdkLogLevel.Debug
        };

        Debug.Log("Initializing the SDK...");
        Candlestick.Sdk.Initialize(USER_ID);
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

    public void ShowEarningsWithStaticPromo()
    {
        Debug.Log("Showing earnings with static promo inside of the current application...");

        Candlestick.Sdk.EarningsManager.ShowEarnings(USER_ID);
    }

    public void ShowEarningsWithVideoPromo()
    {
        Debug.Log("Showing earnings with video promo inside of the current application...");

        var settings = new EarningsPresentationSettings(USER_ID);
        settings.PromoType = EarningsPromoType.Video;
        settings.TriggerName = "show_earnings_with_video_promo_button";

        Candlestick.Sdk.EarningsManager.ShowEarnings(settings);
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

    public void GenerateEarningsPhoneAuthCode()
    {
        Candlestick.Sdk.EarningsManager.GenerateAuthCodeForPhoneNumber(_earningsAuthPhoneNumberInputField.text);
    }

    public void LoginEarningsByPhoneAuthCode()
    {
        Candlestick.Sdk.EarningsManager.LoginByPhoneAuthCode(
            _earningsAuthCodeMetadata.PhoneNumber,
            _earningsAuthPhoneCodeInputField.text,
            _earningsAuthCodeMetadata.CreatedAt,
            USER_ID
        );
    }

    public void GenerateEarningsEmailVerificationCode()
    {
        Candlestick.Sdk.EarningsManager.GenerateVerificationCodeForEmail(_earningsVerificationEmailInputField.text);
    }

    public void VerifyEarningsEmail()
    {
        Candlestick.Sdk.EarningsManager.VerifyEmail(
            _earningsVerificationCodeMetadata.Email,
            _earningsVerificationEmailCodeInputField.text,
            _earningsVerificationCodeMetadata.CreatedAt
        );
    }

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

    private void HandleEarningsAuthCodeMetadata(EarningsManagerCallbacks.AuthCodeMetadata codeMetadata)
    {
        _earningsAuthCodeMetadata = codeMetadata;

        _earningsAuthPhoneNumberInputField.text = null;
    }

    private void HandleEarningsVerificationCodeMetadata(EarningsManagerCallbacks.AuthCodeMetadata codeMetadata)
    {
        _earningsVerificationCodeMetadata = codeMetadata;

        _earningsVerificationEmailInputField.text = null;
    }

    private void HandleEarningsAuthFailure(string failure)
    {
        PopupUtils.ShowPopup(failure);
    }

    private void HandleEarningsVerificationFailure(string failure)
    {
        PopupUtils.ShowPopup(failure);
    }

    private void HandleEarningsVerificationSuccess()
    {
        PopupUtils.ShowPopup("Email is successfully verified!");

        _earningsVerificationEmailCodeInputField.text = null;
    }

    private void UpdateStatusText()
    {
        var isConnected = _appConnectState.Status == Candlestick.AppConnectStatus.Connected;
        var statusText = "";

        statusText += "(" + Candlestick.Sdk.Environment + ")";
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
        _showEarningsWithStaticPromoButton.interactable = true;
        _showEarningsWithVideoPromoButton.interactable = true;
        _trackRandomRevenuedAdButton.interactable = true;
        _trackEarningsImpressionButton.interactable = true;
        _earningsAuthGeneratePhoneCodeButton.interactable = true;
        _earningsAuthLoginByPhoneCodeButton.interactable = true;

        if (isConnected)
        {
            _earningsAuthPhoneCodeInputField.text = null;
        }
    }

}