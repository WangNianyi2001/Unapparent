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
		Billboard billboard;
		Material billboardMat;

		public Identity identity;
		public virtual Identity appearance => identity;

		public override Task<object> TeleportTo(Vector3 position) =>
			Task.FromResult<object>(lastArrived = agent.Warp(position));

		public async Task<object> NavigateTo(Vector3 position, float tolerance = 1f) {
			lastArrived = false;
			agent.stoppingDistance = tolerance;
			agent.SetDestination(position);
			agent.isStopped = false;
			while(Application.isPlaying && !agent.isStopped) {
				if(agent.remainingDistance <= agent.stoppingDistance) {
					lastArrived = agent.pathStatus == NavMeshPathStatus.PathComplete;
					break;
				}
				await Task.Delay(100);
			}
			return lastArrived;
		}

		public void StopNavigation() => agent.isStopped = true;

		public new void Start() {
			base.Start();
			agent = GetComponent<NavMeshAgent>();
			billboard = GetComponentInChildren<Billboard>();
			MeshRenderer renderer = billboard.GetComponentInChildren<MeshRenderer>();
			renderer.sharedMaterial = Resources.Load<Material>("Visual/Transparent Diffuse");
			billboardMat = renderer.material;
			billboardMat.SetTexture("_MainTex", identity.right);
		}

#if UNITY_EDITOR
		public void OnDrawGizmos() {
			string text = identity == null ? "(No identity)" : identity.name;
			if(appearance != null && identity != appearance)
				text += $" ({appearance.name})";
			Handles.Label(transform.position + transform.up, text);
		}
#endif
	}
}
