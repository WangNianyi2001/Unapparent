using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace Unapparent {
	public class Void { }

	public interface ICommand<Ret> : IInspectable, ISerializationCallbackReceiver {
		public Ret Execute();
	}

	public class CommandType {
		public readonly Type type;

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

		public static CommandType GetByName(string name) {
			return table[name];
		}
	}

	public abstract class Command<Ret> : ICommand<Ret> {
		public CommandType type { get => GetType(); }

		public abstract Ret Execute();

		public abstract void Inspect(Action header, Action footer);

		static bool IsSubclassOfRawGeneric(Type generic, Type toCheck) {
			while(toCheck != null && toCheck != typeof(object)) {
				var cur = toCheck.IsGenericType ? toCheck.GetGenericTypeDefinition() : toCheck;
				if(generic == cur) {
					return true;
				}
				toCheck = toCheck.BaseType;
			}
			return false;
		}

		public virtual void OnAfterDeserialize() {
			foreach(FieldInfo field in type.type.GetFields()) {
				Type type = field.FieldType;
				if(IsSubclassOfRawGeneric(typeof(Command<>), type)) {
					//
				}
			}
		}

		public virtual void OnBeforeSerialize() { }
	}

	public abstract class Statement : Command<Void> {
		public static IGUI.SelectMenu<CommandType> menu = new IGUI.SelectMenu<CommandType> {
			"Control flow",
			new CommandType(typeof(Sequential)),
			new CommandType(typeof(Conditional)),
			"Action",
			new CommandType(typeof(SwitchState)),
		};
	}

	public abstract class Condition : Command<bool> {
		public static IGUI.SelectMenu<CommandType> menu = new IGUI.SelectMenu<CommandType> {
			"Constant",
			new CommandType(typeof(BoolConstant)),
		};
	}
}
