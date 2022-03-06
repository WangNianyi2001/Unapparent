using System;

namespace Unapparent {
	public class Delegate : Command {
		public Action action;

		public override object Execute(Carrier target) {
			action?.Invoke();
			return null;
		}

		public override void Inspect(ArgList<Action> elements) {
			IGUI.Italic("This command is only for internal use.");
			throw new NotImplementedException();
		}
	}
}
