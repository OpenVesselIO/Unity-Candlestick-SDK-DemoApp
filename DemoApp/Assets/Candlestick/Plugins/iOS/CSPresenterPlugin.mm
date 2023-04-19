// Copyright © 2021 OpenVessel. All rights reserved.

#import <Candlestick/Candlestick.h>

#import "CSSdkPluginUtils.h"

extern "C" {

    BOOL _CSIsCompanionApplicationInstalled()
    {
        return [CSKSdk.sharedInstance.presentationController isCompanionApplicationInstalled];
    }

}
