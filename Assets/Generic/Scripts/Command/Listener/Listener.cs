using System;

namespace Unapparent {
	public abstract class Listener : Command {
		public static new Listener Create(Type type) {
			Listener listener = Command.Create(type) as Listener;
			listener.command = Command.Create(typeof(Sequential));
			return listener;
		}

		public Command command;

		public abstract bool Validate(Carrier target);

		public virtual void TryExecute(Carrier target) {
			if(Validate(target))
				Execute(target);
		}

		public override object Execute(Carrier target) => command?.Execute(target);
	}

	public abstract class CertainListener : Listener {
		public override bool Validate(Carrier target) => true;
		public override void TryExecute(Carrier target) => Execute(target);
	}
}
