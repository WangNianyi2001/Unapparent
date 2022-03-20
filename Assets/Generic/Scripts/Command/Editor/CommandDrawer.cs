using System;
using UnityEditor;
using UnityEngine;

namespace Unapparent {
	public class CommandMenu : Menu<Type> {
		public CommandMenu() => OnLabelize = (Type target) => target.Name;
	}

	[CustomPropertyDrawer(typeof(Command))]
	public class CommandDrawer : NestedDrawer {
		public static CommandMenu menu = null;

		public new static PropertyFilter propertyFilter = isPropertyOf(typeof(Command));

		public override void NullGUI(SerializedProperty property, GUIContent label, bool draw = true) {
			var menu = property.ClosestDrawerType()?.GetStaticField("menu") as CommandMenu;
			Label(label, draw);
			if(menu == null) {
				Label(new GUIContent("Command is null"), draw);
				return;
			}
			if(Button(new GUIContent("Set command"), draw)) {
				menu.OnSelect = (Type type) => property.SetTarget(Command.Create(type));
				menu.Show();
			}
		}
	}

	[CustomPropertyDrawer(typeof(Listener))]
	public class ListenerDrawer : CommandDrawer {
		public static new CommandMenu menu = new CommandMenu {
			typeof(EnterState),
			typeof(ExitState),
		};
	}

	[CustomPropertyDrawer(typeof(Statement))]
	public class StatementDrawer : CommandDrawer {
		public static new CommandMenu menu = new CommandMenu {
			"Control",
			typeof(Conditional),
			typeof(Sequential),
			typeof(Agent),
			"Logue",
			typeof(Monologue),
			typeof(Logue),
			"Character",
			typeof(SwitchState),
			typeof(NavigateTo),
		};
	}

	[CustomPropertyDrawer(typeof(Expression))]
	public class ExpressionDrawer : CommandDrawer {
		public static new CommandMenu menu = new CommandMenu {
			typeof(BoolConstant),
		};
	}
}
