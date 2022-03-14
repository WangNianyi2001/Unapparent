namespace Unapparent {
	public class Reference : Command {
		public UnityEngine.Object selected;
		public Command command = null;

		public override object Execute(Carrier target) {
			return command?.Execute(target);
		}
	}
}
