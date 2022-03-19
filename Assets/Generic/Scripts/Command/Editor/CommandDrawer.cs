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

		public CommandDrawer() {
			propertyFilter = isPropertyOf(typeof(Command));
		}

		public override void NullGUI(SerializedProperty property, GUIContent label) {
			var menu = property.ClosestDrawerType()?.GetStaticField("menu") as CommandMenu;
			if(menu == null) {
				EditorGUI.LabelField(MakeArea(), label, new GUIContent("Command is null"));
				return;
			}
			if(GUI.Button(MakeArea(), "Set command")) {
				menu.OnSelect = (Type type) => property.SetTarget(Command.Create(type));
				menu.Show();
			}
		}
	}

	[CustomPropertyDrawer(typeof(Listener))]
	public class ListenerDrawer : CommandDrawer {
		public static new CommandMenu menu = new CommandMenu {
			typeof(OnStart),
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
