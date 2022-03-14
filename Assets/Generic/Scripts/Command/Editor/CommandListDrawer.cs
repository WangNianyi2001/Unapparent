using UnityEditor;

namespace Unapparent {
	// Dummy class to make Unity happy with generic drawers
	[CustomPropertyDrawer(typeof(CommandList))]
	public class CommandListDrawer : InspectableListDrawer<Command> {
	}
}
