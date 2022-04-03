namespace Unapparent {
	public abstract class CertainListener : List {
		public override bool Validate(Carrier target, params object[] args) => true;
	}
}
