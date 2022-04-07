using System;
using System.Reflection;
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

		public override void InstanceGUI(PropertyAccessor accessor, GUIContent label) {
			label = new GUIContent(label);
			string appendText = accessor.value.GetType().Name;
			if(!string.IsNullOrWhiteSpace(appendText)) {
				label.text = string.IsNullOrWhiteSpace(label.text)
					? appendText
					: label.text + $": {appendText}";
			}
			const float buttonWidth = 50;
			Rect rect = MakeArea();
			Rect buttonRect = rect;
			buttonRect.xMin = buttonRect.xMax - buttonWidth;
			rect.xMax -= buttonWidth;
			if(draw) {
				EditorGUI.LabelField(rect, label);
				if(GUI.Button(buttonRect, "Clear")) {
					if(EditorUtility.DisplayDialog("Confirm",
						"You are clearing a command, proceed?",
						"OK", "Cancel"))
						accessor.value = null;
				}
			}
			base.InstanceGUI(accessor, GUIContent.none);
		}

		public override void NullGUI(PropertyAccessor accessor, GUIContent label) {
			Type drawerType = DrawerTypeGetter.Closest(accessor.type);
			var fi = drawerType.GetField("menu", BindingFlags.Static | BindingFlags.Public);
			var menu = fi.GetValue(null) as CommandMenu;
			Label(label);
			if(menu == null) {
				Label(new GUIContent("Command is null"));
				return;
			}
			Label(new GUIContent("Command is null, select a type to create one"));
			if(Button(new GUIContent("Create"))) {
				menu.OnSelect = (Type type) => accessor.value = Command.Create(type);
				menu.Show();
			}
		}
	}

	[CustomPropertyDrawer(typeof(Listener))]
	public class ListenerDrawer : CommandDrawer {
		public static new CommandMenu menu = new CommandMenu {
			"State",
			typeof(EnterState),
			typeof(ExitState),
			"Trigger",
			typeof(EnterTrigger),
			typeof(ExitTrigger),
		};
	}

	[CustomPropertyDrawer(typeof(Statement))]
	public class StatementDrawer : CommandDrawer {
		public static new CommandMenu menu = new CommandMenu {
			"Carrier",
			typeof(SwitchState),
			"Character",
			typeof(NavigateTo),
			typeof(Monologue),
			"Control",
			typeof(Conditional),
			typeof(Sequential),
		};
	}

	[CustomPropertyDrawer(typeof(Expression))]
	public class ExpressionDrawer : CommandDrawer {
		public static new CommandMenu menu = new CommandMenu {
			typeof(BoolConstant),
		};
	}
}
