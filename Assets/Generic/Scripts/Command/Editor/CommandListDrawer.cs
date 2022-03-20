using System;
using UnityEditor;

namespace Unapparent {
	public class CommandListDrawer<T> : InspectableListDrawer<T> where T : Command {
		public virtual void OnSelect(Type type) => elements.Add(Command.Create(type) as T);

		public override void OnAdd() {
			var menu = typeof(T).ClosestDrawerType()?.GetStaticField("menu") as CommandMenu;
			if(menu == null)
				return;
			menu.OnSelect = OnSelect;
			menu.Show();
		}
	}

	// Dummy class to make Unity happy with generic drawers
	[CustomPropertyDrawer(typeof(ListenerList))]
	public class ListenerListDrawer : CommandListDrawer<Listener> {
		public override void OnSelect(Type type) {
			var listener = Command.Create(type) as Listener;
			listener.action = Command.Create<Sequential>();
			elements.Add(listener);
		}
	}

	[CustomPropertyDrawer(typeof(StatementList))]
	public class StatementListDrawer : CommandListDrawer<Statement> {
	}

	[CustomPropertyDrawer(typeof(ExpressionList))]
	public class ExpressionListDrawer : CommandListDrawer<Expression> {
	}
}
