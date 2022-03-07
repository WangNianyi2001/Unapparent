using System;
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;

namespace Unapparent {
	public static class IGUI {
		// Flags

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
			content?.Invoke();
			GUILayout.EndHorizontal();
		}

		public static void Block(Action content) {
			GUILayout.BeginVertical();
			content?.Invoke();
			GUILayout.EndVertical();
		}

		public static Action FillLine = GUILayout.FlexibleSpace;

		public static void Center(Action content) => Inline(delegate {
			GUILayout.FlexibleSpace();
			content?.Invoke();
			GUILayout.FlexibleSpace();
		});

		public static void HorizontalLine() => EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

		public static void VerticalLine() => EditorGUILayout.LabelField(
			"", GUI.skin.verticalSlider,
			GUILayout.Width(8), noExWidth, exHeight
		);

		// Controls

		public static void Label(string text, params GUILayoutOption[] options) =>
			GUILayout.Label(text, EditorStyles.label, mergeOptions(options, noExWidth));

		public static void Bold(string text, params GUILayoutOption[] options) =>
			GUILayout.Label(text, EditorStyles.boldLabel, mergeOptions(options, noExWidth));

		public static void Italic(string text, params GUILayoutOption[] options) =>
			GUILayout.Label(text, new GUIStyle(GUI.skin.label) {
				fontStyle = FontStyle.Italic
			}, mergeOptions(options, noExWidth));

		public static bool Button(string text, params GUILayoutOption[] options) =>
			GUILayout.Button(text, GUI.skin.button, mergeOptions(options, noExWidth));

		public static void SelectButton<T>(
			string text, ISelectMenu<T> list, Action<T> callback,
			params GUILayoutOption[] options) {
			if(!Button(text, options))
				return;
			GenericMenu menu = new GenericMenu();
			list.AddTo(menu, callback);
			menu.ShowAsContext();
		}

		public static bool ObjectField<T>(ref T obj, Type type, bool allowSceneObjects, params GUILayoutOption[] options)
			where T : UnityEngine.Object {
			T old = obj;
			obj = EditorGUILayout.ObjectField(obj, type, allowSceneObjects, options) as T;
			return old != obj;
		}

		public static bool ObjectField<T>(ref T obj, bool allowSceneObjects, params GUILayoutOption[] options)
			where T : UnityEngine.Object => ObjectField(ref obj, typeof(T), allowSceneObjects, options);

		public static bool Toggle(ref bool value, params GUILayoutOption[] options) {
			bool old = value;
			value = GUILayout.Toggle(value, GUIContent.none, options);
			return old != value;
		}


		public static bool Confirm(string text) {
			return EditorUtility.DisplayDialog("Confirm", text, "Proceed", "Cancel");
		}

		public static bool TextField(ref string text) {
			string old = text;
			text = EditorGUILayout.TextField(text);
			return old != text;
		}

		public static bool FloatField(ref float number) {
			float old = number;
			number = EditorGUILayout.FloatField(number);
			return old != number;
		}
	}
}
