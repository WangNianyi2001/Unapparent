using System.Threading.Tasks;

namespace Unapparent {
	public class Shapeshift : Statement {
		public Protagonist subject;
		public Identity targetIdentity;

		public override Task<object> Execute() =>
			Task.FromResult<object>(subject.Shapeshift(targetIdentity));
	}
}
