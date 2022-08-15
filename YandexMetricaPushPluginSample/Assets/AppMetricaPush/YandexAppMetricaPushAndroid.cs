/*
 * Version for Unity
 * © 2017 YANDEX
 * You may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * https://yandex.com/legal/appmetrica_sdk_agreement/
 */

using UnityEngine;
using System.Collections;

#if UNITY_ANDROID

public class YandexMetricaPushAndroid : IYandexMetricaPush
{

    #region IYandexMetricaPush implementation

    private readonly AndroidJavaClass metricaPushClass =
        new AndroidJavaClass ("com.yandex.metrica.push.plugin.YandexMetricaPushWrapper");

    public void Initialize ()
    {
        using (var activityClass = new AndroidJavaClass ("com.unity3d.player.UnityPlayer")) {
            var playerActivityContext = activityClass.GetStatic<AndroidJavaObject> ("currentActivity");

            AppMetrica.Instance.OnActivation += ProcessConfigurationUpdate;

            var config = AppMetrica.Instance.ActivationConfig;
            if (config.HasValue) {
                ProcessConfigurationUpdate (config.Value);
            }

            metricaPushClass.CallStatic ("init", playerActivityContext);
        }
    }

    public string Token {
        get {
            return metricaPushClass.CallStatic<string> ("getToken");
        }
    }

    public void ProcessConfigurationUpdate (YandexAppMetricaConfig config)
    {
        var androidAppMetricaConfig = config.ToAndroidAppMetricaConfig ();
        if (androidAppMetricaConfig == null) return;

        using (var metricaConfigStorageClass =
               new AndroidJavaClass("com.yandex.metrica.push.plugin.AppMetricaConfigStorage")) {
            using (var activityClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer")) {
                var playerActivityContext = activityClass.GetStatic<AndroidJavaObject>("currentActivity");
                var configStr = androidAppMetricaConfig.Call<string>("toJson");
                metricaConfigStorageClass.CallStatic("saveConfig", playerActivityContext, configStr);
            }
        }
    }

    #endregion

}

#endif
