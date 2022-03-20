namespace Unapparent {
	public class Conditional : Statement {
		public Expression condition = null;

		public Statement trueBranch = null, falseBranch = null;

		public override object Execute(Carrier target) =>
			((bool)condition?.Execute(target) ? trueBranch : falseBranch)?.Execute(target);
	}
}
