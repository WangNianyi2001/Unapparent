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
		public override void Inspect(Action header, Action footer) {
			IGUI.Indent(header, delegate {
				if(sequence.Count == 0) {
					IGUI.Center(delegate {
						IGUI.Italic("Empty sequence");
					});
				} else {
					for(int i = 0; i < sequence.Count; ++i) {
						sequence[i].Inspect(delegate {
							IGUI.Bold(i.ToString());
						}, delegate {
							int j = i;
							if(IGUI.Button("-")) {
								if(!IGUI.Confirm("Removing action, proceed?"))
									return;
								sequence.RemoveAt(j);
							}
						});
					}
				}
				GUILayout.BeginHorizontal();
				typeIndexToAdd = EditorGUILayout.Popup(
					typeIndexToAdd,
					State.actionTypes.Select(delegate (Type type) {
						return type.Name;
					}).ToArray()
				);
				if(IGUI.Button("+")) {
					Type type = State.actionTypes[typeIndexToAdd];
					sequence.Add(Make(type));
				}
				footer();
				GUILayout.EndHorizontal();
			});
		}
	}

	public class SwitchState : State.Action {
		GameObject destination = null;
		public override void Execute() {
			// TODO
		}
		public override void Inspect(Action header, Action footer) {
			GUILayout.BeginHorizontal();
			header();
			GUILayout.Label("Switch to");
			destination = EditorGUILayout.ObjectField(destination, typeof(GameObject), true) as GameObject;
			footer();
			GUILayout.EndHorizontal();
		}
	}
}