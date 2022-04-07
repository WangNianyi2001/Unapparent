using System.Threading.Tasks;
using UnityEngine;

namespace Unapparent {
	public class NavigateTo : Statement {
		public Character subject;
		public Transform destination;
		public float tolerance = 1f;
		public Statement arrival = null;

		public override async Task<object> Execute() {
			subject.Arrival = arrival;
			subject.NavigateTo(destination.position, tolerance);
			return null;
		}
	}
}
