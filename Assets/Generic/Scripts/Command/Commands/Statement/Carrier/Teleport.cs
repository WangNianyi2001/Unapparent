using System.Threading.Tasks;
using UnityEngine;

namespace Unapparent {
	public class Teleport : Statement {
		public Carrier subject;
		public Transform destination;

		public override async Task<object> Execute() =>
			await subject.Teleport(destination.position);
	}
}
