package com.candlestick.sdk.unity;


import android.net.Uri;
import android.os.AsyncTask;

import com.unity3d.player.UnityPlayer;

import org.json.JSONObject;

import com.candlestick.sdk.SdkConfiguration;
import com.candlestick.sdk.CandlestickEnvironment;
import com.candlestick.sdk.CandlestickSdk;
import com.candlestick.sdk.CandlestickSdkListener;
import com.candlestick.sdk.utils.Logger;
import com.candlestick.sdk.utils.StringUtils;

import static com.unity3d.player.UnityPlayer.currentActivity;

public class SdkPlugin
{
    private static final CandlestickSdkListener forwardingListener = new ForwardingSdkListener();

    public static void initialize(final String cuid)
    {
        AppConnectManagerPlugin.initialize();
        PresenterPlugin.initialize();

        final CandlestickSdk sdk = CandlestickSdk.getInstance( currentActivity );
        sdk.setListener( forwardingListener );

        sdk.initialize( cuid );
    }

    public static void setEnvironment(final String environment)
    {
        final CandlestickSdk sdk = CandlestickSdk.getInstance( currentActivity );
        sdk.setEnvironment( CandlestickEnvironment.valueOf( environment ) );
    }

    public static void setConfiguration(final String settingsJson)
    {
        if ( StringUtils.isValidString( settingsJson ) )
        {
            try
            {
                final JSONObject settings = new JSONObject( settingsJson );
                final int logLevelInt = settings.getInt( "MinLogLevel" );
                final SdkConfiguration.SdkLogLevel logLevel = SdkConfiguration.SdkLogLevel.valueOfOrdinal( logLevelInt );

                final CandlestickSdk sdk = CandlestickSdk.getInstance( currentActivity );
                sdk.setConfiguration( SdkConfiguration.builder()
                                              .minLogLevel( logLevel )
                                              .build() );
            }
            catch ( Exception ex )
            {
                Logger.userError( "SdkPlugin", "Unable to parse settings", ex );
            }
        }
    }

    public static String getEnvironment()
    {
        final CandlestickSdk sdk = CandlestickSdk.getInstance( currentActivity );
        return sdk.getEnvironment().toString();
    }

    private static class ForwardingSdkListener
            implements CandlestickSdkListener
    {
        @Override public void onInitialized()
        {
            AsyncTask.THREAD_POOL_EXECUTOR.execute( () -> UnityPlayer.UnitySendMessage( "CSSdkCallbacks", "ForwardOnSdkInitializedEvent", "" ) );
        }
    }
}
