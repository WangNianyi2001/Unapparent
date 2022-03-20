namespace Unapparent {
	public abstract class CertainListener : Listener {
		public override bool Validate(Carrier target) => true;
		public override void TryExecute(Carrier target) => Execute(target);
	}
}
