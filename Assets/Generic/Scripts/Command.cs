using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Unapparent {
	public class Void { }

	public interface ICommand<Arg, Ret> : IInspectable, ISerializationCallbackReceiver {
		public Ret Execute(Arg arg);
	}

	public class CommandType {
		Type type;
		public CommandType(Type type) => this.type = type;
		public static implicit operator CommandType(Type type) => new CommandType(type);
		public static implicit operator Type(CommandType type) => type.type;
		public string Name { get => type.Name; }
		public override string ToString() => Name;

		public static Dictionary<string, CommandType> table = new List<CommandType> {
			typeof(Sequential),
			typeof(Conditional),
			typeof(SwitchState),
			typeof(BoolConstant),
		}.ToDictionary(keySelector: type => type.Name);
	}

	public abstract class Command<Arg, Ret> : ICommand<Arg, Ret> {
		public abstract Ret Execute(Arg arg);
		public abstract void Inspect(Action header, Action footer);
		public abstract void OnAfterDeserialize();
		public abstract void OnBeforeSerialize();
	}

	public abstract class Statement : Command<Void, Void> {
		public static IGUI.SelectMenu<CommandType> menu = new IGUI.SelectMenu<CommandType> {
			"Control flow",
			new CommandType(typeof(Sequential)),
			new CommandType(typeof(Conditional)),
			"Action",
			new CommandType(typeof(SwitchState)),
		};
	}

	public abstract class Condition : Command<object, bool> {
		public static IGUI.SelectMenu<CommandType> menu = new IGUI.SelectMenu<CommandType> {
			"Constant",
			new CommandType(typeof(BoolConstant)),
		};
	}
}