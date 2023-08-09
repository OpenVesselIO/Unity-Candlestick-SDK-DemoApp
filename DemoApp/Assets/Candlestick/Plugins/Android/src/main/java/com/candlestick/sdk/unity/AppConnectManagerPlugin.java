// package com.candlestick.sdk.unity;


// import android.os.AsyncTask;
// import android.util.Log;

// import com.unity3d.player.UnityPlayer;

// import org.json.JSONException;
// import org.json.JSONObject;

// import com.candlestick.sdk.AppConnectListener;
// import com.candlestick.sdk.AppConnectManager;
// import com.candlestick.sdk.AppConnectState;
// import com.candlestick.sdk.AppConnectStatus;
// import com.candlestick.sdk.CandlestickSdk;

// import static com.unity3d.player.UnityPlayer.currentActivity;

// public class AppConnectManagerPlugin
// {
//     private static final String TAG = "AppConnectManagerPlugin";

//     private static final AppConnectListener forwardingListener = new ForwardingAppConnectListener();

//     public static void disconnectCurrentSession()
//     {
//         final CandlestickSdk sdk = CandlestickSdk.getInstance( currentActivity );
//         sdk.getAppConnectManager().disconnectCurrentSession();
//     }

//     public static void disconnectAllSessions()
//     {
//         final CandlestickSdk sdk = CandlestickSdk.getInstance( currentActivity );
//         sdk.getAppConnectManager().disconnectAllSessions();
//     }

//     static void initialize()
//     {
//         final CandlestickSdk sdk = CandlestickSdk.getInstance( currentActivity );
//         sdk.getAppConnectManager().setAppConnectListener( forwardingListener );
//     }

//     private static class ForwardingAppConnectListener
//             implements AppConnectListener
//     {

//         @Override
//         public void onStateUpdated(final AppConnectManager manager)
//         {
//             final AppConnectState newState = manager.getState();
//             Log.w( "Plugin", "Forwarding ForwardOnStatusUpdated " + toJsonStr( newState ) );

//             AsyncTask.THREAD_POOL_EXECUTOR.execute( () -> {
//                 UnityPlayer.UnitySendMessage( "CSAppConnectManagerCallbacks", "ForwardOnStateUpdatedEvent", toJsonStr( newState ) );
//             } );
//         }

//         private static String toJsonStr(final AppConnectState newState)
//         {
//             try
//             {
//                 final JSONObject result = new JSONObject();
//                 result.put( "status", toUnityString( newState.getStatus() ) );
//                 result.put( "userId", newState.getUserId() );
//                 result.put( "accessToken", newState.getAccessToken() );
//                 return result.toString();
//             }
//             catch ( JSONException e )
//             {
//                 Log.e( TAG, "Failed to convert app connect state to JSON: " + newState, e );

//                 return "{\"status\": \"Error\"}";
//             }
//         }

//         private static String toUnityString(final AppConnectStatus status)
//         {
//             if ( status == AppConnectStatus.NOT_INITIALIZED )
//             {
//                 return "NotInitialized";
//             }
//             else if ( status == AppConnectStatus.CONNECTED )
//             {
//                 return "Connected";
//             }
//             else
//             {
//                 return "Disconnected";
//             }
//         }
//     }
// }
