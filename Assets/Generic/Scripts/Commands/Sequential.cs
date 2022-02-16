using System;
using System.Collections.Generic;
using UnityEngine;

namespace Unapparent {
	[CreateAssetMenu]
	public class Sequential : Command {
		public List<Command> sequence = new List<Command>();

		public override object Execute() {
			// TODO
			return null;
		}

		public override void Inspect(Action header, Action footer) {
			IGUI.Indent(header, () => {
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
									sequence.RemoveAt(j);
								}
							}
						);
					}
				}
				IGUI.Inline(() => {
					IGUI.Label("Add command");
					ScriptableObject so = IGUI.ObjectField(
						null, typeof(ScriptableObject), false,
						GUILayout.ExpandWidth(true)
					) as ScriptableObject;
					if(so != null)
						sequence.Add(Instantiate(so) as Command);
					footer?.Invoke();
				});
			});
		}
	}
}
