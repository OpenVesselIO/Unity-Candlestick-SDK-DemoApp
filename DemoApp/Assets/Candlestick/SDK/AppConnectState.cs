using System;
using UnityEngine;

namespace Candlestick
{
    public class AppConnectState
    {
        public AppConnectStatus Status { get; }

        public string UserId { get; }
        
        public string AccessToken { get; }

        internal AppConnectState(AppConnectStatus status, string userId, string accessToken)
        {
            Status = status;
            UserId = userId;
            AccessToken = accessToken;
        }
    }
}