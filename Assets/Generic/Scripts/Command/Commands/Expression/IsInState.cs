using System.Threading.Tasks;

namespace Unapparent {
	public class IsInState : Expression {
		public Carrier subject;
		public State target;

		public override Task<object> Execute() =>
			Task.FromResult<object>(target.isParentOf(subject.State));
	}
}
