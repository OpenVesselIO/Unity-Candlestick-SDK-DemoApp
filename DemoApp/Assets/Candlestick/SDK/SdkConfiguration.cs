using System;
using UnityEngine;
using Candlestick.Utils;

namespace Candlestick
{
    [Serializable]
    public class SdkConfiguration
    {

        [SerializeField] public SdkLogLevel MinLogLevel = SdkLogLevel.Error;

#if !UNITY_EDITOR

        public SdkConfiguration()
        {
        }

#endif
    }
}