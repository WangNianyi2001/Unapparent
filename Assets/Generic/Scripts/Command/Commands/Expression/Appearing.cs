using System.Threading.Tasks;

#pragma warning disable 1998

namespace Unapparent {
	public class Appearing : Expression {
		public Character subject;
		public Identity expected;

		public override async Task<object> Execute() {
			bool res = subject.appearance == expected;
			return res;
		}
	}
}
