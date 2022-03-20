namespace Unapparent {
	public class SwitchState : Statement {
		public State destinationState = null;

		public override object Execute(Carrier target) =>
			target.State = destinationState;
	}
}
