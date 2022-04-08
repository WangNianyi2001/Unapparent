using System.Threading.Tasks;

namespace Unapparent {
	public class LastArrived : Expression {
		public override Task<object> Execute(Carrier subject) => Task.FromResult<object>(subject.lastArrived);
	}
}
