using System.Threading.Tasks;
using UnityEngine;

namespace Unapparent {
	public class Navigate : Statement {
		public Character subject;
		public Transform destination;
		public float tolerance = 1f;

		public override async Task<object> Execute() =>
			await subject.Navigate(destination.position, tolerance);
	}
}
