namespace Unapparent {
	public abstract class CertainListener : Listener {
		public override bool Validate(Carrier target, params object[] args) => true;
	}
}
