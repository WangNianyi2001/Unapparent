using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

namespace Unapparent {
	public class Sequential : State.Action {
		public List<State.Action> sequence = new List<State.Action>();
		public override void Execute() {
			// TODO
		}
		int typeIndexToAdd = 0;
		public override void Inspect() {
			IGUI.Indent(delegate {
				if(sequence.Count != 0) {
					for(int i = 0; i < sequence.Count; ++i) {
						GUILayout.Label(i.ToString());
						sequence[i].Inspect();
					}
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
					Type type = State.actionTypes[typeIndexToAdd];
					sequence.Add(Make(type));
				}
				GUILayout.EndHorizontal();
			});
		}
	}
	public class SwitchState : State.Action {
		public override void Execute() {
			// TODO
		}
		public override void Inspect() {
			GUILayout.Label("Switch to state");
		}
	}
}