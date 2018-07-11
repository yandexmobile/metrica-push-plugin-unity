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

    private readonly AndroidJavaClass metricaPushClass = new AndroidJavaClass ("com.yandex.metrica.push.YandexMetricaPush");
    private AndroidJavaObject metricaConfigStorage = null;

    public void Initialize ()
    {
        using (var activityClass = new AndroidJavaClass ("com.unity3d.player.UnityPlayer")) {
            var playerActivityContext = activityClass.GetStatic<AndroidJavaObject> ("currentActivity");

            metricaConfigStorage = new AndroidJavaObject ("com.yandex.metrica.push.plugin.MetricaConfigStorage", playerActivityContext);

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
        if (androidAppMetricaConfig != null) {
            metricaConfigStorage.Call ("saveConfig", androidAppMetricaConfig);
        }
    }

    #endregion

}

#endif
