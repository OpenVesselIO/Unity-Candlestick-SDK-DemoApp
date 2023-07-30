// Copyright © 2021 OpenVessel. All rights reserved.

#import <Candlestick/Candlestick.h>

#import "CSSdkPluginUtils.h"
#import "CSSdkPluginDelegateForwarder.h"
#import "CSAppConnectManagerDelegateForwarder.h"
#import "CSPresenterPluginDelegateForwarder.h"
#import "CSEarningsManagerDelegateForwarder.h"

extern "C" {
        
    void _CSInitialize()
    {
        [CSSdkPluginDelegateForwarder.sharedInstance attachDelegate];
        [CSAppConnectManagerDelegateForwarder.sharedInstance attachDelegate];
        [CSPresenterPluginDelegateForwarder.sharedInstance attachDelegate];
        [CSEarningsManagerDelegateForwarder.sharedInstance attachDelegate];
        
        [CSKSdk.sharedInstance start];
    }

    void _CSSetEnvironment(const char * environment)
    {
        if (strcmp("PRODUCTION", environment) == 0) {
            [CSKSdk.sharedInstance setEnvironment: CSKEnvironmentProduction];
        } else if (strcmp("STAGING", environment) == 0) {
            [CSKSdk.sharedInstance setEnvironment: CSKEnvironmentStaging];
        } else if (strcmp("DEVELOPMENT", environment) == 0) {
            [CSKSdk.sharedInstance setEnvironment: CSKEnvironmentDevelopment];
        }
    }

    void _CSSetConfiguration(const char * configurationJson)
    {
        NSError *jsonError;
        NSData *configurationJsonData = [NSSTRING(configurationJson) dataUsingEncoding: NSUTF8StringEncoding];
        NSDictionary * configurationDict = [NSJSONSerialization JSONObjectWithData:configurationJsonData
                                              options:NSJSONReadingMutableContainers
                                                error:&jsonError];
        if ( jsonError == nullptr && configurationDict ) {
            
            CSKLogLevel minLogLevel = CSKLogLevelError;
            
            if ([[configurationDict allKeys] containsObject: @"MinLogLevel"]) {
                int minLogLevelInt = [configurationDict[@"MinLogLevel"] integerValue];
                if ( minLogLevelInt == 0) {
                    minLogLevel = CSKLogLevelDebug;
                }
                else if ( minLogLevelInt == 1) {
                    minLogLevel = CSKLogLevelInfo;
                }
                else if ( minLogLevelInt == 2) {
                    minLogLevel = CSKLogLevelWarning;
                }
                else if ( minLogLevelInt == 3) {
                    minLogLevel = CSKLogLevelError;
                }
            }
            
            CSKSdkConfiguration * configuration = [[CSKSdkConfiguration alloc] init];
            configuration.minLogLevel = minLogLevel;
            
            [CSKSdk.sharedInstance setConfiguration: configuration];
        }
    }

    char * _CSGetEnvironment()
    {
        const char * environmentStr = "PRODUCTION";

        CSKEnvironment environment = [CSKSdk.sharedInstance environment];

        if (environment == CSKEnvironmentStaging) {
            environmentStr = "STAGING";
        } else if (environment == CSKEnvironmentDevelopment) {
            environmentStr = "DEVELOPMENT";
        }
        
        char* environmentStrCopy = (char*)malloc(strlen(environmentStr) + 1);
        strcpy(environmentStrCopy, environmentStr);

        return environmentStrCopy;
    }
    
    extern bool _CSHandleDeeplink(const char * deeplink)
    {
        NSURL * deeplinkUrl = [NSURL URLWithString: NSSTRING(deeplink)];
        
        return [CSKSdk.sharedInstance handleDeepLink: deeplinkUrl];
    }
}
