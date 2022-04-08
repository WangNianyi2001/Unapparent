using System.Threading.Tasks;

namespace Unapparent {
	public class ShapeshiftInto : Statement {
		public Identity target;

		public override Task<object> Execute(Carrier subject) =>
			Task.FromResult<object>((subject as Protagonist).Shapeshift(target));
	}
}
