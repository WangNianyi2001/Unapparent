using System.Threading.Tasks;

namespace Unapparent {
	public class SucceedInteger : Statement {
		public new string name;

		public override Task<object> Execute(Carrier subject) =>
			Task.FromResult<object>(++Variable<int>.Get(name).value);
	}
}
