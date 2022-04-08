using System.Threading.Tasks;

namespace Unapparent {
	public class Subjecting : Statement {
		public Carrier subject;
		public Statement action;

		public override async Task<object> Execute(Carrier subject) =>
			await action?.Execute(this.subject);
	}
}
