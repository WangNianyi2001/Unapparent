using System.Threading.Tasks;

namespace Unapparent {
	public class Conditional : Statement {
		public Expression condition = null;

		public Statement trueBranch = null, falseBranch = null;

		public override async Task<object> Execute() =>
			((bool)await condition?.Execute() ? trueBranch : falseBranch)?.Execute();
	}
}
