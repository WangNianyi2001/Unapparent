using System;

namespace Unapparent {
	public class Delegate : Command {
		public Action action;

		public override object Execute(Carrier target) {
			action?.Invoke();
			return null;
		}
	}
}
