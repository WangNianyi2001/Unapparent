using System.Threading.Tasks;

namespace Unapparent {
	public class StopNavigation : Statement {
		public override Task<object> Execute(Carrier subject) {
			(subject as Character).StopNavigation();
			return Task.FromResult<object>(null);
		}
	}
}
