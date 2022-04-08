using System;
using System.Threading.Tasks;

namespace Unapparent {
	public class Delegate : Statement {
		public Action action;

		public override Task<object> Execute(Carrier subject) {
			action?.Invoke();
			return Task.FromResult<object>(null);
		}
	}
}
