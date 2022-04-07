using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Unapparent {
	public class Character : Carrier {
		[NonSerialized] public NavMeshAgent agent;

		public Identity identity;
		public virtual Identity appearance => identity;

		public override async Task<object> Teleport(Vector3 position) {
			await Task.Delay(1);
			return lastArrived = agent.Warp(position);
		}

		public async Task<object> Navigate(Vector3 position, float tolerance = 1f) {
			lastArrived = false;
			agent.stoppingDistance = tolerance;
			agent.SetDestination(position);
			agent.isStopped = false;
			while(Application.isPlaying) {
				await Task.Delay(100);
				if(agent.remainingDistance <= agent.stoppingDistance) {
					lastArrived = agent.pathStatus == NavMeshPathStatus.PathComplete;
					break;
				}
			}
			return lastArrived;
		}

		public new void Start() {
			base.Start();
			agent = GetComponent<NavMeshAgent>();
		}

#if UNITY_EDITOR
		public void OnDrawGizmos() {
			string text = identity.name;
			if(appearance != null && identity != appearance)
				text += $" ({appearance.name})";
			Handles.Label(transform.position + transform.up, text);
		}
#endif
	}
}
