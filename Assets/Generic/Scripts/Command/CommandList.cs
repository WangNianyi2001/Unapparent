using System;

namespace Unapparent {
	public class CommandList<T> : InspectableList<T> where T : Command {
	}

	[Serializable]
	public class CommandList : CommandList<Command> {
	}

	[Serializable]
	public class ListenerList : CommandList<Listener> {
	}
}
