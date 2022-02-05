using System;
using UnityEngine;
using UnityEditor;

namespace Unapparent {
	public interface IInspectable {
		public void Inspect(Action header, Action footer);
	}

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
		public static void Indent(Action header, Action content) {
			GUILayout.BeginHorizontal();
			GUILayout.BeginVertical();
			header();
			VerticalLine();
			GUILayout.EndVertical();
			GUILayout.BeginVertical();
			content();
			GUILayout.EndVertical();
			GUILayout.EndHorizontal();
		}
		public static void Indent(Action content) {
			Indent(State.Action.Nil, content);
		}
		public static void Center(Action content) {
			GUILayout.BeginHorizontal();
			GUILayout.FlexibleSpace();
			content();
			GUILayout.FlexibleSpace();
			GUILayout.EndHorizontal();
		}
		public static void HorizontalLine() {
			EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
		}
		public static void VerticalLine() {
			EditorGUILayout.LabelField("", GUI.skin.verticalSlider,
				GUILayout.Width(8),
				GUILayout.ExpandWidth(false),
				GUILayout.ExpandHeight(true)
			);
		}
		public static void Label(string text) {
			GUILayout.Label(text, EditorStyles.label, GUILayout.ExpandWidth(false));
		}
		public static void Bold(string text) {
			GUILayout.Label(text, EditorStyles.boldLabel, GUILayout.ExpandWidth(false));
		}
		public static void Italic(string text) {
			GUILayout.Label(text, new GUIStyle(GUI.skin.label) {
				fontStyle = FontStyle.Italic
			}, GUILayout.ExpandWidth(false));
		}
		public static bool Button(string text) {
			return GUILayout.Button(text, GUI.skin.button, GUILayout.ExpandWidth(false));
		}
		public static bool Confirm(string text) {
			return EditorUtility.DisplayDialog("Confirm", text, "Proceed", "Cancel");
		}
	}
}