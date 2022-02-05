using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;


namespace Unapparent {
	public class Sequential : State.Action {
		List<State.Action> sequence = new List<State.Action>();
		public override void Execute() {
			// TODO
		}
		int typeIndexToAdd = 0;
		public override void Inspect() {
			IGUI.Indent(delegate {
				if(sequence.Count != 0) {
					foreach(State.Action action in sequence)
						action.Inspect();
				} else {
					IGUI.Center(delegate {
						GUILayout.Label(
							"Empty sequence",
							new GUIStyle(GUI.skin.label){
								fontStyle= FontStyle.Italic
							}
						);
					});
				}
				GUILayout.BeginHorizontal();
				typeIndexToAdd = EditorGUILayout.Popup(
					typeIndexToAdd,
					State.actionTypes.Select(delegate (Type type) {
						return type.Name;
					}).ToArray()
				);
				if(GUILayout.Button("Add action")) {
					// TODO
				}
				GUILayout.EndHorizontal();
			});
		}
	}
}