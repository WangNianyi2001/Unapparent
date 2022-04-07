using UnityEditor;

namespace Unapparent {
	[CustomPropertyDrawer(typeof(Expression))]
	public class ExpressionDrawer : CommandDrawer {
		public static new CommandMenu menu = new CommandMenu {
			"Carrier",
			typeof(LastArrived),
		};
	}
}
