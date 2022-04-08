using System.Threading.Tasks;
using UnityEngine;

namespace Unapparent {
	public class NavigateTo : Statement {
		public Transform destination;
		public float tolerance = 1f;

		public override async Task<object> Execute(Carrier subject) =>
			await (subject as Character).NavigateTo(destination.position, tolerance);
	}
}
