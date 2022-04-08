using System.Threading.Tasks;

namespace Unapparent {
	public class Async : Statement {
		public Statement action;

		public override Task<object> Execute(Carrier subject) => Task.FromResult<object>(action.Execute(subject));
	}
}
