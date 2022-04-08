using System.Threading.Tasks;

namespace Unapparent {
	public class SendMessage : Statement {
		public Carrier target;
		public string message;

		public override Task<object> Execute(Carrier subject) {
			target.ReceiveMessage(message);
			return Task.FromResult<object>(null);
		}
	}
}
