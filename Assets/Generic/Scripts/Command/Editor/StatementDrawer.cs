using UnityEditor;

namespace Unapparent {
	[CustomPropertyDrawer(typeof(Statement))]
	public class StatementDrawer : CommandDrawer {
		public static new CommandMenu menu = new CommandMenu {
			"Carrier",
			typeof(SwitchState),
			typeof(TeleportTo),
			"Character",
			typeof(NavigateTo),
			typeof(Monologue),
			typeof(Logue),
			"Protagonist",
			typeof(ShapeshiftInto),
			"Control",
			typeof(Conditional),
			typeof(Sequential),
		};
	}
}
