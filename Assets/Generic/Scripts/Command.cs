using System;

namespace Unapparent {
	public interface ICommand : IInspectable {
		public object Execute();
	}

	public class Command : ICommand {
		ICommand command = null;

		public object Execute() => command.Execute();
		public void Inspect(Action header, Action footer) => command.Inspect(header, footer);


		public class TypeMenu : IGUI.SelectMenu<Type, TypeMenu.Labelizer> { 
			public class Labelizer : IGUI.Labelizer<Type> {
				public new static string Labelize(Type type) => type.Name;
			}
		}

		public static TypeMenu statement = new TypeMenu {
			"Control flow",
			typeof(Sequential),
			typeof(Conditional),
			"Action",
			typeof(SwitchState),
		}, condition = new TypeMenu {
			"Constant",
			typeof(BoolConstant),
		};
	}
}
