using UnityEngine;
using UnityEditor;
using System;

namespace Unapparent {
	public class Inspector<T> : Editor where T : UnityEngine.Object {
		public new T target {
			get { return base.target as T; }
		}
	}

	public class ReadOnlyAttribute : PropertyAttribute { }

	[CustomPropertyDrawer(typeof(ReadOnlyAttribute))]
	public class ReadOnlyDrawer : PropertyDrawer {
		public override float GetPropertyHeight(
			SerializedProperty property,
			GUIContent label) {
			return EditorGUI.GetPropertyHeight(property, label, true);
		}
		public override void OnGUI(
			Rect position,
			SerializedProperty property,
			GUIContent label) {
			GUI.enabled = false;
			EditorGUI.PropertyField(position, property, label, true);
			GUI.enabled = true;
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