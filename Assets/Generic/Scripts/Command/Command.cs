using System;
using UnityEditor;
using UnityEngine;

namespace Unapparent {
	[Serializable]
	public abstract class Command : ScriptableObject {
		public class TypeMenu : SelectMenu<Type, TypeMenu.Labelizer> {
			public class Labelizer : Labelizer<Type> {
				public new static string Labelize(Type obj) => obj.Name;
			}

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

		public new void SetDirty() {
			EditorUtility.SetDirty(this);
		}

		public static Command Create(Type type) => CreateInstance(type) as Command;

		public static T Create<T>() where T : Command => Create(typeof(T)) as T;

		public abstract object Execute(Carrier target);
	}
}
