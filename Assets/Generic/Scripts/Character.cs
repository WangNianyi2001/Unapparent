using UnityEngine;
using UnityEngine.AI;

namespace Unapparent {
	public class Character : Carrier {
		protected static LayerMask walkableLayerMask;

		NavMeshAgent agent;

		Command arrival = null;
		public Command Arrival {
			set => arrival = value;
		}

		public string displayName;

		public float checkFrequency = .5f;

		void CheckArrival() {
			if(arrival == null)
				return;
			if(agent.remainingDistance <= agent.stoppingDistance) {
				arrival.Execute(this);
				arrival = null;
				return;
			}
			Invoke("CheckArrival", checkFrequency);
		}

		public void NavigateTo(Vector3 location) {
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
