using System;
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;

namespace Unapparent {
	public interface IInspectable {
		public void Inspect(Action header, Action footer);
	}

	public class Inspector<T> : Editor where T : UnityEngine.Object {
		public new T target {
			get => base.target as T;
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
		// Aux

		public static void Nil() { }

		public class Labelizer<T> {
			public static string Labelize(T obj) {
				return obj.ToString();
			}
		}

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
			content();
			GUILayout.EndHorizontal();
		}

		public static void Block(Action content) {
			GUILayout.BeginVertical();
			content();
			GUILayout.EndVertical();
		}

		public static Action FillLine = GUILayout.FlexibleSpace;

		public static void Indent(Action header, Action content) => Inline(delegate {
			Block(delegate {
				header();
				VerticalLine();
			});
			Block(content);
		});

		public static void Indent(Action content) => Indent(Nil, content);

		public static void Center(Action content) => Inline(delegate {
			GUILayout.FlexibleSpace();
			content();
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

		public class MenuEntry<T> {
			public string text = null;
			public T element;
			public MenuEntry(T element) => this.element = element;
			public MenuEntry(string text) => this.text = text;
			public static implicit operator MenuEntry<T>(T element) => new MenuEntry<T>(element);
			public static implicit operator MenuEntry<T>(string text) => new MenuEntry<T>(text);
			public void AddTo(GenericMenu menu, Action<T> callback) {
				if(text != null)
					menu.AddSeparator(text + "/");
				else
					menu.AddItem(new GUIContent(element.ToString()), false, delegate (object element) {
						callback((T)element);
					}, element);
			}
		}

		public class SelectMenu<T, Labelizer> : List<MenuEntry<T>> where Labelizer : Labelizer<T> {
			public SelectMenu() { }
			public SelectMenu(IEnumerable<T> entries) : base(
				entries.Select(
					entry => new MenuEntry<T>(entry)
				)) {
			}
			public SelectMenu(IEnumerable<MenuEntry<T>> entries) : base(entries) { }

			public void AddTo(GenericMenu menu, Action<T> callback) {
				string path = "";
				var Labelize = typeof(Labelizer).GetMethod("Labelize");
				foreach(MenuEntry<T> entry in this) {
					if(entry.text != null) {
						if(entry.text == "") {
							path = "";
						} else {
							path = entry.text + "/";
							menu.AddSeparator(path);
						}
					} else
						menu.AddItem(
							new GUIContent(path + Labelize.Invoke(null, new object[] { entry.element })),
							false,
							(object element) => callback((T)element),
							entry.element
						);
				}
			}
		}

		public class SelectMenu<T> : SelectMenu<T, Labelizer<T>> { }

		public static void SelectButton<T, Menu, Labelizer>(
			string text, Menu list, Action<T> callback,
			params GUILayoutOption[] options)
				where Labelizer : Labelizer<T>
				where Menu : SelectMenu<T, Labelizer> {
			if(!Button(text, options))
				return;
			GenericMenu menu = new GenericMenu();
			list.AddTo(menu, callback);
			menu.ShowAsContext();
		}
		public static void Toggle(ref bool value, GUIContent label, params GUILayoutOption[] options) {
			value = GUILayout.Toggle(value, label, options);
		}

		public static bool Confirm(string text) {
			return EditorUtility.DisplayDialog("Confirm", text, "Proceed", "Cancel");
		}
	}
}
