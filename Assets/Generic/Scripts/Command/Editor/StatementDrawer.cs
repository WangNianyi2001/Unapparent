using UnityEditor;

namespace Unapparent {
	[CustomPropertyDrawer(typeof(Statement))]
	public class StatementDrawer : CommandDrawer {
		public static new CommandMenu menu = new CommandMenu {
			"Control",
			typeof(Sequential),
			typeof(Conditional),
			typeof(Async),
			typeof(CallMacro),
			"Variable",
			typeof(DeclareInteger),
			typeof(SucceedInteger),
			"Carrier",
			typeof(Subjecting),
			typeof(SwitchState),
			typeof(TeleportTo),
			typeof(SendMessage),
			"Character",
			typeof(NavigateTo),
			typeof(StopNavigation),
			typeof(Monologue),
			typeof(Logue),
			"Protagonist",
			typeof(ShapeshiftInto),
			typeof(ChooseShapeshift),
			typeof(UnlockIdentity)
		};
	}
}
