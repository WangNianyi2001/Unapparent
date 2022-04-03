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

		public override void DrawGUI(SerializedProperty property, GUIContent label) {
			GUIContent modifiedLabel = new GUIContent(label);
			modifiedLabel.text += $": {property.TargetObject().GetType().Name}";
			base.DrawGUI(property, modifiedLabel);
		}

		public override void NullGUI(SerializedProperty property, GUIContent label) {
			var menu = property.ClosestDrawerType()?.GetStaticField("menu") as CommandMenu;
			Label(label);
			if(menu == null) {
				Label(new GUIContent("Command is null"));
				return;
			}
			Label(new GUIContent("Command is null, click the button to create one"));
			if(Button(new GUIContent("Select type"))) {
				menu.OnSelect = (Type type) => {
					PropertyAccessor accessor = new PropertyAccessor(property);
					accessor.Value = Command.Create(type);
					Debug.Log(accessor.Value);
				};
				menu.Show();
			}
		}
	}

	[CustomPropertyDrawer(typeof(List))]
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
