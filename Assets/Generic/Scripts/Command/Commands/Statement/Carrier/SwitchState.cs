using System.Threading.Tasks;

namespace Unapparent {
	public class SwitchState : Statement {
		public Carrier subject;
		public State targetState = null;

		public override Task<object> Execute() => Task.FromResult<object>(subject.State = targetState);
	}
}
