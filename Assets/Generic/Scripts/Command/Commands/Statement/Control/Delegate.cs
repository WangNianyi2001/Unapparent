using System.Threading.Tasks;

namespace Unapparent {
	public class Delegate : Statement {
		public System.Action action;

		public override async Task<object> Execute() {
			action?.Invoke();
			return null;
		}
	}
}
