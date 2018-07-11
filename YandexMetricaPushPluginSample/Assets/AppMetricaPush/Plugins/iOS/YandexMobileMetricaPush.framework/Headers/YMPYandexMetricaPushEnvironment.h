/*
 *  Version for iOS
 *  © 2016-2018 YANDEX
 *  You may not use this file except in compliance with the License.
 *  You may obtain a copy of the License at
 *  https://yandex.com/legal/appmetrica_sdk_agreement/
 */

#ifndef YMPYandexMetricaPushEnvironment_h
#define YMPYandexMetricaPushEnvironment_h

typedef NS_ENUM(NSUInteger, YMPYandexMetricaPushEnvironment) {
    // Environment for production certificates.
    YMPYandexMetricaPushEnvironmentProduction,

    // Environment for production and development(sandbox) certificates.
    YMPYandexMetricaPushEnvironmentDevelopment,
};

#endif /* YMPYandexMetricaPushEnvironment_h */
