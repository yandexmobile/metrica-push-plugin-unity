/*
 * Version for Unity
 * © 2017 YANDEX
 * You may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * https://yandex.com/legal/appmetrica_sdk_agreement/
 */

#import <Foundation/Foundation.h>
#import <YandexMobileMetrica/YandexMobileMetrica.h>
#import <YandexMobileMetricaPush/YandexMobileMetricaPush.h>

#import "UnityAppController.h"
#import "YMMBridge.h"
#import "YMPUTokenStorage.h"
#import "YMPUUtils.h"
#import <objc/runtime.h>

// TODO: Describe UIApplicationDelegate hijacking here.

static NSString *const kYMPUserDefaultsConfigurationKey = @"com.yandex.mobile.metrica.push.sdk.Configuration";

static void ymp_swizleApplicationDelegate();
static bool ymp_ensureAppMetricaActivated();

__attribute__((constructor))
static void ymp_initializeAppMetricaPushPlugin()
{
    ymp_swizleApplicationDelegate();
}

@implementation UnityAppController (YMPBridge)

#define RECURSION_CHECK(CMD) if ([NSStringFromSelector(_cmd) rangeOfString:@"ymp_"].location == 0) { CMD; }

- (BOOL)ymp_application:(UIApplication *)application didFinishLaunchingWithOptions:(NSDictionary *)launchOptions
{
    RECURSION_CHECK(return YES);

    // Call the original method
    BOOL result = [self ymp_application:application didFinishLaunchingWithOptions:launchOptions];

    if (launchOptions[UIApplicationLaunchOptionsRemoteNotificationKey] != nil) {
        // We should activate AppMetrica first to handle push notification
        ymp_ensureAppMetricaActivated();
    }

    // Enable in-app push notifications handling in iOS 10
    if ([UNUserNotificationCenter class] != nil) {
        id<YMPUserNotificationCenterDelegate> delegate = [YMPYandexMetricaPush userNotificationCenterDelegate];
        delegate.nextDelegate = [UNUserNotificationCenter currentNotificationCenter].delegate;
        [UNUserNotificationCenter currentNotificationCenter].delegate = delegate;
    }

    [YMPYandexMetricaPush handleApplicationDidFinishLaunchingWithOptions:launchOptions];

    return result;
}

- (void)ymp_application:(UIApplication *)application didRegisterForRemoteNotificationsWithDeviceToken:(NSData *)deviceToken
{
    RECURSION_CHECK(return);

    if (ymp_ensureAppMetricaActivated()) {
        // We have to ensure that AppMetrica activated here
        [YMPYandexMetricaPush setDeviceTokenFromData:deviceToken];
    }
    [YMPUTokenStorage saveToken:deviceToken];

    // Call the original method
    [self ymp_application:application didRegisterForRemoteNotificationsWithDeviceToken:deviceToken];
}

- (void)ymp_application:(UIApplication *)application didReceiveRemoteNotification:(NSDictionary *)userInfo
{
    RECURSION_CHECK(return);

    [self ymp_handleRemoteNotification:userInfo];

    // Call the original method
    [self ymp_application:application didReceiveRemoteNotification:userInfo];
}

- (void)ymp_application:(UIApplication *)application
    didReceiveRemoteNotification:(NSDictionary *)userInfo
    fetchCompletionHandler:(void (^)(UIBackgroundFetchResult))completionHandler
{
    RECURSION_CHECK(return);

    [self ymp_handleRemoteNotification:userInfo];

    // Call the original method
    [self ymp_application:application didReceiveRemoteNotification:userInfo fetchCompletionHandler:completionHandler];
}

- (void)ymp_handleRemoteNotification:(NSDictionary *)userInfo
{
    // We should activate AppMetrica first to handle push notification
    ymp_ensureAppMetricaActivated();
    [YMPYandexMetricaPush handleRemoteNotification:userInfo];
}

#undef RECURSION_CHECK

@end

static void ymp_appdelegateMethodsSwap(SEL origSel, SEL ympSel)
{
    Class cls = [UnityAppController class];
    Method origMethod = class_getInstanceMethod(cls, origSel);
    Method ympMethod = class_getInstanceMethod(cls, ympSel);

    if (origMethod != NULL) {
        method_exchangeImplementations(origMethod, ympMethod);
    }
    else {
        const char *types = method_getTypeEncoding(ympMethod);
        IMP ympImp = method_getImplementation(ympMethod);
        if (ympImp != NULL) {
            class_addMethod(cls, origSel, ympImp, types);
        }
    }
}

static void ymp_swizleApplicationDelegate()
{
    #pragma clang diagnostic push
    #pragma clang diagnostic error "-Wundeclared-selector"

    #define SWAP_APPDELEGATE_METHODS(SEL) ymp_appdelegateMethodsSwap(@selector(SEL), @selector(ymp_ ## SEL))

    SWAP_APPDELEGATE_METHODS(application:didFinishLaunchingWithOptions:);
    SWAP_APPDELEGATE_METHODS(application:didRegisterForRemoteNotificationsWithDeviceToken:);
    SWAP_APPDELEGATE_METHODS(application:didReceiveRemoteNotification:);
    SWAP_APPDELEGATE_METHODS(application:didReceiveRemoteNotification:fetchCompletionHandler:);

    #undef SWAP_APPDELEGATE_METHODS
    #pragma clang diagnostic pop
}

static bool ymp_ensureAppMetricaActivated()
{
    if (ymm_isAppMetricaActivated()) {
        // AppMetrica is already activated
        return true;
    }

    bool result = false;
    NSString *JSONString = [[NSUserDefaults standardUserDefaults] stringForKey:kYMPUserDefaultsConfigurationKey];
    if (JSONString != nil) {
        const char *cConfigJSON = [JSONString UTF8String];
        char *configJSON = (char *)malloc(strlen(cConfigJSON) + 1);
        strcpy(configJSON, cConfigJSON);

        // Activating AppMetrica with some cached configuration
        ymm_activateWithConfigurationJSON(configJSON);
        result = ymm_isAppMetricaActivated();

        free(configJSON);
    }
    return result;
}

void ymp_saveActivationConfigurationJSON(char *configurationJSON)
{
    if (configurationJSON == nil) {
        return;
    }
    NSString *JSONString = [NSString stringWithUTF8String:configurationJSON];
    if (JSONString == nil) {
        return;
    }

    [[NSUserDefaults standardUserDefaults] setObject:JSONString forKey:kYMPUserDefaultsConfigurationKey];
}

char *ymp_getToken()
{
    NSString *token = [YMPUUtils stringForTokenData:[YMPUTokenStorage getToken]];

    char *result = NULL;
    if (token != NULL) {
        const char *cToken = [token UTF8String];
        result = (char *)malloc(strlen(cToken) + 1);
        strcpy(result, cToken);
    }
    return result;
}

char *ymp_getLibraryVersion()
{
    NSString *version = [[NSString alloc] initWithFormat:@"%d.%d%d", YMP_VERSION_MAJOR, YMP_VERSION_MINOR, YMP_VERSION_PATCH];

    char *result = NULL;
    if (version != NULL) {
        const char *cVersion = [version UTF8String];
        result = (char *)malloc(strlen(cVersion) + 1);
        strcpy(result, cVersion);
    }
    return result;
}
