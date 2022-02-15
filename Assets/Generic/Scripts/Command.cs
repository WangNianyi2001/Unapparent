using System;
using UnityEngine;

namespace Unapparent {
	public interface ICommand : IInspectable {
		public object Execute();
	}

	[Serializable]
	public class Command : ICommand, ISerializationCallbackReceiver {
		string typeName;
		public Type Type {
			get => Type.GetType(typeName);
			set => typeName = value.FullName;
		}
		public ICommand command = null;

		public Command(ICommand command) {
			this.command = command;
			Type = command.GetType();
		}

		public Command(Type type) {
			Type = type;
			OnAfterDeserialize();
		}

		public static implicit operator Command(Type type) => new Command(type);

		public void OnAfterDeserialize() {
			command = (ICommand)Activator.CreateInstance(Type);
		}

		public void OnBeforeSerialize() { }

		public object Execute() => command.Execute();
		public void Inspect(Action header, Action footer) => command.Inspect(header, footer);

		public class TypeMenu : IGUI.SelectMenu<Type, TypeMenu.Labelizer> { 
			public class Labelizer : IGUI.Labelizer<Type> {
				public new static string Labelize(Type type) => type.Name;
			}

			public static TypeMenu
			statement = new TypeMenu {
				"Control flow",
				typeof(Sequential),
				typeof(Conditional),
				"Action",
				typeof(SwitchState),
			},
			condition = new TypeMenu {
				"Constant",
				typeof(BoolConstant),
			};
		}
	}
}
