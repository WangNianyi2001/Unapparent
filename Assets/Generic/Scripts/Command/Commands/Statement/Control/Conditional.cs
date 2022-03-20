namespace Unapparent {
	public class Conditional : Statement {
		public Command condition = null;

		public Command trueBranch = null, falseBranch = null;

		public override object Execute(Carrier target) =>
			((bool)condition?.Execute(target) ? trueBranch : falseBranch)?.Execute(target);
	}
}
