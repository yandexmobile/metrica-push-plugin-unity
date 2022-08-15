/*
 * Version for Unity
 * © 2022 YANDEX
 * You may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * https://yandex.com/legal/appmetrica_sdk_agreement/
 */

#import "YMPUTokenStorage.h"

@implementation YMPUTokenStorage

NSString *const kYMPUUserDefaultsTokenKey = @"com.yandex.mobile.metrica.push.unity.PushToken";

+ (void)saveToken:(NSData *)token
{
    [[NSUserDefaults standardUserDefaults] setObject:token forKey:kYMPUUserDefaultsTokenKey];
}

+ (NSData *)getToken
{
    return [[NSUserDefaults standardUserDefaults] dataForKey:kYMPUUserDefaultsTokenKey];
}

@end
