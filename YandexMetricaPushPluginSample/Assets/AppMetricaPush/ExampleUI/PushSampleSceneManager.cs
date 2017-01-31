using UnityEngine;
using System.Collections;

public class PushSampleSceneManager : MonoBehaviour
{
    
    private void Start ()
    {
#if UNITY_IPHONE || UNITY_IOS
		UnityEngine.iOS.NotificationServices.RegisterForNotifications (
			UnityEngine.iOS.NotificationType.Alert |
			UnityEngine.iOS.NotificationType.Badge |
			UnityEngine.iOS.NotificationType.Sound, true);
#endif
    }

    private void initGUI ()
    {
        GUI.skin.button.fontSize = 40;
        GUI.skin.textArea.fontSize = 35;
        GUI.skin.label.normal.textColor = Color.black;
        GUI.contentColor = Color.white;
        GUI.skin.label.fontSize = 40;
    }

    private void OnGUI ()
    {
        initGUI ();

        GUILayout.Label ("Token:", GUILayout.Width (Screen.width), GUILayout.Height (50));

        string token = AppMetricaPush.Instance.Token;

        GUILayout.TextArea (token ?? "", GUILayout.Width (Screen.width), GUILayout.Height (Screen.height / 10));

        if (Button ("Exit")) {
            Application.Quit ();
        }
    }

    private bool Button (string title)
    {
        return GUILayout.Button (title, GUILayout.Width (Screen.width), GUILayout.Height (Screen.height / 10));
    }
}
