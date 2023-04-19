//
//  CSAppConnectManagerDelegateForwarder.m
//  Unity-iPhone
//
//  Created by Basil Shikin on 11/10/21.
//

#import "CSAppConnectManagerDelegateForwarder.h"


#import "CSSdkPluginUtils.h"
#import <Candlestick/Candlestick.h>

#ifdef __cplusplus
extern "C" {
#endif
    // UnityAppController.mm
    UIViewController* UnityGetGLViewController(void);
    UIWindow* UnityGetMainWindow(void);
    
    // life cycle management
    void UnityPause(int pause);
    void UnitySendMessage(const char* obj, const char* method, const char* msg);
#ifdef __cplusplus
}
#endif

@interface CSAppConnectManagerDelegateForwarder()<CSKAppConnectManagerDelegate>

+ (NSString *)jsonStringFromState:(CSKAppConnectState *)state;
+ (NSString *)unityStringFromStatus:(CSKAppConnectStatus)status;

@end

@implementation CSAppConnectManagerDelegateForwarder

- (instancetype)init
{
    self = [super init];
    if ( self )
    {
    }
    
    return self;
}

- (void)attachDelegate
{
    [CSKSdk.sharedInstance.appConnectManager setDelegate: self
                                           delegateQueue: dispatch_get_main_queue()];
}

- (void)appConnectManagerDidUpdateState:(CSKAppConnectManager *)appConnectManager
{
    NSString * stateJson = [CSAppConnectManagerDelegateForwarder jsonStringFromState:appConnectManager.state];
    UnitySendMessage("CSAppConnectManagerCallbacks", "ForwardOnStateUpdatedEvent", stateJson.UTF8String);
}

+ (NSString *)jsonStringFromState:(CSKAppConnectState *)state
{
    NSDictionary * dictionary = @{
        @"status": [self unityStringFromStatus: state.status],
        @"userId": (state.userId ?: @""),
        @"accessToken": (state.accessToken ?: @"")
    };
    
    NSError *error;
    NSData *jsonData = [NSJSONSerialization dataWithJSONObject:dictionary
                                                       options:0
                                                         error:&error];
    NSString *jsonString;
    if (! jsonData) {
        jsonString = @"{\"status\": \"Error\"}";
    } else {
        jsonString = [[NSString alloc] initWithData:jsonData encoding:NSUTF8StringEncoding];
    }

    return jsonString;
}

+ (NSString *)unityStringFromStatus:(CSKAppConnectStatus)status
{
    if ( status == CSKAppConnectStatusNotInitialized )
    {
        return @"NotInitialized";
    }
    else if ( status == CSKAppConnectStatusConnected )
    {
        return @"Connected";
    }
    else
    {
        return @"Disconnected";
    }
}

+ (CSAppConnectManagerDelegateForwarder *)sharedInstance
{
    static dispatch_once_t token;
    static CSAppConnectManagerDelegateForwarder *shared;
    dispatch_once(&token, ^{
        shared = [[CSAppConnectManagerDelegateForwarder alloc] init];
    });
    return shared;
}


@end
