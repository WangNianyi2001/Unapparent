using System;

namespace Unapparent {
	public interface ICommand : IInspectable {
		public object Execute();
	}

	public class TypeMenu : IGUI.SelectMenu<Type, TypeMenu.Labelizer> { 
		public class Labelizer : IGUI.Labelizer<Type> {
			public new static string Labelize(Type type) {
				return type.FullName;
			}
		}
	}

	public abstract class Command : ICommand {
		public abstract object Execute();
		public abstract void Inspect(Action header, Action footer);
	}

	public abstract class Statement : Command {
		public static TypeMenu menu = new TypeMenu {
			"Control flow",
			typeof(Sequential),
			typeof(Conditional),
			"Action",
			typeof(SwitchState),
		};
	}

	public abstract class Condition : Command {
		public static TypeMenu menu = new TypeMenu {
			"Constant",
			typeof(BoolConstant),
		};
	}
}
