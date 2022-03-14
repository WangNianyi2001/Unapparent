using System;
using UnityEngine;

namespace Unapparent {
	[Serializable]
	public abstract class Command : ScriptableObject {
		public static Command Create(Type type) => CreateInstance(type) as Command;

		public static T Create<T>() where T : Command => Create(typeof(T)) as T;

		public abstract object Execute(Carrier target);

		public static TypeMenu menu = TypeMenu.statement;

		public class TypeMenu : Menu<Type> {
			public TypeMenu() => OnLabelize = (Type target) => target.Name;

			public static TypeMenu
				statement = new TypeMenu {
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
				condition = new TypeMenu {
					typeof(Reference),
					typeof(BoolConstant),
				},
				listener = new TypeMenu {
					typeof(OnStart),
				};
		}
	}
}
