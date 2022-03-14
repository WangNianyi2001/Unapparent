using System.Linq;

namespace Unapparent {
	public class Logue : Command {
		public CommandList monologues;
		public Command next;

		public override object Execute(Carrier target) {
			var elements = monologues.elements;
			for(int i = 1; i < elements.Count; ++i)
				(elements[i - 1] as Logue).next = elements[i];
			(elements.Last() as Logue).next = null;
			return elements.Count == 0 ? null : elements[0].Execute(target);
		}
	}
}
