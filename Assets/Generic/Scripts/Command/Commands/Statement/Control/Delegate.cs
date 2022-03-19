namespace Unapparent {
	public class Delegate : Statement {
		public System.Action action;

		public override object Execute(Carrier target) {
			action?.Invoke();
			return null;
		}
	}
}
