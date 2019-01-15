using System;
using BeatThat.GetComponentsExt;
using BeatThat.Pools;
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
						var added = onGUITarget.AddIfMissing (ct);

                        using (var allComps = ListPool<Component>.Get())
                        {
                            onGUITarget.GetComponents<Component>(allComps);
                            var afterIndex = allComps.FindIndex(x => x == onGUITarget);
                            if(afterIndex != -1) {
                                var curIndex = allComps.FindIndex(x => x.GetType() == ct);
                                if (curIndex > afterIndex + 1)
                                {
                                    for (; curIndex > afterIndex + 1; curIndex--)
                                    {
                                        UnityEditorInternal.ComponentUtility.MoveComponentUp(added);
                                    }
                                }
                                else if (curIndex != -1 && curIndex < afterIndex + 1)
                                {
                                    for (; curIndex < afterIndex + 1; curIndex++)
                                    {
                                        UnityEditorInternal.ComponentUtility.MoveComponentDown(added);
                                    }
                                }
                            }
                        }

					}
					GUI.backgroundColor = bkgColorSave;
				}
			}
		}
	}
}




