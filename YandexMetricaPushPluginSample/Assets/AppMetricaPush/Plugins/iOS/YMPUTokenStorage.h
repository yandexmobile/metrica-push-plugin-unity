/*
 * Version for Unity
 * © 2022 YANDEX
 * You may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * https://yandex.com/legal/appmetrica_sdk_agreement/
 */

#import <Foundation/Foundation.h>

@interface YMPUTokenStorage : NSObject

+ (void)saveToken:(NSData *)token;
+ (NSData *)getToken;

@end
