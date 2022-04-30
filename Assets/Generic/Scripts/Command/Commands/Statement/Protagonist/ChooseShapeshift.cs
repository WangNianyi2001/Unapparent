using System.Threading.Tasks;
using UnityEngine;

namespace Unapparent {
	public class ChooseShapeshift : Statement {
		public override Task<object> Execute(Carrier subject) =>
			Level.current.ui.ShowShapeshift(subject as Protagonist);
	}
}
