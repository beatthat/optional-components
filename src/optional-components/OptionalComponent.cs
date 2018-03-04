using System;
using UnityEngine;

namespace BeatThat.OptionalComponents
{
	/// <summary>
	/// Sometimes your Component could use an attribute to add a helper component,
	/// but you want to make it optional (at least possible to disable)
	/// and so Unity's RequireComponent isn't the thing.
	/// </summary>
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
	public class OptionalComponentAttribute : Attribute 
	{
		public OptionalComponentAttribute(Type componentType) 
		{
			this.componentType = componentType;
		}

		public Type componentType { get; private set; }

		/// <summary>
		/// Checks if the passed in component's class has [OptionalComponent] attrs
		/// and if any found, ensures the components are present as siblings
		/// </summary>
		/// <returns>The any defined.</returns>
		/// <param name="c">C.</param>
		/// <returns>the number of components added</returns>
		public static int EnsureAll(Component c)
		{
			var attrs = c.GetType ().GetCustomAttributes (typeof(OptionalComponentAttribute), true);
			if (attrs == null || attrs.Length == 0) {
				return 0;
			}

			var n = 0;
			foreach (var a in attrs) {
				var curAttr = a as OptionalComponentAttribute;
				if (curAttr == null) {
					continue;
				}
				if (curAttr.componentType == null) {
					#if UNITY_EDITOR || DEBUG_UNSTRIP
					Debug.LogWarning("No component type set on OptionalComponent attr for class " + c.GetType());
					continue;
					#endif
				}

				if (!typeof(Component).IsAssignableFrom (curAttr.componentType)) {
					#if UNITY_EDITOR || DEBUG_UNSTRIP
					Debug.LogWarning("No component type is not a Component on OptionalComponent attr for class " 
						+ c.GetType() + " (componentType=" + curAttr.componentType + ")");
					continue;
					#endif
				}

				c.AddIfMissing (curAttr.componentType);
				n++;
			}

			return n;
		}
	}

	public static class Ext
	{
		/// <summary>
		/// Checks if the passed in component's class has [OptionalComponent] attrs
		/// and if any found, ensures the components are present as siblings
		/// </summary>
		/// <returns>The any defined.</returns>
		/// <param name="c">C.</param>
		/// <returns>the number of components added</returns>
		public static int EnsureAllOptionalComponents(this Component c)
		{
			return OptionalComponentAttribute.EnsureAll (c);
		}
	}
}
