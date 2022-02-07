using System;
using System.Collections.Generic;
using UnityEngine;

namespace Unapparent {
	public class Void { }

	public interface ICommand<Arg, Ret> : IInspectable, ISerializationCallbackReceiver {
		public Ret Execute(Arg arg);
	}

	public abstract class Command<Arg, Ret> : ICommand<Arg, Ret> {
		public abstract Ret Execute(Arg arg);
		public abstract void Inspect(Action header, Action footer);
		public abstract void OnAfterDeserialize();
		public abstract void OnBeforeSerialize();
	}

	public class CommandType {
		Type type;
		public CommandType(Type type) => this.type = type;
		public static implicit operator Type(CommandType type) => type.type;
		public override string ToString() => type.Name;
	}

	public abstract class Statement : Command<Void, Void> {
		public static IGUI.SelectMenu<CommandType> types = new IGUI.SelectMenu<CommandType> {
			"Control flow",
			new CommandType(typeof(Sequential)),
			new CommandType(typeof(Conditional)),
			"Action",
			new CommandType(typeof(SwitchState)),
		};
	}

	public abstract class Expression<Ret> : Command<object, Ret> { }

	public abstract class Condition : Expression<bool> {
		public static IGUI.SelectMenu<CommandType> types = new IGUI.SelectMenu<CommandType> {
			"Constant",
			new CommandType(typeof(BoolConstant)),
		};
	}
}