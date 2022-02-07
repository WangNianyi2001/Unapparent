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

	public abstract class Statement : Command<Void, Void> {
		public static IGUI.SelectMenu<Type> types = new IGUI.SelectMenu<Type> {
			"Control flow",
			typeof(Sequential),
			typeof(Conditional),
			"Action",
			typeof(SwitchState),
		};
	}

	public abstract class Expression<Ret> : Command<object, Ret> { }

	public abstract class Condition : Expression<bool> {
		public static IGUI.SelectMenu<Type> types = new IGUI.SelectMenu<Type> {
			"Constant",
			typeof(BoolConstant),
		};
	}
}