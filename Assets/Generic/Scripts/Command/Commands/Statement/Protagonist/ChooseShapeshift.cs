using System.Threading.Tasks;

namespace Unapparent {
	public class ChooseShapeshift : Statement {
		public override async Task<object> Execute(Carrier subject) =>
			await Level.current.ui.ShowShapeshift();
	}
}
