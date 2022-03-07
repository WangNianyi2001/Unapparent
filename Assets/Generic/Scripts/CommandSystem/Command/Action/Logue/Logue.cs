using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Unapparent {
	public class Logue : Command {
		public List<Monologue> monologues;
		public InspectorList<Monologue> iList;
		public Command next = null;

		public override object Execute(Carrier target) {
			for(int i = 1; i < monologues.Count; ++i)
				monologues[i - 1].next = monologues[i];
			monologues.Last().next = null;
			return monologues.Count == 0 ? null : monologues[0].Execute(target);
		}

		public override void Inspect(ArgList<Action> elements) {
			if(iList == null) {
				iList = new InspectorList<Monologue>(monologues, "Logue");
				iList.actionButtons.Add(new InspectorList<Monologue>.ActionButton {
					label = "Append",
					action = () => {
						monologues.Add(Create<Monologue>(this));
						SetDirty();
					}
				});
				iList.moreOptions.Add(new InspectorList<Monologue>.MoreOption {
					label = "Remove",
					action = (Monologue monologue, int i) => {
						monologues[i].Dispose();
						monologues.RemoveAt(i);
						SetDirty();
					}
				});
			}
			iList.Inspect();
		}

		public override void Dispose() {
			foreach(Monologue monologue in monologues)
				monologue?.Dispose();
			base.Dispose();
		}
	}
}
