using System.Collections.Generic;

namespace Unapparent {
	public class Sequential : Statement {
		public List<Statement> sequence;

		public override object Execute(Carrier target) {
			foreach(Command command in sequence)
				command.Execute(target);
			return null;
		}
	}
}
