namespace Unapparent {
	public abstract class Constant<T> : Expression {
		public T value = default(T);

		public override object Execute(Carrier target) => value;
	}
}
