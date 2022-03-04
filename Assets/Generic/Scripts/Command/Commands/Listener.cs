using System;

namespace Unapparent {
	public abstract class Listener : Command {
		public static Listener Create() {
			Listener listener = Create(typeof(Listener)) as Listener;
			listener.command = Create(typeof(Sequential));
			return listener;
		}

		public Command command;

		public abstract bool Validate(Carrier target);

		public void TryExecute(Carrier target) {
			if(Validate(target))
				Execute(target);
		}

		public override object Execute(Carrier target) => command?.Execute(target);

		public override void Inspect(ArgList<Action> elements) {
			IGUI.Inline(() => {
				IGUI.Label("When");
			});
			command?.Inspect(() => IGUI.Label("Do"));
		}

		public override void Dispose() {
			command?.Dispose();
			base.Dispose();
		}
	}
}
