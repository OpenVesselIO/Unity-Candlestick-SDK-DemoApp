// Copyright © 2021 OpenVessel. All rights reserved.

#import <Candlestick/Candlestick.h>

#import "CSEarningsManagerDelegateForwarder.h"

extern "C" {

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

        char *result = (char *)malloc((size_t)experimentUserInfoJsonData.length + 1);
        result[experimentUserInfoJsonData.length] = '\0';

        [experimentUserInfoJsonData getBytes:result length:experimentUserInfoJsonData.length];

        return (const char *)result;
    }
    
}
