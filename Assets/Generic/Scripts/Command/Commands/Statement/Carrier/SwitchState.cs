using System.Threading.Tasks;

namespace Unapparent {
	public class SwitchState : Statement {
		public State targetState = null;

		public override Task<object> Execute(Carrier subject) => Task.FromResult<object>(subject.State = targetState);
	}
}
