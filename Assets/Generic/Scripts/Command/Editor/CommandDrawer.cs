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

		public override void InstanceGUI(SerializedProperty property, GUIContent label) {
			label = new GUIContent(label);
			label.text += $": {property.TargetObject().GetType().Name}";
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
						"OK", "Cancel")) {
						PropertyAccessor accessor = new PropertyAccessor(property);
						accessor.Value = null;
					}
				}
			}
			base.InstanceGUI(property, GUIContent.none);
		}

		public override void NullGUI(SerializedProperty property, GUIContent label) {
			var menu = property.ClosestDrawerType()?.GetStaticField("menu") as CommandMenu;
			Label(label);
			if(menu == null) {
				Label(new GUIContent("Command is null"));
				return;
			}
			Label(new GUIContent("Command is null, select a type to create one"));
			if(Button(new GUIContent("Create"))) {
				menu.OnSelect = (Type type) => {
					PropertyAccessor accessor = new PropertyAccessor(property);
					accessor.Value = Command.Create(type);
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
