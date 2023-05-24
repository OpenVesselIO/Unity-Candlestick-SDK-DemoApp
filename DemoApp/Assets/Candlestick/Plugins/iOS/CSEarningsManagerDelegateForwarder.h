//
//  CSEarningsManagerDelegateForwarder.h
//  Unity-iPhone
//

#import <Foundation/Foundation.h>

#import "CSSdkPluginUtils.h"

#ifdef __cplusplus
extern "C" {
#endif

static const char * const kCallbacksObjectName = "CSEarningsManagerCallbacks";

static void earnings_send_message(const char *method, NSDictionary *json)
{
    cs_unity_send_message(kCallbacksObjectName, method, json);
}

#ifdef __cplusplus
}
#endif

NS_ASSUME_NONNULL_BEGIN

@interface CSEarningsManagerDelegateForwarder : NSObject

- (void)attachDelegate;

+ (instancetype)sharedInstance;

- (instancetype)init NS_UNAVAILABLE;

@end

NS_ASSUME_NONNULL_END
