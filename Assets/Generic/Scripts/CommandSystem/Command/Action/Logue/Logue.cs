using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Unapparent {
	public class Logue : Command {
		[SerializeField] public List<Monologue> monologues;
		public Command next = null;

		public override object Execute(Carrier target) {
			for(int i = 1; i < monologues.Count; ++i)
				monologues[i - 1].next = monologues[i];
			monologues.Last().next = null;
			return monologues.Count == 0 ? null : monologues[0]?.Execute(target);
		}

		public override void Inspect(ArgList<Action> elements) {
			IGUI.Inline(() => {
				elements[0]?.Invoke();
				IGUI.Label("Logue");
				ShowRefBtn();
				GUILayout.FlexibleSpace();
			});
			IGUI.Indent(() => {
				for(int i = 0; i < monologues.Count; ++i) {
					int j = i;
					Monologue monologue = monologues[j];
					monologue.Inspect(null, () => {
						if(IGUI.Button("Remove", IGUI.exWidth)) {
							if(!IGUI.Confirm("Removing a monologue, proceed?"))
								return;
							monologues[j]?.Dispose();
							monologues.RemoveAt(j);
							SetDirty();
						}
					});
				}
			});
			IGUI.Inline(() => {
				if(IGUI.Button("Add monologue", IGUI.exWidth)) {
					Monologue monologue = Create<Monologue>(this);
					monologues.Add(monologue);
					SetDirty();
				}
				elements[1]?.Invoke();
			});
		}

		public override void Dispose() {
			foreach(Monologue monologue in monologues)
				monologue?.Dispose();
			base.Dispose();
		}
	}
}
