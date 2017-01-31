//
//  YMPYandexMetricaPush.h
//
//  This file is a part of the AppMetrica
//
//  Version for iOS © 2016 YANDEX
//
//  You may not use this file except in compliance with the License.
//  You may obtain a copy of the License at http://legal.yandex.com/metrica_termsofuse/
//

#import <Foundation/Foundation.h>

NS_ASSUME_NONNULL_BEGIN

@interface YMPYandexMetricaPush : NSObject

/** Setting push notification device token.
 If value is nil, previously set device token is revoked.
 Should be called after AppMetrica initialization.

 @param data Device token data.
 */
+ (void)setDeviceTokenFromData:(NSData *)data;

/** Handling push notification from application launch options.
 Should be called after AppMetrica initialization.

 @param launchOptions A dictionary that contains information related to the 
 application launch options, potentially including a notification info.
 */
+ (void)handleApplicationDidFinishLaunchingWithOptions:(nullable NSDictionary *)launchOptions;

/** Handling push notification event.
 Should be called after AppMetrica initialization.

 @param userInfo A dictionary that contains information related to the remote notification,
 potentially including a badge number for the app icon, an alert sound,
 an alert message to display to the user, a notification identifier
 and custom data.
 */
+ (void)handleRemoteNotification:(NSDictionary *)userInfo;

/** Returning user data string from push notification payload.

 @param userInfo A dictionary that contains information related to the remote notification,
 potentially including a badge number for the app icon, an alert sound,
 an alert message to display to the user, a notification identifier
 and custom data.
 @return A string with custom user data.
 */
+ (nullable NSString *)userDataForNotification:(NSDictionary *)userInfo;

@end

NS_ASSUME_NONNULL_END
