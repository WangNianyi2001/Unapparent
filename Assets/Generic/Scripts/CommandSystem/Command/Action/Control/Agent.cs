using System;

namespace Unapparent {
	public class Agent : Command {
		Carrier carrier;
		Command command;

		public override object Execute(Carrier target) => command?.Execute(carrier);

		public override void Inspect(ArgList<Action> elements) {
			if(command == null) {
				command = Create(typeof(Sequential));
				SetDirty();
			}
			IGUI.Inline(() => {
				elements[0]?.Invoke();
				IGUI.Label("Agent");
				if(IGUI.ObjectField(ref carrier, typeof(Carrier), true))
					SetDirty();
			});
			command?.Inspect(null, elements[1]);
		}

		public override void Dispose() {
			command?.Dispose();
			base.Dispose();
		}
	}
}
