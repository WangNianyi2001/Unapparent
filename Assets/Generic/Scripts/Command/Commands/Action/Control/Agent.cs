using System;

namespace Unapparent {
	public class Agent : Command {
		Carrier carrier;
		Command command;

		public override object Execute(Carrier target) => command?.Execute(carrier);
	}
}
