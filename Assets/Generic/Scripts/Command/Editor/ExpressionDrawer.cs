using UnityEditor;

namespace Unapparent {
	[CustomPropertyDrawer(typeof(Expression))]
	public class ExpressionDrawer : CommandDrawer {
		public static new CommandMenu menu = new CommandMenu {
			"Variable",
			typeof(IntegerIs),
			"Carrier",
			typeof(LastArrived),
			typeof(IsInState),
			"Character",
			typeof(AppearingAs),
			typeof(HasTag),
		};
	}
}
