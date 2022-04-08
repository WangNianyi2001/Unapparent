using System.Threading.Tasks;

namespace Unapparent {
	public class IsInState : Expression {
		public State target;

		public override Task<object> Execute(Carrier subject) =>
			Task.FromResult<object>(target.isParentOf(subject.State));
	}
}
