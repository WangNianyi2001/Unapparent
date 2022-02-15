using System;
using System.Collections.Generic;

namespace Unapparent {
	[Serializable]
	public class Sequential : ICommand {
		public List<Command> sequence = new List<Command>();

		public object Execute() {
			// TODO
			return null;
		}

		public void Inspect(Action header, Action footer) {
			IGUI.Indent(header, delegate {
				if(sequence.Count == 0) {
					IGUI.Center(delegate {
						IGUI.Italic("Empty");
					});
				} else {
					for(int i = 0; i < sequence.Count; ++i) {
						sequence[i].Inspect(delegate {
							IGUI.Bold(i.ToString(), UnityEngine.GUILayout.MinWidth(18));
						}, delegate {
							int j = i;
							if(IGUI.Button("Remove")) {
								if(!IGUI.Confirm("Removing command, proceed?"))
									return;
								sequence.RemoveAt(j);
							}
						});
					}
				}
				IGUI.Inline(delegate {
					IGUI.SelectButton("Add command", Command.TypeMenu.statement, delegate (Type type) {
						sequence.Add(new Command(type));
					}, IGUI.exWidth);
					footer();
				});
			});
		}
	}
}
