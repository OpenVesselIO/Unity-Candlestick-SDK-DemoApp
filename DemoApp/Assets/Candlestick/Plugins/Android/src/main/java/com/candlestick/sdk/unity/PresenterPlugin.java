package com.candlestick.sdk.unity;

import android.os.AsyncTask;

import com.unity3d.player.UnityPlayer;

import com.candlestick.sdk.CandlestickSdk;
import com.candlestick.sdk.PresenterListener;

import static com.unity3d.player.UnityPlayer.currentActivity;

public class PresenterPlugin
{

    private static final PresenterListener forwardingListener = new ForwardingPresenterListener();

    static void initialize()
    {
        final CandlestickSdk sdk = CandlestickSdk.getInstance( currentActivity );
        sdk.getPresenter().setListener( forwardingListener );
    }

    public static boolean isCompanionApplicationInstalled()
    {
        final CandlestickSdk sdk = CandlestickSdk.getInstance( currentActivity );
        return sdk.getPresenter().isCompanionApplicationInstalled();
    }

    private static class ForwardingPresenterListener
            implements PresenterListener
    {

        private static final String CallbacksClassName = "CSPresenterCallbacks";

        @Override
        public void onPortalShow()
        {
            AsyncTask.THREAD_POOL_EXECUTOR.execute( () -> UnityPlayer.UnitySendMessage( CallbacksClassName, "ForwardOnPortalShowEvent", "" ) );
        }

        @Override
        public void onPortalDismiss()
        {
            AsyncTask.THREAD_POOL_EXECUTOR.execute( () -> UnityPlayer.UnitySendMessage( CallbacksClassName, "ForwardOnPortalDismissEvent", "" ) );
        }

    }

}
