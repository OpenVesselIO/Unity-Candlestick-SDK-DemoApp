// Copyright © 2021 OpenVessel. All rights reserved.

#import <Candlestick/Candlestick.h>

#import "CSSdkPluginUtils.h"
#import "CSSdkPluginDelegateForwarder.h"
#import "CSAppConnectManagerDelegateForwarder.h"
#import "CSPresenterPluginDelegateForwarder.h"

extern "C" {
        
    void _CSInitialize(const char * userId)
    {
        [CSSdkPluginDelegateForwarder.sharedInstance attachDelegate];
        [CSAppConnectManagerDelegateForwarder.sharedInstance attachDelegate];
        [CSPresenterPluginDelegateForwarder.sharedInstance attachDelegate];
        
        [CSKSdk.sharedInstance startWithUserId: NSSTRING(userId)];
    }

    void _CSSetEnvironment(const char * environment)
    {
        if (strcmp("PRODUCTION", environment) == 0) {
            [CSKSdk.sharedInstance setEnvironment: CSKEnvironmentProduction];
        } else if (strcmp("TESTING", environment) == 0) {
            [CSKSdk.sharedInstance setEnvironment: CSKEnvironmentTesting];
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
            NSURL * callbackUrl = NULL;
            
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
            
            if ([configurationDict objectForKey: @"CallbackUrl"]) {
                callbackUrl = [NSURL URLWithString:[configurationDict objectForKey: @"CallbackUrl"]];
            }
            
            CSKSdkConfiguration * configuration = [[CSKSdkConfiguration alloc] init];
            configuration.minLogLevel = minLogLevel;
            configuration.callbackUrl = callbackUrl;
            
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
