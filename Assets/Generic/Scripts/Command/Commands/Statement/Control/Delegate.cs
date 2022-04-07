using System;
using System.Threading.Tasks;

namespace Unapparent {
	public class Delegate : Statement {
		public Action action;

		public override async Task<object> Execute() => await Task.Run<object>(() => {
			action?.Invoke();
			return null;
		});
	}
}
