/*
 * Version for Flutter
 * © 2022 YANDEX
 * You may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * https://yandex.com/legal/appmetrica_sdk_agreement/
 */

#import "YMPUUtils.h"

@implementation YMPUUtils

+ (NSString *)stringForTokenData:(NSData *)deviceToken
{
    if (deviceToken.length == 0) {
        return nil;
    }

    const char *bytes = [deviceToken bytes];
    NSMutableString *token = [NSMutableString string];
    for (NSUInteger i = 0; i < deviceToken.length; ++i) {
        [token appendFormat:@"%02.2hhx", bytes[i]];
    }
    return [token copy];
}

@end
