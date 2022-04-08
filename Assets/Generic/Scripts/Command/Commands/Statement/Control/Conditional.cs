using System.Threading.Tasks;

namespace Unapparent {
	public class Conditional : Statement {
		public Expression @if = null;

		public Statement then = null, @else = null;

		public override async Task<object> Execute(Carrier subject) =>
			((bool)await @if?.Execute(subject) ? then : @else)?.Execute(subject);
	}
}
