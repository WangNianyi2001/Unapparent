namespace Unapparent {
	public abstract class Constant<T> : Command {
		public T value = default(T);

		public override object Execute(Carrier target) => value;
	}
}
