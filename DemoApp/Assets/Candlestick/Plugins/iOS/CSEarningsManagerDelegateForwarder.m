//
//  CSEarningsManagerDelegateForwarder.m
//  Unity-iPhone
//

#import <Candlestick/Candlestick.h>

#import "CSEarningsManagerDelegateForwarder.h"

@interface CSEarningsManagerDelegateForwarder() <CSKEarningsManagerDelegate>
@end

@implementation CSEarningsManagerDelegateForwarder

+ (CSEarningsManagerDelegateForwarder *)sharedInstance
{
    static dispatch_once_t token;
    static CSEarningsManagerDelegateForwarder *shared;
    dispatch_once(&token, ^{
        shared = [[CSEarningsManagerDelegateForwarder alloc] init];
    });
    return shared;
}

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
    [CSKSdk.sharedInstance.earningsManager setDelegate: self
                                         delegateQueue: dispatch_get_main_queue()];
}

- (void)earningsManagerDidUpdateExperimentUserInfo:(CSKEarningsManager *)earningsManager
{
    earnings_send_message("ForwardOnExperimentInfoUpdatedEvent", @{});
}

@end
