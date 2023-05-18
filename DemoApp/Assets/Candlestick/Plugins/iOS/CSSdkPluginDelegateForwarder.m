//
//  CSSdkPluginCallbackForwarder.m
//  Unity-iPhone
//
//  Created by Basil Shikin on 11/10/21.
//

#import "CSSdkPluginDelegateForwarder.h"

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

    static const char * const kCallbacksObjectName = "CSSdkCallbacks";

    static void sdk_send_message(const char *method, NSDictionary *json)
    {
        cs_unity_send_message(kCallbacksObjectName, method, json);
    }
#ifdef __cplusplus
}
#endif


@interface CSSdkPluginDelegateForwarder() <CSKSdkDelegate>
@end

@implementation CSSdkPluginDelegateForwarder

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
    [CSKSdk.sharedInstance setDelegate: self
                         delegateQueue: dispatch_get_main_queue()];
}

- (void)candlestickSdkDidStart:(CSKSdk *)sdk
{
    sdk_send_message("ForwardOnSdkInitializedEvent", @{});
}

- (void)candlestickSdk:(CSKSdk *)sdk didFinishConsentFlowWithUserInfo:(CSKConsentFlowUserInfo *)consentFlowUserInfo
{
    NSError *error;
    NSData *extrasData = [NSJSONSerialization dataWithJSONObject: consentFlowUserInfo.experimentUserInfo.extras
                                                         options: kNilOptions
                                                           error: &error];

    if (extrasData == nil) {
        return;
    }

    sdk_send_message("ForwardOnSdkConsentFlowFinishedEvent", @{
        @"experimentInfo": @{
            @"installationId": consentFlowUserInfo.experimentUserInfo.installationId,
            @"extras": extrasData,
        },
    });
}

+ (CSSdkPluginDelegateForwarder *)sharedInstance
{
    static dispatch_once_t token;
    static CSSdkPluginDelegateForwarder *shared;
    dispatch_once(&token, ^{
        shared = [[CSSdkPluginDelegateForwarder alloc] init];
    });
    return shared;
}


@end
