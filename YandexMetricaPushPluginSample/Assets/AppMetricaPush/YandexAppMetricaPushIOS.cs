/*
 * Version for Unity
 * © 2017 YANDEX
 * You may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * https://yandex.com/legal/appmetrica_sdk_agreement/
 */

using System.Runtime.InteropServices;

#if UNITY_IPHONE || UNITY_IOS

public class YandexMetricaPushIOS : IYandexMetricaPush
{
    [DllImport ("__Internal")]
    private static extern void ymp_saveActivationConfigurationJSON (string configurationJSON);

    [DllImport ("__Internal")]
    private static extern string ymp_getToken ();

    #region IYandexMetricaPush implementation

    public void Initialize ()
    {
        AppMetrica.Instance.OnActivation += ProcessConfigurationUpdate;
        var config = AppMetrica.Instance.ActivationConfig;
        if (config.HasValue) {
            ProcessConfigurationUpdate (config.Value);
        }
    }

    public string Token {
        get {
            return ymp_getToken();
        }
    }

    private void ProcessConfigurationUpdate(YandexAppMetricaConfig config)
    {
        // Yandex AppMetrica Unity Plugin JSON Utils are used here.
        ymp_saveActivationConfigurationJSON(YMMJSONUtils.JSONEncoder.Encode(config.ToHashtable()));
    }

    #endregion

}

#endif
