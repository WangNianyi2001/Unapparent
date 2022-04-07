using System.Threading.Tasks;

namespace Unapparent {
	public class Shapeshift : Statement {
		public Protagonist subject;
		public Identity targetIdentity;

		public override async Task<object> Execute() {
			await Task.Delay(1);
			return subject.Shapeshift(targetIdentity);
		}
	}
}
