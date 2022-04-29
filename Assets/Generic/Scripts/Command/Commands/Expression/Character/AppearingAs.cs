using System.Threading.Tasks;

namespace Unapparent {
	public class AppearingAs : Expression {
		public Identity expected;

		public override Task<object> Execute(Carrier subject) =>
			Task.FromResult<object>((subject as Character).Appearance == expected);
	}
}
