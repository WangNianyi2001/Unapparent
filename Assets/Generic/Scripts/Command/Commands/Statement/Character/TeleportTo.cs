using System.Threading.Tasks;
using UnityEngine;

namespace Unapparent {
	public class TeleportTo : Statement {
		public Transform destination;

		public override async Task<object> Execute(Carrier subject) {
			Location before = Location.Of(subject.transform);
			object res = await (subject as Character)?.TeleportTo(destination);
			Location after = Location.Of(subject.transform);
			if(before != after) {
				subject.AddToFireQueue(subject.State, typeof(LeaveLocation), before);
				subject.AddToFireQueue(subject.State, typeof(ArriveLocation), after);
			}
			return res;
		}
	}
}
