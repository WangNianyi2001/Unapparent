using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

namespace Unapparent {
	public class Sequence : Statement {
		public List<Statement> sequence = new List<Statement>();
		public override void Execute() {
			// TODO
		}
		int typeIndexToAdd = 0;
		public override void Inspect(Action header, Action footer) {
			IGUI.Indent(header, delegate {
				IGUI.Label("Sequence");
				if(sequence.Count == 0) {
					IGUI.Center(delegate {
						IGUI.Italic("Empty");
					});
				} else {
					for(int i = 0; i < sequence.Count; ++i) {
						sequence[i].Inspect(delegate {
							IGUI.Bold(i.ToString(), GUILayout.MinWidth(18));
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
}