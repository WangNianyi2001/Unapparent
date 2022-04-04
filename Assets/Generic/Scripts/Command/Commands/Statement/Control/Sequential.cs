using System.Collections.Generic;
using System.Threading.Tasks;

namespace Unapparent {
	public class Sequential : Statement {
		public List<Statement> sequence;

		public override async Task<object> Execute() {
			foreach(Command command in sequence)
				await command.Execute();
			return null;
		}
	}
}
