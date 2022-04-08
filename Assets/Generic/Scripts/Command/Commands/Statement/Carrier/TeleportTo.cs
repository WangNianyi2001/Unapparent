using System.Threading.Tasks;
using UnityEngine;

namespace Unapparent {
	public class TeleportTo : Statement {
		public Transform destination;

		public override async Task<object> Execute(Carrier subject) =>
			await subject.TeleportTo(destination.position);
	}
}
