using System.Threading.Tasks;

namespace Unapparent {
	public class UnlockIdentity : Statement {
		public Identity target;

		public override Task<object> Execute(Carrier subject) =>
			Task.FromResult<object>((subject as Protagonist).shapeshiftables.Add(target));
	}
}
