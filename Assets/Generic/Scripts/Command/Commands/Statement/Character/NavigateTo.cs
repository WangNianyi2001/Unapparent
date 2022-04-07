using System.Threading.Tasks;
using UnityEngine;

namespace Unapparent {
	public class NavigateTo : Statement {
		public Character subject;
		public Transform destination;
		public float tolerance = 1f;

		public override async Task<object> Execute() =>
			await subject.NavigateTo(destination.position, tolerance);
	}
}
