using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;


namespace Unapparent {
	public class Sequential : List<State.Action> {
		int typeIndexToAdd = 0;
		public void Inspect() {
			IGUI.Indent(10, 10, delegate {
				foreach(State.Action action in this) {
					action.Inspect();
				}
				GUILayout.BeginHorizontal();
				typeIndexToAdd = EditorGUILayout.Popup(
					typeIndexToAdd,
					State.actionTypes.Select(delegate (Type type) {
						return type.Name;
					}).ToArray()
				);
				GUILayout.Button("Add sub action");
				GUILayout.EndHorizontal();
			});
		}
	}
}