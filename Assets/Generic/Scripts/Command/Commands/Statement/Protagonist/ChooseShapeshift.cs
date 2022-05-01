using System.Threading.Tasks;

namespace Unapparent {
	public class ChooseShapeshift : Statement {
		public override Task<object> Execute(Carrier subject) =>
			Level.current.ui.ShowShapeshift();
	}
}
