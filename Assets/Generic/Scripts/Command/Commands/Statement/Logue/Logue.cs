using System.Collections.Generic;
using System.Linq;

namespace Unapparent {
	public class Logue : Statement {
		public List<Command> monologues;
		public Command next;

		public override object Execute(Carrier target) {
			for(int i = 1; i < monologues.Count; ++i)
				(monologues[i - 1] as Logue).next = monologues[i];
			(monologues.Last() as Logue).next = null;
			return monologues.Count == 0 ? null : monologues[0].Execute(target);
		}
	}
}
