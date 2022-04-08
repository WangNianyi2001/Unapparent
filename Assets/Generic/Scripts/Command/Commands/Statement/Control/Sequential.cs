using System.Collections.Generic;
using System.Threading.Tasks;

namespace Unapparent {
	public class Sequential : Statement {
		public List<Statement> sequence = new List<Statement>();

		public override async Task<object> Execute(Carrier subject) {
			foreach(Command command in sequence)
				await command.Execute(subject);
			return null;
		}
	}
}
