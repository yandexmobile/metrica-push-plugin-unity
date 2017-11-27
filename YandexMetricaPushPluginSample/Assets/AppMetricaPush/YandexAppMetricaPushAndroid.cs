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

    private AndroidJavaClass metricaPushClass = null;
    private AndroidJavaObject metricaConfigStorageClass = null;

    public void Initialize ()
    {
        using (var activityClass = new AndroidJavaClass ("com.unity3d.player.UnityPlayer")) {
            var playerActivityContext = activityClass.GetStatic<AndroidJavaObject> ("currentActivity");

            metricaConfigStorageClass = new AndroidJavaObject ("com.yandex.metrica.push.unity.MetricaConfigStorage", playerActivityContext);

            AppMetrica.Instance.OnActivation += ProcessConfigurationUpdate;

            var config = AppMetrica.Instance.ActivationConfig;
            if (config.HasValue) {
                ProcessConfigurationUpdate (config.Value);
            }

            metricaPushClass = new AndroidJavaClass ("com.yandex.metrica.push.YandexMetricaPush");
            metricaPushClass.CallStatic ("init", playerActivityContext);
        }
    }

    public string Token {
        get {
            if (metricaPushClass != null) {
                return metricaPushClass.CallStatic<string> ("getToken");
            }
            return null;
        }
    }

    public void ProcessConfigurationUpdate (YandexAppMetricaConfig config)
    {
        using (var metricaClass = new AndroidJavaClass ("com.yandex.metrica.YandexMetrica")) {
            var androidAppMetricaConfig = config.ToAndroidAppMetricaConfig (metricaClass);
            if (androidAppMetricaConfig != null) {
                metricaConfigStorageClass.Call ("saveConfig", androidAppMetricaConfig);
            }
        }
    }

    #endregion

}

#endif
