using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;
using System;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Unapparent {
	public class Character : Carrier {
		protected NavMeshAgent agent;

		[SerializeField] Identity identity;
		Identity appearance;

		Billboard billboard;
		Material billboardMat;

		public Identity Appearance {
			get => appearance;
			set {
				appearance = value;
				UpdateBillboardOrientation(true);
			}
		}

		float orientation = 1;
		void UpdateBillboardOrientation(bool force = false) {
			float x = Vector3.Dot(agent.velocity, billboard.right);
			if(!force && x * orientation >= 0)
				return;
			orientation = x > 0 ? 1 : -1;
			Vector3 scale = billboard.transform.localScale;
			if(orientation > 0) {
				scale.x = 1;
				billboardMat.SetTexture("_MainTex", appearance.right);
			} else {
				scale.x = -1;
				billboardMat.SetTexture("_MainTex", appearance.left);
			}
			billboard.transform.localScale = scale;
		}

		public Task<object> TeleportTo(Transform target) {
			NavMeshHit hit;
			NavMesh.SamplePosition(target.position, out hit, Mathf.Infinity, NavMesh.AllAreas);
			return Task.FromResult<object>(agent.Warp(hit.position));
		}

		public async Task<object> NavigateTo(Vector3 position, float tolerance = 1f) {
			agent.stoppingDistance = tolerance;
			agent.SetDestination(position);
			agent.isStopped = false;
			while(Application.isPlaying && !agent.isStopped) {
				if(agent.remainingDistance <= agent.stoppingDistance)
					break;
				await Task.Delay(100);
			}
			return null;
		}

		public void StopNavigation() => agent.isStopped = true;

		public new void Start() {
			base.Start();
			agent = GetComponent<NavMeshAgent>();
			billboard = GetComponentInChildren<Billboard>();
			MeshRenderer renderer = billboard.GetComponentInChildren<MeshRenderer>();
			renderer.sharedMaterial = Resources.Load<Material>("Visual/Transparent Diffuse");
			billboardMat = renderer.material;
			Appearance = identity;
		}

		public void FixedUpdate() {
			UpdateBillboardOrientation();
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
