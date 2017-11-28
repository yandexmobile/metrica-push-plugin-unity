//
//  YMPYandexMetricaPush.h
//
//  This file is a part of the AppMetrica
//
//  Version for iOS © 2017 YANDEX
//
//  You may not use this file except in compliance with the License.
//  You may obtain a copy of the License at http://legal.yandex.com/metrica_termsofuse/
//

#import <Foundation/Foundation.h>

#if __has_include("YMPYandexMetricaPushEnvironment.h")
    #import "YMPYandexMetricaPushEnvironment.h"
    #import "YMPUserNotificationCenterDelegate.h"
#else
    #import <YandexMobileMetricaPush/YMPYandexMetricaPushEnvironment.h>
    #import <YandexMobileMetricaPush/YMPUserNotificationCenterDelegate.h>
#endif

NS_ASSUME_NONNULL_BEGIN

@interface YMPYandexMetricaPush : NSObject

#ifdef YMP_USER_NOTIFICATIONS_CENTER_AVAILABLE

/** Returning YMPUserNotificationCenterDelegate that handles foreground push notifications on iOS 10+.
 YMPUserNotificationCenterDelegate protocol is derived from UNUserNotificationCenterDelegate.
 To handle foreground push notifications execute this line before application finished launching:

 [UNUserNotificationCenter currentNotificationCenter].delegate = [YMPYandexMetricaPush userNotificationCenterDelegate];

 @return Delegate that implements UNUserNotificationCenterDelegate protocol.
 */
+ (id<YMPUserNotificationCenterDelegate>)userNotificationCenterDelegate;

#endif

/** Setting push notification device token with production environment.
 If value is nil, previously set device token is revoked.
 Should be called after AppMetrica initialization.

 @param data Device token data.
 */
+ (void)setDeviceTokenFromData:(nullable NSData *)data;

/** Setting push notification device token with specific environment.
 If value is nil, previously set device token is revoked.
 Should be called after AppMetrica initialization.

 @param data Device token data.
 @param pushEnvironment Application APNs environment.
 */
+ (void)setDeviceTokenFromData:(nullable NSData *)data pushEnvironment:(YMPYandexMetricaPushEnvironment)pushEnvironment;

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

/** Returning YES if push notification is related to AppMetrica.

 @param userInfo A dictionary that contains information related to the remote notification,
 potentially including a badge number for the app icon, an alert sound,
 an alert message to display to the user, a notification identifier
 and custom data.
 @return YES for SDK related notifications.
 */
+ (BOOL)isNotificationRelatedToSDK:(NSDictionary *)userInfo;

@end

NS_ASSUME_NONNULL_END
