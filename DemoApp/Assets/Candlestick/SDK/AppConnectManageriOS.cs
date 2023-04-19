using System.Runtime.InteropServices;

namespace Candlestick
{
#if UNITY_IOS
    public class AppConnectManageriOs : AppConnectManagerBase
    {

        [DllImport("__Internal")]
        private static extern void _CSDisconnectCurrentSession();

        /// <summary>
        /// Disconnect from the current session  
        /// </summary>
        public void DisconnectCurrentSession()
        {
            _CSDisconnectCurrentSession();
        }

        [DllImport("__Internal")]
        private static extern void _CSDisconnectAllSessions();

        /// <summary>
        /// Disconnect from all of the sessions  
        /// </summary>
        public void DisconnectAllSessions()
        {
            _CSDisconnectAllSessions();
        }

        }
#endif
}