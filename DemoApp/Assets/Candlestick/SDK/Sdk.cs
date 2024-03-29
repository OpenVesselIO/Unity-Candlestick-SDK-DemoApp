#if UNITY_ANDROID
#else

namespace Candlestick
{
    public class Sdk :
#if UNITY_EDITOR
    // Check for Unity Editor first since the editor also responds to the currently selected platform.
    SdkUnityEditor
#elif UNITY_ANDROID
    SdkAndroid
#elif UNITY_IPHONE || UNITY_IOS
    SdkiOs
#else
    SdkUnityEditor
#endif
    {
    }
}

#endif