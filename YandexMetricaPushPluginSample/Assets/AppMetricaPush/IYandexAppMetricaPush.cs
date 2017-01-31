using UnityEngine;
using System.Collections;

public interface IYandexMetricaPush
{
    void Initialize ();

    string Token { get; }
}
