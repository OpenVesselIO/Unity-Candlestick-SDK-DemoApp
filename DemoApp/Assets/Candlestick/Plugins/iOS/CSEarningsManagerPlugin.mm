// Copyright © 2021 OpenVessel. All rights reserved.

#import <Candlestick/Candlestick.h>

#import "CSEarningsManagerDelegateForwarder.h"

extern "C" {

    static void earnings_send_auth_error(NSString *msg)
    {
        UnitySendMessage(kCallbacksObjectName, "ForwardOnAuthFailureEvent", msg.UTF8String);
    }

    static void earnings_send_verification_error(NSString *msg)
    {
        UnitySendMessage(kCallbacksObjectName, "ForwardOnVerificationFailureEvent", msg.UTF8String);
    }

    static void earnings_send_verification_success()
    {
        UnitySendMessage(kCallbacksObjectName, "ForwardOnVerificationSuccessEvent", "");
    }

    void _CSTrackRevenuedAd(const char * adTypeCString)
    {
        CSKAdType adType;

        if (strcmp("APPOPEN", adTypeCString) == 0) {
            adType = CSKAdTypeAppOpen;
        }
        else if (strcmp("BANNER", adTypeCString) == 0) {
            adType = CSKAdTypeBanner;
        }
        else if (strcmp("INTERSTITIAL", adTypeCString) == 0) {
            adType = CSKAdTypeInterstitial;
        }
        else if (strcmp("MREC", adTypeCString) == 0) {
            adType = CSKAdTypeMREC;
        }
        else if (strcmp("REWARDED", adTypeCString) == 0) {
            adType = CSKAdTypeRewarded;
        }
        else {
            return;
        }

        [CSKSdk.sharedInstance.earningsManager trackRevenuedAd:adType];
    }

    void _CSTrackImpression(const char * triggerName)
    {
        [CSKSdk.sharedInstance.earningsManager trackImpressionWithTriggerName:NSSTRING(triggerName)];
    }

    void _CSShowEarnings(const char * settingsJson)
    {
        NSData *settingsJsonData = [NSSTRING(settingsJson) dataUsingEncoding:NSUTF8StringEncoding];

        NSError *error;
        NSDictionary *settingsDict = [NSJSONSerialization JSONObjectWithData: settingsJsonData
                                                                     options: kNilOptions
                                                                       error: &error];

        if (settingsDict == nil) {
            return;
        }

        NSString *promoTypeString = settingsDict[@"promoType"];

        CSKEarningsPromoType promoType;

        if ([@"STATIC" isEqualToString:promoTypeString]) {
            promoType = CSKEarningsPromoTypeStatic;
        }
        else if ([@"VIDEO" isEqualToString:promoTypeString]) {
            promoType = CSKEarningsPromoTypeVideo;
        }
        else {
            return;
        }

        CSKEarningsPresentationSettings *settings;
        settings = [CSKEarningsPresentationSettings settingsWithUserId:settingsDict[@"userId"]];
        settings.promoType = promoType;
        settings.triggerName = settingsDict[@"triggerName"];

        [CSKSdk.sharedInstance.earningsManager presentEarningsFromViewController: UNITY_VIEW_CONTROLLER
                                                                    withSettings: settings
                                                                        animated: YES];
    }

    void _CSGenerateAuthCodeForPhoneNumber(const char * cPhoneNumber)
    {
        NSString *phoneNumber = NSSTRING(cPhoneNumber);

        [CSKSdk.sharedInstance.earningsManager generateAuthCodeForPhoneNumber:phoneNumber resultHandler:
         ^(CSKEarningsAuthCodeMetadata * _Nullable authCodeMetadata, NSError * _Nullable error) {
            if (authCodeMetadata) {
                earnings_send_message("ForwardOnAuthCodeMetadataEvent", @{
                    @"phoneNumber": phoneNumber,
                    @"createdAt": @(authCodeMetadata.createdAt),
                    @"expiresAt": @(authCodeMetadata.expiresAt),
                    @"ttl": @(authCodeMetadata.ttl),
                });
            } else {
                earnings_send_auth_error(error.localizedDescription);
            }
        }];
    }

    void _CSLoginByPhoneAuthCode(const char * json)
    {
        NSData *jsonData = [NSSTRING(json) dataUsingEncoding:NSUTF8StringEncoding];

        NSError *error;
        NSDictionary *dict = [NSJSONSerialization JSONObjectWithData: jsonData
                                                             options: kNilOptions
                                                               error: &error];

        if (dict == nil) {
            return;
        }

        [CSKSdk.sharedInstance.earningsManager
         loginWithPhoneNumber:dict[@"phoneNumber"]
         code:dict[@"code"]
         codeCreatedAt:[dict[@"codeCreatedAt"] longLongValue]
         userId:dict[@"userId"]
         completionHandler:^(NSError * _Nullable error) {
            if (error) {
                earnings_send_auth_error(error.localizedDescription);
            }
        }];
    }

    void _CSGenerateVerificationCodeForEmail(const char * cEmail)
    {
        NSString *email = NSSTRING(cEmail);

        [CSKSdk.sharedInstance.earningsManager generateVerificationCodeForEmail:email resultHandler:
         ^(CSKEarningsVerificationCodeMetadata * _Nullable codeMetadata, NSError * _Nullable error) {
            if (codeMetadata) {
                earnings_send_message("ForwardOnVerificationCodeMetadataEvent", @{
                    @"email": email,
                    @"createdAt": @(codeMetadata.createdAt),
                    @"expiresAt": @(codeMetadata.expiresAt),
                    @"ttl": @(codeMetadata.ttl),
                });
            } else {
                earnings_send_auth_error(error.localizedDescription);
            }
        }];
    }

    void _CSVerifyEmail(const char * json)
    {
        NSData *jsonData = [NSSTRING(json) dataUsingEncoding:NSUTF8StringEncoding];

        NSError *error;
        NSDictionary *dict = [NSJSONSerialization JSONObjectWithData: jsonData
                                                             options: kNilOptions
                                                               error: &error];

        if (dict == nil) {
            return;
        }

        [CSKSdk.sharedInstance.earningsManager
         verifyEmail:dict[@"email"]
         code:dict[@"code"]
         codeCreatedAt:[dict[@"codeCreatedAt"] longLongValue]
         completionHandler:^(NSError * _Nullable error) {
            if (error) {
                earnings_send_verification_error(error.localizedDescription);
            } else {
                earnings_send_verification_success();
            }
        }];
    }

    const char * _CSGetEarningsExperimentUserInfo()
    {
        CSKEarningsExperimentUserInfo *experimentUserInfo = CSKSdk.sharedInstance.earningsManager.experimentUserInfo;
        NSDictionary<NSString *, id> *extras = experimentUserInfo.extras;

        NSString *extrasJsonString;

        if (extras) {
            NSError *error;
            NSData *extrasJsonData = [NSJSONSerialization dataWithJSONObject: extras
                                                                     options: kNilOptions
                                                                       error: &error];

            extrasJsonString = [[NSString alloc] initWithData:extrasJsonData encoding:NSUTF8StringEncoding];
        }

        NSDictionary *experimentUserInfoDict = @{
            @"installationId": experimentUserInfo.installationId,
            @"extras": extrasJsonString ?: NSNull.null,
        };

        NSError *error;
        NSData *experimentUserInfoJsonData = [NSJSONSerialization dataWithJSONObject: experimentUserInfoDict
                                                                             options: kNilOptions
                                                                               error: &error];

        if (experimentUserInfoJsonData == nil) {
            return NULL;
        }

        void *result = malloc((size_t)experimentUserInfoJsonData.length);

        [experimentUserInfoJsonData getBytes:result length:experimentUserInfoJsonData.length];

        return (const char *)result;
    }
    
}
