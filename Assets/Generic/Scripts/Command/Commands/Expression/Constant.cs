using System.Threading.Tasks;

namespace Unapparent {
	public abstract class Constant<T> : Expression {
		public T value = default(T);

		public override async Task<object> Execute() => value;
	}
}
