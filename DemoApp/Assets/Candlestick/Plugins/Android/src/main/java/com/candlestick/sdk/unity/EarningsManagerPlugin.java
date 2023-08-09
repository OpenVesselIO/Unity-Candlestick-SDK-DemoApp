// package com.candlestick.sdk.unity;

// import android.os.AsyncTask;

// import com.candlestick.sdk.CandlestickSdk;
// import com.candlestick.sdk.EarningsActivitySettings;
// import com.candlestick.sdk.EarningsManager;
// import com.candlestick.sdk.utils.Logger;
// import com.candlestick.sdk.utils.StringUtils;
// import com.unity3d.player.UnityPlayer;

// import org.json.JSONObject;

// import static com.unity3d.player.UnityPlayer.currentActivity;

// public class EarningsManagerPlugin
// {

//     private static final String TAG = "EarningsManagerPlugin";

//     private static final String CALLBACKS_OBJECT_NAME = "CSEarningsManagerCallbacks";

//     public static void trackRevenuedAd(final String adTypeString)
//     {
//         final CandlestickSdk sdk = CandlestickSdk.getInstance( currentActivity );

//         final EarningsManager.AdType adTypeOrNull = EarningsManager.AdType.valueOfOrNull( adTypeString );

//         if ( adTypeOrNull != null )
//         {
//             sdk.getEarningsManager().trackRevenuedAd( adTypeOrNull );
//         }
//     }

//     private static void UnitySendMessageAsync(final String method, final JSONObject jsonObj)
//     {
//         UnitySendMessageAsync( method, jsonObj.toString() );
//     }

//     private static void UnitySendMessageAsync(final String method, final String msg)
//     {
//         AsyncTask.THREAD_POOL_EXECUTOR.execute( () -> {
//             UnityPlayer.UnitySendMessage( CALLBACKS_OBJECT_NAME, method, msg );
//         } );
//     }

// }
