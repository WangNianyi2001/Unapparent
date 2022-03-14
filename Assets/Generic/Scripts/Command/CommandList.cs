using System;

namespace Unapparent {
	[Serializable]
	public class CommandList : InspectableList<Command> {
		public override void OnAdd() {
			var type = typeof(OnStart);
			var command = Command.Create(type);
			elements.Add(command);
		}
	}
}
