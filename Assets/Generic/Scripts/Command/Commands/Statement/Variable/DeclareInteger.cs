using System.Threading.Tasks;

namespace Unapparent {
	public class DeclareInteger : Statement {
		public new string name;
		public int value;

		public override Task<object> Execute(Carrier subject) =>
			Task.FromResult<object>(Variable<int>.Create(name, value));
	}
}
