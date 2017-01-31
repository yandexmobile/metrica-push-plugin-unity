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
