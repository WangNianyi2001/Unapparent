using System.Threading.Tasks;
using UnityEngine;

namespace Unapparent {
	public class TeleportTo : Statement {
		public Carrier subject;
		public Transform destination;

		public override async Task<object> Execute() =>
			await subject.TeleportTo(destination.position);
	}
}
