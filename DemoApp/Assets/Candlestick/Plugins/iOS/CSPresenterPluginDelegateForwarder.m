//
//  CSSdkPluginCallbackForwarder.m
//  Unity-iPhone
//
//  Created by Basil Shikin on 11/10/21.
//

#import "CSPresenterPluginDelegateForwarder.h"

#import "CSSdkPluginUtils.h"
#import <Candlestick/Candlestick.h>

#ifdef __cplusplus
extern "C" {
#endif
    static const char * const kCallbacksClassName = "CSPresenterCallbacks";

    // UnityAppController.mm
    UIViewController* UnityGetGLViewController(void);
    UIWindow* UnityGetMainWindow(void);
    
    // life cycle management
    void UnityPause(int pause);
    void UnitySendMessage(const char* obj, const char* method, const char* msg);
#ifdef __cplusplus
}
#endif


@interface CSPresenterPluginDelegateForwarder() <CSKPresentationControllerDelegate>
@end

@implementation CSPresenterPluginDelegateForwarder

- (instancetype)init
{
    if ( self = [super init] )
    {
        // do nothing
    }
    
    return self;
}

- (void)attachDelegate
{
    CSKSdk.sharedInstance.presentationController.delegate = self;
}

- (void)presentationControllerWillPresentPortal:(CSKPresentationController *)presentationController
{
    UnitySendMessage(kCallbacksClassName, "ForwardOnPortalShowEvent", "");
}

- (void)presentationControllerWillDismissPortal:(CSKPresentationController *)presentationController
{
    UnitySendMessage(kCallbacksClassName, "ForwardOnPortalDismissEvent", "");
}

+ (CSPresenterPluginDelegateForwarder *)sharedInstance
{
    static dispatch_once_t token;
    static CSPresenterPluginDelegateForwarder *shared;
    dispatch_once(&token, ^{
        shared = [[CSPresenterPluginDelegateForwarder alloc] init];
    });
    return shared;
}


@end
