using UnityEngine;
using UnityEditor;
using System;

namespace Unapparent {
	public class Inspector<T> : Editor where T : UnityEngine.Object {
		public new T target {
			get { return base.target as T; }
		}
	}

	public static class IGUI {
		public static void Indent(float left, float right, Action content) {
			GUILayout.BeginHorizontal();
			GUILayout.Space(left);
			GUILayout.BeginVertical();
			content();
			GUILayout.EndVertical();
			GUILayout.Space(right);
			GUILayout.EndHorizontal();
		}
	}
}