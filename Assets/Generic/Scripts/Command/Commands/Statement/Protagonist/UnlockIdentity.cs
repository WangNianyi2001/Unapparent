using System.Threading.Tasks;

namespace Unapparent {
	public class UnlockIdentity : Statement {
		public Identity target;

		public override Task<object> Execute(Carrier subject) {
			Protagonist protagonist = subject as Protagonist;
			if(!protagonist.shapeshiftables.Contains(target))
				protagonist.shapeshiftables.Add(target);
			return Task.FromResult<object>(null);
		}
	}
}
