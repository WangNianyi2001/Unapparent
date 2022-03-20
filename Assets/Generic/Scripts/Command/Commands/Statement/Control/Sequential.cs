namespace Unapparent {
	public class Sequential : Statement {
		public StatementList sequence;

		public override object Execute(Carrier target) {
			foreach(Command command in sequence.elements)
				command.Execute(target);
			return null;
		}
	}
}
