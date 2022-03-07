using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Unapparent {
	public class InspectorList<T> : IInspectable where T : IInspectable {
		IList<T> list;
		public string name;
		bool expand = true;

		public struct ActionButton {
			public string label;
			public Action action;
		}
		public List<ActionButton> actionButtons = new List<ActionButton>();

		public struct MoreOption {
			public string label;
			public Action<T, int> action;
		}
		public List<MoreOption> moreOptions = new List<MoreOption>();

		public InspectorList(IList<T> list, string name) {
			this.name = name;
			this.list = list;
		}

		public void Inspect(params Action[] elements) {
			expand = EditorGUILayout.Foldout(expand, name);
			GUILayout.FlexibleSpace();
			if(!expand)
				return;
			EditorGUILayout.BeginVertical();
			if(list.Count == 0) {
				EditorGUILayout.BeginHorizontal();
				GUILayout.FlexibleSpace();
				GUILayout.Label("Empty", new GUIStyle(GUI.skin.label) {
					fontStyle = FontStyle.Italic
				});
				GUILayout.FlexibleSpace();
				EditorGUILayout.EndHorizontal();
			}
			for(int i = 0; i < list.Count; ++i) {
				int j = i;
				T element = list[j];
				EditorGUILayout.BeginHorizontal();
				if(moreOptions.Count > 0) {
					if(GUILayout.Button(GUIContent.none, EditorStyles.radioButton, IGUI.noExWidth)) {
						GenericMenu menu = new GenericMenu();
						foreach(MoreOption moreOption in moreOptions) {
							GUIContent label = new GUIContent(moreOption.label);
							GenericMenu.MenuFunction action = () => moreOption.action(element, j);
							menu.AddItem(label, false, action);
						}
						menu.ShowAsContext();
					}
				}
				EditorGUILayout.BeginVertical();
				element.Inspect();
				EditorGUILayout.EndVertical();
				EditorGUILayout.EndHorizontal();
			}
			EditorGUILayout.BeginHorizontal();
			foreach(ActionButton actionButton in actionButtons) {
				if(GUILayout.Button(actionButton.label))
					actionButton.action();
			}
			EditorGUILayout.EndHorizontal();
			EditorGUILayout.EndVertical();
		}
	}
}
