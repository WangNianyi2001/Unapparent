namespace Unapparent {
	public class CloseLogue : Statement {
		public override object Execute(Carrier target) {
			Level.current.CloseMonologue();
			return null;
		}
	}
}
