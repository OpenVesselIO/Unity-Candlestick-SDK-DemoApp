// Copyright © 2021 OpenVessel. All rights reserved.

#import <Candlestick/Candlestick.h>

#import "CSSdkPluginUtils.h"

extern "C" {

    void _CSDisconnectCurrentSession()
    {
        [CSKSdk.sharedInstance.appConnectManager disconnectCurrentSessionWithResultHandler:^(BOOL success) {}];
    }
    
    void _CSDisconnectAllSessions()
    {
        [CSKSdk.sharedInstance.appConnectManager disconnectAllSessionsWithResultHandler:^(BOOL success) {}];
    }
    
    
}
