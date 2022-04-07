using System.Threading.Tasks;

namespace Unapparent {
	public class LastArrived : Expression {
		public Carrier subject;

		public override async Task<object> Execute() =>
			await Task.Run(() => subject.lastArrived);
	}
}
