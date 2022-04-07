using System.Threading.Tasks;

namespace Unapparent {
	public class SwitchState : Statement {
		public Carrier subject;
		public State targetState = null;

		public override async Task<object> Execute() =>
			await Task.Run(() => subject.State = targetState);
	}
}
