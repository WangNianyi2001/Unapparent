using System;

namespace Unapparent {
	[Serializable]
	public class CommandList : InspectableList<Command> {
		public override void OnAdd() {
			elements.Add(null);
		}
	}
}
