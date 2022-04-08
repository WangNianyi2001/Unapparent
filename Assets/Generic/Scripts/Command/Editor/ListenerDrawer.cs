using UnityEditor;

namespace Unapparent {
	[CustomPropertyDrawer(typeof(Listener))]
	public class ListenerDrawer : CommandDrawer {
		public static new CommandMenu menu = new CommandMenu {
			typeof(ReceiveMessage),
			"State",
			typeof(EnterState),
			typeof(ExitState),
			"Trigger",
			typeof(EnterTrigger),
			typeof(ExitTrigger),
		};
	}
}
