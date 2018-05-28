using System;
using UnityEngine;

namespace BeatThat.OptionalComponents
{
    public static class EditorExt
	{
		public static void OnGUIProposeAddOptionalComponents(this Component onGUITarget)
		{

			var bkgColorSave = GUI.backgroundColor;

			using (var optionalCompTypes = ListPool<Type>.Get ()) {
				onGUITarget.GetType().GetOptionalComponentTypes (optionalCompTypes);
				foreach (var ct in optionalCompTypes) {
					if (onGUITarget.GetComponent (ct) != null) {
						continue; 
					}

					GUI.backgroundColor = Color.cyan;
					if (GUILayout.Button (new GUIContent("Add a " + ct.Name, ct.Name + " is specified as an OptionalComponent for " + onGUITarget.GetType().Name))) {
						onGUITarget.AddIfMissing (ct);
					}
					GUI.backgroundColor = bkgColorSave;
				}
			}
		}
	}
}
