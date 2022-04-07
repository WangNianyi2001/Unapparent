namespace Unapparent {
	public abstract class CertainListener : Listener {
		public override bool Validate(params object[] args) => true;
	}
}
