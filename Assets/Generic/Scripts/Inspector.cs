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
		public static void Indent(Action content) {
			++EditorGUI.indentLevel;
			content();
			--EditorGUI.indentLevel;
		}
		public static void Center(Action content) {
			GUILayout.BeginHorizontal();
			GUILayout.FlexibleSpace();
			content();
			GUILayout.FlexibleSpace();
			GUILayout.EndHorizontal();
		}
		public static void LineSep() {
			EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
		}
	}
}