using UnityEngine;
using UnityEngine.AI;

namespace Unapparent {
	public class Character : Carrier {
		protected static LayerMask walkableLayerMask;

		NavMeshAgent agent;

		Statement arrival = null;
		public Statement Arrival {
			set => arrival = value;
		}

		public Identity identity;
		public virtual Identity appearance => identity;

		public float checkFrequency = .5f;

		void CheckArrival() {
			if(agent.remainingDistance <= agent.stoppingDistance) {
				arrival?.Execute();
				return;
			}
			Invoke("CheckArrival", checkFrequency);
		}

		public void NavigateTo(Vector3 location, float tolerance = 1f) {
			agent.stoppingDistance = tolerance;
			agent.SetDestination(location);
			Invoke("CheckArrival", checkFrequency);
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
