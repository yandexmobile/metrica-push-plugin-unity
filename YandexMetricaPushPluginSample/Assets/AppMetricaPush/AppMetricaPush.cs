/*
 * Version for Unity
 * © 2017 YANDEX
 * You may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * https://yandex.com/legal/appmetrica_sdk_agreement/
 */

using UnityEngine;
using System.Collections;

public class AppMetricaPush : MonoBehaviour
{
    public const string VERSION = "1.1.0";
    
    private static bool _isInitialized = false;

    private static IYandexMetricaPush _instance = null;
    private static object syncRoot = new Object ();

    public static IYandexMetricaPush Instance {
        get {
            if (_instance == null) {
                lock (syncRoot) {
                    if (_instance == null) {
                        if (Application.platform == RuntimePlatform.IPhonePlayer) {
#if UNITY_IPHONE || UNITY_IOS
							_instance = new YandexMetricaPushIOS();
#endif
                        } else if (Application.platform == RuntimePlatform.Android) {
#if UNITY_ANDROID
                            _instance = new YandexMetricaPushAndroid ();
#endif
                        }

                        if (_instance == null) {
                            _instance = new YandexAppMetricaPushDummy ();
                        }
                    }
                }
            }
            return _instance;
        }
    }

    private void Start ()
    {
        if (!_isInitialized) {
            _isInitialized = true;
            DontDestroyOnLoad (this.gameObject);
            Instance.Initialize ();
        }
    }

    private void Awake ()
    {
        if (_isInitialized) {
            Destroy (this.gameObject);
        }
    }

}
