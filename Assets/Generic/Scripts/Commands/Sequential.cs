using System;
using System.Collections.Generic;
using UnityEngine;

namespace Unapparent {
	public class Sequential : Command {
		public List<Command> sequence = new List<Command>();

		public override object Execute() {
			// TODO
			return null;
		}

		public override void Inspect(ArgList<Action> elements) {
			IGUI.Indent(() => {
				if(sequence.Count == 0)
					IGUI.Center(() => IGUI.Italic("Empty"));
				else {
					for(int i = 0; i < sequence.Count; ++i) {
						sequence[i].Inspect(
							() => IGUI.Bold(i.ToString(), GUILayout.MinWidth(18)),
							() => {
								int j = i;
								if(IGUI.Button("Remove")) {
									if(!IGUI.Confirm("Removing command, proceed?"))
										return;
									sequence[j]?.Dispose();
									sequence.RemoveAt(j);
									SetDirty();
								}
							}
						);
					}
				}
				IGUI.Inline(() => {
					IGUI.SelectButton("Add command", TypeMenu.statement,
						(Type type) => {
							sequence.Add(Create(type, this));
							SetDirty();
						},
						IGUI.exWidth);
					elements[1]?.Invoke();
				});
			}, elements[0]);
		}

		public override void Dispose() {
			foreach(Command command in sequence)
				command?.Dispose();
			base.Dispose();
		}
	}
}
