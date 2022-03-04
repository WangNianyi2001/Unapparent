using UnityEngine;
using UnityEngine.AI;

namespace Unapparent {
	public class Character : Carrier {
		protected static LayerMask walkableLayerMask;

		NavMeshAgent agent;

		public void NavigateTo(Vector3 location) {
			agent.SetDestination(location);
		}

		public void Awake() {
			walkableLayerMask = LayerMask.GetMask("Walkable");
		}

		public new void Start() {
			agent = GetComponent<NavMeshAgent>();
			base.Start();
		}
	}
}
