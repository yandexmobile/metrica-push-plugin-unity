/*
 * Version for Unity
 * © 2017 YANDEX
 * You may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * https://yandex.com/legal/appmetrica_sdk_agreement/
 */

using UnityEngine;
using System.Collections;

public class YandexAppMetricaPushDummy : IYandexMetricaPush
{

    #region IYandexMetricaPush implementation

    public void Initialize ()
    {
    }

    public string Token {
        get {
            return null;
        }
    }

    #endregion

}
