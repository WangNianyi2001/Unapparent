using System;
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

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
		public static void Nil() { }
		public static readonly GUILayoutOption
			noExWidth = GUILayout.ExpandWidth(false),
			noExHeight = GUILayout.ExpandHeight(false),
			exWidth = GUILayout.ExpandWidth(true),
			exHeight = GUILayout.ExpandHeight(true);
		public static GUILayoutOption[] mergeOptions(GUILayoutOption[] a, params GUILayoutOption[] b) {
			var list = new List<GUILayoutOption>(a);
			list.InsertRange(0, b);
			return list.ToArray();
		}

		// Layout
		public static void Inline(Action content) {
			GUILayout.BeginHorizontal();
			content();
			GUILayout.EndHorizontal();
		}
		public static void Block(Action content) {
			GUILayout.BeginVertical();
			content();
			GUILayout.EndVertical();
		}
		public static Action FillLine = GUILayout.FlexibleSpace;
		public static void Indent(Action header, Action content) {
			Inline(delegate {
				Block(delegate {
					header();
					VerticalLine();
				});
				Block(content);
			});
		}
		public static void Indent(Action content) {
			Indent(Nil, content);
		}
		public static void Center(Action content) {
			Inline(delegate {
				GUILayout.FlexibleSpace();
				content();
				GUILayout.FlexibleSpace();
			});
		}
		public static void HorizontalLine() {
			EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
		}
		public static void VerticalLine() {
			EditorGUILayout.LabelField("", GUI.skin.verticalSlider,
				GUILayout.Width(8), noExWidth, exHeight
			);
		}

		// Controls
		public static void Label(string text, params GUILayoutOption[] options) {
			GUILayout.Label(text, EditorStyles.label, mergeOptions(options, noExWidth));
		}
		public static void Bold(string text, params GUILayoutOption[] options) {
			GUILayout.Label(text, EditorStyles.boldLabel, mergeOptions(options, noExWidth));
		}
		public static void Italic(string text, params GUILayoutOption[] options) {
			GUILayout.Label(text, new GUIStyle(GUI.skin.label) {
				fontStyle = FontStyle.Italic
			}, mergeOptions(options, noExWidth));
		}
		public static bool Button(string text, params GUILayoutOption[] options) {
			return GUILayout.Button(text, GUI.skin.button, mergeOptions(options, noExWidth));
		}
		public static void SelectButton<T, L>(string text, L list, Action<T> callback, params GUILayoutOption[] options)
			where L : IList<T> {
			if(!Button(text, options))
				return;
			GenericMenu menu = new GenericMenu();
			foreach(T element in list) {
				menu.AddItem(new GUIContent(element.ToString()), false, delegate(object element) {
					callback((T)element);
				}, element);
			}
			menu.ShowAsContext();
		}
		public static void Toggle(ref bool value, GUIContent label, params GUILayoutOption[] options) {
			value = GUILayout.Toggle(value, label, options);
		}

		// Misc
		public static bool Confirm(string text) {
			return EditorUtility.DisplayDialog("Confirm", text, "Proceed", "Cancel");
		}
	}
}