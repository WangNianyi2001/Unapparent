using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;

namespace Unapparent {
	public class Character : Carrier {
		protected static LayerMask walkableLayerMask;

		NavMeshAgent agent;

		public Identity identity;
		public virtual Identity appearance => identity;

		public async Task<object> NavigateTo(Vector3 location, float tolerance = 1f) {
			agent.stoppingDistance = tolerance;
			agent.SetDestination(location);
			while(Application.isPlaying) {
				await Task.Delay(100);
				if(agent.remainingDistance <= agent.stoppingDistance)
					return true;
			}
			return false;
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
