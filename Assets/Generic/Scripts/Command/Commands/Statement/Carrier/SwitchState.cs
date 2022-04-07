using System.Threading.Tasks;

namespace Unapparent {
	public class SwitchState : Statement {
		public Carrier target;
		public State destinationState = null;

		public override async Task<object> Execute() =>
			await Task.Run(() => target.State = destinationState);
	}
}
