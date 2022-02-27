using UnityEngine;
using UnityEngine.AI;

namespace Unapparent {
	public class Protagonist : Carrier {
		static LayerMask walkableLayerMask;

		public GameObject cameraObject;
		new Camera camera;

		NavMeshAgent agent;

		void NavigateTo(Vector3 location) {
			agent.SetDestination(location);
		}

		void MouseDown() {
			Ray ray = camera.ScreenPointToRay(Input.mousePosition);
			RaycastHit walkableHit;
			Physics.Raycast(ray, out walkableHit, Mathf.Infinity, walkableLayerMask);
			if(walkableHit.collider != null)
				NavigateTo(walkableHit.point);
		}

		public void Awake() {
			walkableLayerMask = LayerMask.GetMask("Walkable");
		}

		public new void Start() {
			base.Start();
			camera = cameraObject.GetComponent<Camera>();
			agent = GetComponent<NavMeshAgent>();
		}

		public void Update() {
			if(Input.GetMouseButtonDown(0))
				MouseDown();
		}
	}
}
