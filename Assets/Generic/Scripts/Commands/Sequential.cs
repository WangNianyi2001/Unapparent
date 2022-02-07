using System;
using System.Collections.Generic;

namespace Unapparent {
	public class Sequential : Statement {
		public List<Statement> sequence = new List<Statement>();

		public override Void Execute(Void arg) {
			// TODO
			return null;
		}

		public override void Inspect(Action header, Action footer) {
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
					IGUI.SelectButton("Add command", types, delegate (Type type) {
						sequence.Add((Statement)Activator.CreateInstance(type));
					}, IGUI.exWidth);
					footer();
				});
			});
		}

		public override void OnAfterDeserialize() {
			throw new NotImplementedException();
		}

		public override void OnBeforeSerialize() {
			throw new NotImplementedException();
		}
	}
}