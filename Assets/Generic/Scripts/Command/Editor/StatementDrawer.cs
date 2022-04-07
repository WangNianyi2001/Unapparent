using UnityEditor;

namespace Unapparent {
	[CustomPropertyDrawer(typeof(Statement))]
	public class StatementDrawer : CommandDrawer {
		public static new CommandMenu menu = new CommandMenu {
			"Carrier",
			typeof(SwitchState),
			typeof(Teleport),
			"Character",
			typeof(Navigate),
			typeof(Monologue),
			typeof(Logue),
			"Protagonist",
			typeof(Shapeshift),
			"Control",
			typeof(Conditional),
			typeof(Sequential),
		};
	}
}
