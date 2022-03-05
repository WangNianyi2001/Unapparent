using UnityEngine;
using UnityEngine.AI;

namespace Unapparent {
	public class Protagonist : Character {
		public GameObject cameraObject;
		new Camera camera;

		void MouseDown() {
			Ray ray = camera.ScreenPointToRay(Input.mousePosition);
			RaycastHit walkableHit;
			Physics.Raycast(ray, out walkableHit, Mathf.Infinity, walkableLayerMask);
			if(walkableHit.collider != null)
				NavigateTo(walkableHit.point);
		}

		public new void Start() {
			base.Start();
			camera = cameraObject.GetComponent<Camera>();
		}

		public void Update() {
			if(Input.GetMouseButtonDown(0))
				MouseDown();
		}
	}
}
