/*
 *  Version for iOS
 *  © 2016-2018 YANDEX
 *  You may not use this file except in compliance with the License.
 *  You may obtain a copy of the License at
 *  https://yandex.com/legal/appmetrica_sdk_agreement/
 */

#if __IPHONE_OS_VERSION_MAX_ALLOWED >= __IPHONE_10_0
    #define YMP_USER_NOTIFICATIONS_CENTER_AVAILABLE
#endif

#ifdef YMP_USER_NOTIFICATIONS_CENTER_AVAILABLE

#import <UserNotifications/UserNotifications.h>

/** Delegate for handling foreground push notifications on iOS 10+.
 To handle foreground push notifications execute this line before application finished launching:

 [UNUserNotificationCenter currentNotificationCenter].delegate = [YMPYandexMetricaPush userNotificationCenterDelegate];
 */
@protocol YMPUserNotificationCenterDelegate <UNUserNotificationCenterDelegate>

@required

/** Notification presentation options to be passed into completion handler of
 userNotificationCenter:willPresentNotification:withCompletionHandler:

 This delegate calls handler only if nextDelegate property is not set
 or if an object in nextDelegate doesn't respond to the selector above.
 */
@property (nonatomic, assign) UNNotificationPresentationOptions presentationOptions;

/** Delegate to which calls of this protocol will be proxied.
 */
@property (nonatomic, weak, nullable) id<UNUserNotificationCenterDelegate> nextDelegate;

@end

#endif
