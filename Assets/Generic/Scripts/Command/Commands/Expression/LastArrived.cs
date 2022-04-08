using System.Threading.Tasks;

namespace Unapparent {
	public class LastArrived : Expression {
		public Carrier subject;

		public override Task<object> Execute() => Task.FromResult<object>(subject.lastArrived);
	}
}
