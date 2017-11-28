/*
 * Version for Unity
 * © 2017 YANDEX
 * You may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * https://yandex.com/legal/appmetrica_sdk_agreement/
 */

using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.iOS.Xcode;
using System.IO;
using System.Collections;

/// <summary>
/// Postprocess build player for AppMetrica Push.
/// See https://bitbucket.org/Unity-Technologies/iosnativecodesamples/src/ae6a0a2c02363d35f954d244a6eec91c0e0bf194/NativeIntegration/Misc/UpdateXcodeProject/
/// </summary>

public class PostprocessBuildPlayerAppMetricaPush
{
	private static readonly string[] WeakFrameworks = {
		"UserNotifications"
	};

	[PostProcessBuild]
	public static void OnPostprocessBuild (BuildTarget buildTarget, string path)
	{
		if (buildTarget == BuildTarget.iOS) {
			var projectPath = path + "/Unity-iPhone.xcodeproj/project.pbxproj";

			var project = new PBXProject ();
			project.ReadFromString (File.ReadAllText (projectPath));

			var target = project.TargetGuidByName ("Unity-iPhone");

			foreach (var frameworkName in WeakFrameworks) {
				project.AddFrameworkToProject (target, frameworkName + ".framework", true);
			}

			File.WriteAllText (projectPath, project.WriteToString ());
		}
	}
}
