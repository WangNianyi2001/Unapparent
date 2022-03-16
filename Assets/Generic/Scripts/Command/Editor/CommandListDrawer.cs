using System;
using UnityEditor;
using UnityEngine;

namespace Unapparent {
	public class CommandListDrawer<T> : InspectableListDrawer<T> where T : Command {
		public static Menu menu = null;

		public class Menu : Menu<Type> {
			public Menu() => OnLabelize = (Type target) => target.Name;

			public static Menu
				statement = new Menu {
					typeof(Reference),
					"Logue",
					typeof(Monologue),
					typeof(Logue),
					"Control",
					typeof(Conditional),
					typeof(Sequential),
					typeof(Agent),
					"Character",
					typeof(SwitchState),
					typeof(NavigateTo),
				},
				condition = new Menu {
					typeof(Reference),
					typeof(BoolConstant),
				};
		}

		public override void OnAdd() {
			var menu = GetType().GetStaticField("menu") as Menu;
			if(menu == null)
				return;
			menu.OnSelect = (Type type) => {
				elements.Add(Command.Create(type) as T);
			};
			menu.Show();
		}
	}

	// Dummy class to make Unity happy with generic drawers
	[CustomPropertyDrawer(typeof(CommandList))]
	public class CommandListDrawer : CommandListDrawer<Command> {
	}

	[CustomPropertyDrawer(typeof(ListenerList))]
	public class ListenerListDrawer : CommandListDrawer<Command> {
		public static new Menu menu = new Menu {
			typeof(OnStart),
		};
	}
}
